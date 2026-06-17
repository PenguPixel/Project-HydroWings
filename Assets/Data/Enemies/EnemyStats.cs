using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Stats")]
    public float MaxHealth;
    public float Speed;
    public int Bounty;      // Check if needed
    public GameObject Projectile = null;
}
