using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    public static void welcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }
    public static void PlayerMovement(bool[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.PlayerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach(bool _input in _inputs)
            {
                _packet.Write(_input);
            }
            _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);

            SendUDPData(_packet);
        }
    }
    public static void PlayerShoot(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.PlayerShoot))
        {
            _packet.Write(_facing);

            SendTCPData(_packet);
        }
    }
    public static void PlayerThrowItem(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.PlayerThrowItem))
        {
            _packet.Write(_facing);

            SendTCPData(_packet);
        }
    }

    #endregion
}
/*
  public static void UDPTestReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.udpTestReceive))
        {
            _packet.Write("Received a UDP packet.");

            SendUDPData(_packet);
        }
    }

    { (int)ServerPackets.udpTest,ClientHandle.UDPTest }

    public static void UDPTest(Packet _packet)
    {
        string msg = _packet.ReadString();

        Debug.Log($"Received packet via UDP, message: {msg}");
        ClientSend.UDPTestReceived();
    }
*/
