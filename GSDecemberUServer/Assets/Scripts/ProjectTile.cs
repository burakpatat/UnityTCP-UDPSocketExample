using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectTile : MonoBehaviour
{
    public static Dictionary<int, ProjectTile> projectTiles = new Dictionary<int, ProjectTile>();
    private static int nextProjectTileId = 1;

    public int id;
    public Rigidbody rb;
    public int thrownByPlayer;
    public Vector3 initialForce;
    public float explosionRadius = 1.5f;
    public float explosionDamage = 75f;

    private void Start()
    {
        id = nextProjectTileId;
        nextProjectTileId++;
        projectTiles.Add(id, this);

        ServerSend.SpawnProjecTile(this, thrownByPlayer);

        rb.AddForce(initialForce);
        StartCoroutine(ExplodeAfterTime());
    }
    private void FixedUpdate()
    {
        ServerSend.ProjecTilePosition(this);
    }
    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }
    public void Initialize(Vector3 _initialMovementDirection, float _initialForceStrength, int _thrownByPlayer)
    {
        initialForce = _initialMovementDirection * _initialForceStrength;
        thrownByPlayer = _thrownByPlayer;
    }
    private void Explode()
    {
        ServerSend.ProjectTileExploded(this);

        Collider[] _colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider _collider in _colliders)
        {
            if (_collider.CompareTag("Player"))
            {
                _collider.GetComponent<Player>().TakeDamage(explosionDamage);
            }
            else if (_collider.CompareTag("Enemy"))
            {
                _collider.GetComponent<Enemy>().TakeDamage(explosionDamage);
            }
        }

        projectTiles.Remove(id);
        Destroy(gameObject);
    }
    private IEnumerator ExplodeAfterTime()
    {
        yield return new WaitForSeconds(10f);

        Explode();
    }
}
