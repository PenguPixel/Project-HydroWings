using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Splines;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public List<SplineContainer> availablePaths;
    public CameraStats camStats;

    [SerializeField] int MaxEnemys = 10;

    [SerializeField] float SpawnRate = 2f;

    private int _enemiesSpawned;

    private float _spawnTimer;

    private float _currentX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
        {
            Debug.Log("Keine Enemy Prefabs zugewiesen!");
            return;
        }

        if (availablePaths == null || availablePaths.Count == 0)
        {
            Debug.Log("Keine Pfade zugewiesen!");
            return;
        }
        
        _currentX = transform.position.x;
        transform.position = new Vector3(_currentX, transform.position.y, transform.position.z);
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _currentX += camStats.speed * Time.deltaTime;   // Movement on the x-axis to the right along camera movement
        transform.position = new Vector3(_currentX, transform.position.y, transform.position.z);
        
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
        int randomPrefabIndex = Random.Range(0, enemyPrefabs.Count);
        int randomPathIndex = Random.Range(0, availablePaths.Count);
        
        GameObject selectedPrefab = enemyPrefabs[randomPrefabIndex];
        SplineContainer selectedSpline = availablePaths[randomPathIndex];
        
        GameObject newEnemy = Instantiate(selectedPrefab, transform.position, Quaternion.identity);

        if (newEnemy.TryGetComponent<SplineAnimate>(out SplineAnimate splineAnimate))
        {
            splineAnimate.Container = selectedSpline;
            splineAnimate.Play();
        }
        
    }
}
