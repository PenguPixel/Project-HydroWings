using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Splines;
using Object = UnityEngine.Object;

public class Enemy : MonoBehaviour
{
    [Header("Scriptable Object Scripts")]
    [SerializeField] public EnemyStats  Stats;

    private bool _isDead = false;
    private MeshRenderer _localMeshRenderer;
    private Collider _localCollider;
    private WeaponPoint _localWeaponPoint;
    
    private float _currentHealth;
    private float _remainingLifetime;

    public static UnityEvent<int> BountyOnDeath;
    
    // TODO Enemy local stats, resistances and PowerUp-Drop logics
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _localMeshRenderer = GetComponent<MeshRenderer>();
        _localCollider = GetComponent<Collider>();
        _localWeaponPoint = GetComponentInChildren<WeaponPoint>();
        _currentHealth = Stats.MaxHealth;
        _remainingLifetime = Stats.MaxLifetime;
    }

    // Update is called once per frame
    void Update()
    {
        LifetimeHandling();
        if (_isDead)
        {
            if (_localWeaponPoint == null || !_localWeaponPoint.HasActiveProjectiles)
            {
                Destroy(gameObject);
            }
        }
    }

    private void LifetimeHandling()
    {
        if (_remainingLifetime <= 0)
        {
            TriggerLocalDeath();
            return;
        }
        _remainingLifetime -= Time.deltaTime;
    }

    public void DealDamage(float incomingDamage)
    {
        float wouldBeHealth = _currentHealth - incomingDamage;
        if (wouldBeHealth < 0)
        {
            wouldBeHealth = 0;
        }
        
        Debug.Log($"{this.name} wurde getroffen und hat {incomingDamage} Schaden genommen. Verbleibendes Leben: {_currentHealth}");

        if (wouldBeHealth == 0)
        {
            TriggerLocalDeath();
            BountyOnDeath?.Invoke(Stats.Bounty);
            return;
        }

        _currentHealth = wouldBeHealth;
    }

    private void TriggerLocalDeath()
    {
        _isDead = true;
        if (_localMeshRenderer != null) _localMeshRenderer.enabled = false;
        if (_localCollider != null) _localCollider.enabled = false;
        if (_localWeaponPoint != null) _localWeaponPoint.enabled = false;
        
    }
}
