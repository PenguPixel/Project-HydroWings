using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Stats")]
    public float MaxHealth;

    public float MaxLifetime;
    public float MovementSpeed;
    public int Bounty;
    public bool IsKamikaze;
    public float KamikazeDamage;
}
