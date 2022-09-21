using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager instance;

    public GameObject playerPrefab;
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

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 30;

        Server.Start(100, 26950);
    }

    private void OnApplicationQuit()
    {
        Server.Stop();
    }

    public Player InstantiatePlayer()
    {
        return Instantiate(playerPrefab, new Vector3(0,.5f,0), Quaternion.identity).GetComponent<Player>();
    }
    public ProjectTile InstantiateProjectTile(Transform _shootOrigin)
    {
        return Instantiate(ProjectTilePrefab, _shootOrigin.position + _shootOrigin.forward * .7f, Quaternion.identity).GetComponent<ProjectTile>();
    }
    public void InstantiateEnemy(Vector3 _positon)
    {
        Instantiate(EnemyPrefab, _positon, Quaternion.identity);
    }
}
