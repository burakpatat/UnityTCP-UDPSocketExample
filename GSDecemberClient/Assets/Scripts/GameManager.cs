using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public static Dictionary<int, ItemSpawner> itemSpawner = new Dictionary<int, ItemSpawner>();
    public static Dictionary<int, ProjectTileManager> ProjectTiles = new Dictionary<int, ProjectTileManager>();
    public static Dictionary<int, EnemyManager> Enemies = new Dictionary<int, EnemyManager>();

    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;
    public GameObject itemSpawnerPrefab;
    public GameObject ProjectTilePrefab;
    public GameObject EnemyPrefab;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("instance already exist, destroy obj!");
            Destroy(this);
        }
    }
    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }

        _player.GetComponent<PlayerManager>().Initialize(_id, _username);
        players.Add(_id, _player.GetComponent<PlayerManager>());

        _player.GetComponent<PlayerManager>().username_text.text = _username;
    }
    public void CreateItemSpawner(int _spawnerId, Vector3 _position, bool _hasItem)
    {
        GameObject _item = Instantiate(itemSpawnerPrefab, _position, Quaternion.Euler(45,0,45));
        _item.GetComponent<ItemSpawner>().Initialize(_spawnerId, _hasItem);
        itemSpawner.Add(_spawnerId, _item.GetComponent<ItemSpawner>());
    }
    public void SpawnProjectTile(int _id, Vector3 _position)
    {
        GameObject _projectTile = Instantiate(ProjectTilePrefab, _position, Quaternion.identity);
        _projectTile.GetComponent<ProjectTileManager>().Initialize(_id);
        ProjectTiles.Add(_id, _projectTile.GetComponent<ProjectTileManager>());
    }
    public void SpawnEnemy(int _id, Vector3 _position)
    {
        GameObject _enemy = Instantiate(EnemyPrefab, _position, Quaternion.identity);
        _enemy.GetComponent<EnemyManager>().Initialize(_id);
        Enemies.Add(_id, _enemy.GetComponent<EnemyManager>());
    }
}
