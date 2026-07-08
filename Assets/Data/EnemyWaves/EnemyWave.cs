using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyWave", menuName = "Scriptable Objects/EnemyWave")]
public class EnemyWave : ScriptableObject
{
    public float triggerXPosition;
    public List<EnemySpawnData> enemies;
}


[System.Serializable]
public struct EnemySpawnData
{
    public GameObject prefab;
    public float spawnTimeOffset;
}
