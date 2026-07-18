using UnityEngine;
using UnityEngine.Splines;

public class BossEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Spawn Sound")]
    [SerializeField] private AudioClip fartSound;
    [SerializeField] [Range(0f, 1f)] private float fartVolume = 1f;

    public void SpawnRandomEnemy()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("Keine Gegner-Prefabs beim BossEnemySpawner eingetragen!");
            return;
        }

        if (enemySpawner == null)
        {
            Debug.LogWarning("EnemySpawner fehlt beim BossEnemySpawner!");
            return;
        }

        GameObject selectedPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
        Enemy enemy = selectedPrefab.GetComponent<Enemy>();

        if (enemy == null || enemy.Stats == null)
        {
            Debug.LogWarning($"{selectedPrefab.name} besitzt keine gültige Enemy-Komponente oder Stats!");
            return;
        }

        SplineContainer selectedPath;

        if (enemy.Stats.IsKamikaze)
        {
            if (enemySpawner.availableKamikazePaths == null || enemySpawner.availableKamikazePaths.Count == 0)
            {
                Debug.LogWarning("Keine Kamikaze-Pfade im EnemySpawner eingetragen!");
                return;
            }

            selectedPath = enemySpawner.availableKamikazePaths[
                Random.Range(0, enemySpawner.availableKamikazePaths.Count)
            ];
        }
        else
        {
            if (enemySpawner.availableEnemyPaths == null || enemySpawner.availableEnemyPaths.Count == 0)
            {
                Debug.LogWarning("Keine normalen Enemy-Pfade im EnemySpawner eingetragen!");
                return;
            }

            selectedPath = enemySpawner.availableEnemyPaths[
                Random.Range(0, enemySpawner.availableEnemyPaths.Count)
            ];
        }

        enemySpawner.SpawnEnemy(
            selectedPrefab,
            selectedPath,
            enemy.Stats.MovementSpeed
        );

        if (fartSound != null)
        {
            AudioSource.PlayClipAtPoint(
                fartSound,
                transform.position,
                fartVolume * SFXVolumeManager.Volume
            );
        }
    }
}