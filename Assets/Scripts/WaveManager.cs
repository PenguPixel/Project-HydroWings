using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Splines;

public class WaveManager : MonoBehaviour
{
    public Transform cameraTransform;

    public List<EnemyWave> levelWaves;

    private EnemySpawner _spawner;
    private HashSet<EnemyWave> _triggeredWaves = new();
    private List<ActiveWave> _activeWaves = new();

    private class ActiveWave
    {
        public EnemyWave wave;
        public float timer;
        public int enemiesSpawnedCount;
    }

    void Awake()
    {
        _spawner = GetComponent<EnemySpawner>();
    }
    // Update is called once per frame
    void Update()
    {
        CheckForNewWaves();
        UpdateActiveWaves();
    }

    private void UpdateActiveWaves()
    {
        for (int i = _activeWaves.Count - 1; i >= 0; i--)
        {
            var active = _activeWaves[i];
            active.timer += Time.deltaTime;

            while (active.enemiesSpawnedCount < active.wave.enemies.Count &&
                   active.timer >= active.wave.enemies[active.enemiesSpawnedCount].spawnTimeOffset)
            {
                var data = active.wave.enemies[active.enemiesSpawnedCount];

                if (!data.prefab)
                {
                    Debug.LogWarning($"Ein Prefab in der Welle {active.wave.name} fehlt");
                    active.enemiesSpawnedCount++;
                    continue;
                }

                bool isKamikaze = data.prefab.GetComponent<EnemyStats>().IsKamikaze;

                if (_spawner.availableEnemyPaths != null && _spawner.availableEnemyPaths.Count > 0)
                {
                    if (isKamikaze)
                    {
                        int randomPathIndex = Random.Range(0, _spawner.availableKamikazePaths.Count);
                        SplineContainer selectedSpline = _spawner.availableKamikazePaths[randomPathIndex];
                        _spawner.SpawnEnemy(data.prefab, selectedSpline); 
                    }
                    else
                    {
                        int randomPathIndex = Random.Range(0, _spawner.availableEnemyPaths.Count);
                        SplineContainer selectedSpline = _spawner.availableEnemyPaths[randomPathIndex];
                        _spawner.SpawnEnemy(data.prefab, selectedSpline); 
                    }
                }
                else
                {
                    Debug.Log("WaveManager versucht zu spawnen aber EnemySpawner hat keine availablePaths");
                }
                active.enemiesSpawnedCount++;
            }

            if (active.enemiesSpawnedCount >= active.wave.enemies.Count)
            {
                _activeWaves.RemoveAt(i);
            }
        }
    }

    private void CheckForNewWaves()
    {
        foreach (var wave in levelWaves)
        {
            if (!_triggeredWaves.Contains(wave) && cameraTransform.position.x >= wave.triggerXPosition)
            {
                _triggeredWaves.Add(wave);
                _activeWaves.Add(new ActiveWave
                {
                    wave = wave, timer = 0f, enemiesSpawnedCount = 0
                });
            }
        }
    }
}
