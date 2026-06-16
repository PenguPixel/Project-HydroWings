using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileStats", menuName = "Scriptable Objects/ProjectileStats")]
public class ProjectileStats : ScriptableObject
{
    [Header("Projectile Stats")]
    public float damage = 100f;
    public float Speed = 1f;
    public float RemainingLifetime = 2f;
    public float Spread = 0;
    [Header("Projectile Prefab")] 
    public GameObject projectilePrefab;

    /*----------------------------  OLD VERSION-----------------
     public GameObject GetRandomProjectilePrefab()
    {
        if (projectilePrefabs == null || projectilePrefabs.Count == 0)
        {
            Debug.Log($"ProjectilePrafabs List of {name} is empty or null");
            return null;
        }
        int randomIndex = Random.Range(0, projectilePrefabs.Count);
        return projectilePrefabs[randomIndex];
    }*/
}
