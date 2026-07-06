using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyStats", menuName = "Scriptable Objects/EnemyStats")]
public class EnemyStats : ScriptableObject
{
    [Header("Stats")]
    public float MaxHealth;

    public float MaxLifetime;
    public float MovementSpeed;
    public int Bounty; 
}
