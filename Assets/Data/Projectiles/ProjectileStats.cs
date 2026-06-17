using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileStats", menuName = "Scriptable Objects/ProjectileStats")]
public class ProjectileStats : ScriptableObject
{
    [Header("Projectile Stats")]
    public float Basedamage = 10f;
    public float BaseSpeed = 1f;
    public float RemainingLifetime = 2f;
    public float Spread = 0;
    public bool EnemyProjectile = false;
    [Header("Projectile Prefab")] 
    public GameObject projectilePrefab;
}
