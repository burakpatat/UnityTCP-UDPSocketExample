using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class ServerSend
{
    private static void SendTCPData(int _clientId, Packet _packet)
    {
        _packet.WriteLength();
        Server.Clients[_clientId].tcp.SendData(_packet);
    }
    private static void SendUDPData(int _clientId, Packet _packet)
    {
        _packet.WriteLength();
        Server.Clients[_clientId].udp.SendData(_packet);
    }

    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.Clients[i].tcp.SendData(_packet);
        }
    }
    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.Clients[i].tcp.SendData(_packet);
            }
        }
    }

    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.Clients[i].udp.SendData(_packet);
        }
    }
    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i != _exceptClient)
            {
                Server.Clients[i].udp.SendData(_packet);
            }
        }
    }

    #region Packets
    public static void Welcome(int _clientId, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_clientId);

            SendTCPData(_clientId, _packet);
        }
    }
    public static void SpawnPlayer(int _clientId, Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.SpawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            SendTCPData(_clientId, _packet);
        }
    }
    public static void PlayerPosition(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.PlayerPosition))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.position);

            SendUDPDataToAll(_packet);
        }
    }
    public static void PlayerRotation(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.PlayerRotation))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.rotation);

            SendUDPDataToAll(_player.id, _packet);
        }
    }
    public static void PlayerDisconnected(int _playerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerDisconnected))
        {
            _packet.Write(_playerId);

            SendTCPDataToAll(_packet);
        }
    }
    public static void PlayerHealth(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.PlayerHealth))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.health);

            SendTCPDataToAll(_packet);
        }
    }
    public static void PlayerRespawned(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.PlayerRespawn))
        {
            _packet.Write(_player.id);

            SendTCPDataToAll(_packet);
        }
    }
    public static void CreateItemSpawner(int _toClient, int _spawnerId, Vector3 _spawnerPosition, bool _hasItem)
    {
        using (Packet _packet = new Packet((int)ServerPackets.CreateItemSpawner))
        {
            _packet.Write(_spawnerId);
            _packet.Write(_spawnerPosition);
            _packet.Write(_hasItem);

            SendTCPData(_toClient,_packet);
        }
    }
    public static void ItemSpawned(int _spawnerId)
    {
        using (Packet _packet = new Packet((int)ServerPackets.ItemSpawned))
        {
            _packet.Write(_spawnerId);

            SendTCPDataToAll(_packet);
        }
    }
    public static void ItemPickedUp(int _spawnerId, int _byPlayer)
    {
        using (Packet _packet = new Packet((int)ServerPackets.ItemPickedUp))
        {
            _packet.Write(_spawnerId);
            _packet.Write(_byPlayer);

            SendTCPDataToAll(_packet);
        }
    }
    public static void SpawnProjecTile(ProjectTile _projectTile, int _thrownByPlayer)
    {
        using (Packet _packet = new Packet((int)ServerPackets.SpawnProjectTile))
        {
            _packet.Write(_projectTile.id);
            _packet.Write(_projectTile.transform.position);
            _packet.Write(_thrownByPlayer);

            SendTCPDataToAll(_packet);
        }
    }
    public static void ProjecTilePosition(ProjectTile _projectTile)
    {
        using (Packet _packet = new Packet((int)ServerPackets.ProjectTilePosition))
        {
            _packet.Write(_projectTile.id);
            _packet.Write(_projectTile.transform.position);

            SendUDPDataToAll(_packet);
        }
    }
    public static void ProjectTileExploded(ProjectTile _projectTile)
    {
        using (Packet _packet = new Packet((int)ServerPackets.ProjecTileExpolded))
        {
            _packet.Write(_projectTile.id);
            _packet.Write(_projectTile.transform.position);

            SendTCPDataToAll(_packet);
        }
    }
    public static void SpawnEnemy(Enemy _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.SpawnEnemy))
        {
            SendTCPDataToAll(SpawnEnemy_Data(_enemy, _packet));
        }
    }
    public static void SpawnEnemy(int _toClient, Enemy _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.SpawnEnemy))
        {
            SendTCPData(_toClient,SpawnEnemy_Data(_enemy, _packet));
        }
    }
    public static Packet SpawnEnemy_Data(Enemy _enemy, Packet _packet)
    {
        _packet.Write(_enemy.id);
        _packet.Write(_enemy.transform.position);
        return _packet;    }
    public static void EnemyPosition(Enemy _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.EnemyPosition))
        {
            _packet.Write(_enemy.id);
            _packet.Write(_enemy.transform.position);

            SendUDPDataToAll(_packet);
        }
    }
    public static void EnemyHealth(Enemy _enemy)
    {
        using (Packet _packet = new Packet((int)ServerPackets.EnemyHealth))
        {
            _packet.Write(_enemy.id);
            _packet.Write(_enemy.health);

            SendTCPDataToAll(_packet);
        }
    }
    #endregion
}
