using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public static Dictionary<int, ItemSpawner> spawners = new Dictionary<int, ItemSpawner>();
    private static int nextSpawnId = 1;

    public int spawnerId;
    public bool hasItem = false;

    private void Start()
    {
        hasItem = false;
        spawnerId = nextSpawnId;
        nextSpawnId++;
        spawners.Add(spawnerId, this);

        StartCoroutine(SpawnerItem());
    }
    private void OnTriggerEnter(Collider other)
    {
        if (hasItem && other.CompareTag("Player"))
        {
            Player _player = other.GetComponent<Player>();
            if (_player.AttemptPickupItem())
            {
                ItemsPickedUp(_player.id);
            }
        }
    }
    private IEnumerator SpawnerItem()
    {
        yield return new WaitForSeconds(10f);
        hasItem = true;
        ServerSend.ItemSpawned(spawnerId);
    }
    private void ItemsPickedUp(int _byPlayer)
    {
        hasItem = false;
        ServerSend.ItemPickedUp(spawnerId, _byPlayer);

        StartCoroutine(SpawnerItem());
    }
}
