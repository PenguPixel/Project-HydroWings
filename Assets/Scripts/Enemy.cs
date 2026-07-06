using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    
    
    // TODO Enemy local stats, resistances and PowerUp-Drop logics
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _localMeshRenderer = GetComponent<MeshRenderer>();
        _localCollider = GetComponent<Collider>();
        _localWeaponPoint = GetComponentInChildren<WeaponPoint>();
        _currentHealth = Stats.MaxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDead)
        {
            if (!_localWeaponPoint || !_localWeaponPoint.HasActiveProjectiles)
            {
                Destroy(gameObject);
            }
        }
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
