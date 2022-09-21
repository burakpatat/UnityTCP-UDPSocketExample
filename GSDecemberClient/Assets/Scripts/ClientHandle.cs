using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Server Message: {msg}");
        Client.instance.myId = _myId;
        //send welcome received packet->
        ClientSend.welcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }
    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();

        Vector3 _pos = _packet.ReadVector3();
        Quaternion _rot = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _pos, _rot);
    }
    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _pos = _packet.ReadVector3();

        PlayerManager _playerManager;
        if(GameManager.players.TryGetValue(_id, out _playerManager))
        {
            _playerManager.transform.position = _pos;
        }
    }
    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rot = _packet.ReadQuaternion();

        PlayerManager _playerManager;
        if (GameManager.players.TryGetValue(_id, out _playerManager))
        {
            _playerManager.transform.rotation = _rot;
        }
    }
    public static void PlayerDisconnnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }
    public static void PlayerHealth(Packet _packet)
    {
        int _id = _packet.ReadInt();

        float _health = _packet.ReadFloat();
        GameManager.players[_id].setHealth(_health);
    }
    public static void PlayerRespawned(Packet _packet)
    {
        int _id = _packet.ReadInt();
        GameManager.players[_id].Respawn();
    }
    public static void CreateItemSpawner(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        Vector3 _spawnerPosition = _packet.ReadVector3();
        bool _hasItem = _packet.ReadBool();

        GameManager.instance.CreateItemSpawner(_spawnerId, _spawnerPosition, _hasItem);
    }
    public static void ItemSpawned(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        GameManager.itemSpawner[_spawnerId].ItemSpawned();
    }
    public static void ItemPickedUp(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        int _byPlayer = _packet.ReadInt();

        GameManager.itemSpawner[_spawnerId].ItemPickedUp();
        GameManager.players[_byPlayer].ItemCount++;
    }
    public static void SpawnProjecTile(Packet _packet)
    {
        int _projectTileId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        int _thrownPlayer = _packet.ReadInt();

        GameManager.instance.SpawnProjectTile(_projectTileId, _position);
        GameManager.players[_thrownPlayer].ItemCount--;
    }
    public static void ProjectTilePosition(Packet _packet)
    {
        int _projectTileId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        ProjectTileManager _projectTileManager;
        if(GameManager.ProjectTiles.TryGetValue(_projectTileId, out _projectTileManager))
        {
            _projectTileManager.transform.position = _position;
        }
    }
    public static void ProjectTileExploded(Packet _packet)
    {
        int _projectTileId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.ProjectTiles[_projectTileId].Explode(_position);
    }
    public static void SpawnEnemy(Packet _packet)
    {
        int _enemyId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.instance.SpawnEnemy(_enemyId, _position);
    }
    public static void EnemyPosition(Packet _packet)
    {
        int _enemyId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        EnemyManager _enemyManager;
        if(GameManager.Enemies.TryGetValue(_enemyId, out _enemyManager))
        {
            _enemyManager.transform.position = _position;
        }
    }
    public static void EnemyHealth(Packet _packet)
    {
        int _enemyId = _packet.ReadInt();
        float _enemyHealth = _packet.ReadFloat();

        GameManager.Enemies[_enemyId].SetHealth(_enemyHealth);
    }
}
