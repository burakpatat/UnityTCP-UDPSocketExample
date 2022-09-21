using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float frequency = 3f;

    [SerializeField]
    protected float debugDrawradius = 1.0f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, debugDrawradius);
    }
    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }
    private IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(frequency);

        if (Enemy.Enemies.Count < Enemy.maxEnemies)
        {
            NetworkManager.instance.InstantiateEnemy(transform.position);
        }
        StartCoroutine(SpawnEnemy());
    }
}
