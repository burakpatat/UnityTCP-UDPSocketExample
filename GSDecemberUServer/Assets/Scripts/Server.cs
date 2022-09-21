using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class Server
{
    public static int MaxPlayers { get; private set; }

    public static int Port { get; private set; }
    public static Dictionary<int, Client> Clients = new Dictionary<int, Client>();
    public delegate void PacketHandler(int _fromClientId, Packet _packet);
    public static Dictionary<int, PacketHandler> packetHandlers;

    private static TcpListener tcpListener;
    private static UdpClient UdpListener;

    public static void Start(int _maxPlayer, int _port)
    {
        MaxPlayers = _maxPlayer;
        Port = _port;

        Debug.Log("Starting Server...");
        ServerLog.instance.WriteLog("Starting Server...");
        InitializeServerData();

        tcpListener = new TcpListener(IPAddress.Any, Port);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);

        UdpListener = new UdpClient(Port);
        UdpListener.BeginReceive(UDPReceiveCallback, null);

        Debug.Log($"Server Started On {Port}");
        ServerLog.instance.WriteLog($"Server Started On {Port}");
    }
    private static void TCPConnectCallback(IAsyncResult _result)
    {
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
        tcpListener.BeginAcceptTcpClient(new AsyncCallback(TCPConnectCallback), null);
        Debug.Log($"Incoming connection from {_client.Client.RemoteEndPoint}...");
        ServerLog.instance.WriteLog($"Incoming connection from {_client.Client.RemoteEndPoint}...");

        for (int i = 1; i <= MaxPlayers; i++)
        {
            if (Clients[i].tcp.socket == null)
            {
                Clients[i].tcp.Connect(_client);
                return;
            }
        }

        Debug.Log($"{_client.Client.RemoteEndPoint} failed to connect: Server is full!");
        ServerLog.instance.WriteLog($"{_client.Client.RemoteEndPoint} failed to connect: Server is full!");
    }

    private static void UDPReceiveCallback(IAsyncResult _result)
    {
        try
        {
            IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] _data = UdpListener.EndReceive(_result, ref _clientEndPoint);
            UdpListener.BeginReceive(UDPReceiveCallback, null);

            if (_data.Length < 4)
            {
                return;
            }

            using (Packet _packet = new Packet(_data))
            {
                int _clientId = _packet.ReadInt();

                if (_clientId == 0)
                {
                    return;
                }

                if (Clients[_clientId].udp.endPoint == null)
                {
                    Clients[_clientId].udp.Connect(_clientEndPoint);
                    return;
                }

                if (Clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString())
                {
                    Clients[_clientId].udp.HandleData(_packet);
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Error receiving UDP data: {e}");
            ServerLog.instance.WriteLog($"Error receiving UDP data: {e}");
        }
    }

    public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet)
    {
        try
        {
            if (_clientEndPoint != null)
            {
                UdpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
            }
        }
        catch (Exception e)
        {
            Debug.Log($"Error sending data to {_clientEndPoint} via UDP: {e}");
            ServerLog.instance.WriteLog($"Error sending data to {_clientEndPoint} via UDP: {e}");
        }
    }

    private static void InitializeServerData()
    {
        for (int i = 1; i <= MaxPlayers; i++)
        {
            Clients.Add(i, new Client(i));
        }

        packetHandlers = new Dictionary<int, PacketHandler>()
            {
                { (int)ClientPackets.welcomeReceived,ServerHandle.WelcomeReceived },
                { (int)ClientPackets.PlayerMovement,ServerHandle.PlayerMovementReceived },
                { (int)ClientPackets.PlayerShoot,ServerHandle.PlayerShootReceived },
                { (int)ClientPackets.PlayerThrowItem,ServerHandle.PlayerThrowItem },
            };
        Debug.Log("Initialized Packets.");
        ServerLog.instance.WriteLog("Initialized Packets.");
    }
    public static void Stop()
    {
        tcpListener.Stop();
        UdpListener.Close();
    }
}
