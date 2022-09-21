using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public Text username_text;
    public float health;
    public float maxHealth;
    public MeshRenderer model;

    public int ItemCount = 0;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
    }
    public void setHealth(float _health)
    {
        health = _health;

        if (health <= 0f)
        {
            Die();
        }
    }
    public void Die()
    {
        model.enabled = false;
    }
    public void Respawn()
    {
        model.enabled = true;
        setHealth(maxHealth);
    }
}
