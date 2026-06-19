using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Splines;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> EnemyPrefabs;
    public List<SplineContainer> Paths;

    [SerializeField] int MaxEnemys = 10;

    [SerializeField] float SpawnRate = 2f;

    private int _enemiesSpawned;

    private float _spawnTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (EnemyPrefabs == null)
        {
            Debug.Log("Keine Enemy Prefabs zugewiesen!");
            return;
        }

        if (Paths == null)
        {
            Debug.Log("Keine Pfade zugewiesen!");
            return;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer >= SpawnRate && _enemiesSpawned < MaxEnemys)
        {
            SpawnEnemy();
            _enemiesSpawned++;
            _spawnTimer = 0f;
        }
    }

    private void SpawnEnemy()
    {
        throw new System.NotImplementedException();
    }
}
