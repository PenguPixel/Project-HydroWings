using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Scriptable Object Scripts")]
    [SerializeField] public EnemyStats  Stats;
    [SerializeField] public CameraStats  CameraStats;
    
    /*
    // TODO Enemy local stats, resistances and PowerUp-Drop logics
    [Header("Local Stats")]
    [SerializeField] private bool HasResist = false;
    public List<DamageType> Resistances;
    [SerializeField] private bool HasPowerup = false;
    public List<PowerUpType> PowerUps;
    */
    
    // TODO Layer handling
    // TODO Move handling
    // TODO damage dealt handling
    // TODO damage received handling
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
