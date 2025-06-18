using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance {  get; private set; }

    [Header("Core")]
    [SerializeField] private List<Transform> spawnPoints;
    [SerializeField] private Transform enemyPrefab;

    

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void SpawnEnemy()
    {
        StartCoroutine(Spawn());
    }
    private IEnumerator Spawn()
    {
        yield return new WaitForSeconds(3);
        var enemy = Instantiate(enemyPrefab, spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count)]);
    }

}
