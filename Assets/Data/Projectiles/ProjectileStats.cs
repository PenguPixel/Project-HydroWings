using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileStats", menuName = "Scriptable Objects/ProjectileStats")]
public class ProjectileStats : ScriptableObject
{
    [Header("Projectile Stats")]
    public int damage = 100;
    public float FireRate = 1;
    public float Range = 5;
    public float Speed = 1;
    public float Spread = 0;
    public GameObject ProjectilePrefab;
}
