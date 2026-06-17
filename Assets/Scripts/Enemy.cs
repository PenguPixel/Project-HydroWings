using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public EnemyStats  Stats;
    
    [Header("Local Stats")]
    [SerializeField] private bool HasResist = false;
    public List<DamageType> Resistances;
    [SerializeField] private bool HasPowerup = false;
    public List<PowerUpType> PowerUps;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
