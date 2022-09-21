using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectTileManager : MonoBehaviour
{
    public int id;
    public GameObject explosionPrefab;

    public void Initialize(int _id)
    {
        id = _id;
    }
    public void Explode(Vector3 _position)
    {
        transform.position = _position;
        GameObject _vfx = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        GameManager.ProjectTiles.Remove(id);
        Destroy(gameObject);
        Destroy(_vfx, 1.2f);
    }
}
