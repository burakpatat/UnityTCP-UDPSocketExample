using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class ServerHandle
{
    public static void WelcomeReceived(int _fromClientId, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Debug.Log($"{Server.Clients[_fromClientId].tcp.socket.Client.RemoteEndPoint} connected succesfully and is now player {_fromClientId}");
        ServerLog.instance.WriteLog($"{Server.Clients[_fromClientId].tcp.socket.Client.RemoteEndPoint} connected succesfully and is now player {_fromClientId}");
        if (_fromClientId != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClientId}) has assumed the wrong client ID ({_clientIdCheck})!");
            ServerLog.instance.WriteLog($"Player \"{_username}\" (ID: {_fromClientId}) has assumed the wrong client ID ({_clientIdCheck})!");
        }
        //send player into game ->
        Server.Clients[_fromClientId].SendIntoGame(_username);
    }

    public static void PlayerMovementReceived(int _fromClientId, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }

        Quaternion _rotation = _packet.ReadQuaternion();

        Server.Clients[_fromClientId].player.SetInput(_inputs, _rotation);
    }
    public static void PlayerShootReceived(int _fromClientId, Packet _packet)
    {
        Vector3 _shootDirection = _packet.ReadVector3();

        Server.Clients[_fromClientId].player.Shoot(_shootDirection);
    }
    public static void PlayerThrowItem(int _fromClientId, Packet _packet)
    {
        Vector3 _throwDirection = _packet.ReadVector3();

        Server.Clients[_fromClientId].player.ThrowItem(_throwDirection);
    }
}
