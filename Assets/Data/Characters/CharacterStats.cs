using UnityEngine;

[CreateAssetMenu(
    fileName = "CharacterStats",
    menuName = "Scriptable Objects/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    [Header("Health")]
    public int MaxHealth = 100;

    [Header("Movement")]
    public float MovementSpeed = 30f;

    [Header("Water")]
    public float MaxWaterAmount = 100f;
    public float WaterRefillRate = 12.5f;

    [Header("Attack")]
    public float AttackDamage = 10f;
    public float FireRate = 0.4f;
}