using System;
using UnityEngine;
using static Player;

public class Enemy : MonoBehaviour,IDamageable
{
    [Header("Core")]
    [SerializeField] private int health = 100;

    
    private BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }
    

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            // destroy 
            boxCollider.enabled = false;
            EnemySpawner.Instance.SpawnEnemy();
        }
    }
    
}
