using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Pool;
using Random = Unity.Mathematics.Random;

public class Projectile : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private  ProjectileStats projectileStats;

    [SerializeField] private WeaponPointStats weaponStats;
    
    private ObjectPool<Projectile> _assignedPool;
    private float _localRemainingLifetime;
    private bool _hasHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        if (projectileStats != null)
        {
            _localRemainingLifetime = projectileStats.RemainingLifetime;
        }
        else
        {
            Debug.Log("Projectile Stats not set on {name}");
        }
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        MovementHandling();
        LifetimeHandling();
        float distanceThisFrame = projectileStats.BaseSpeed * Time.deltaTime;
        _hasHit = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit,
            distanceThisFrame + Single.Epsilon, LayerMask.GetMask("Enemy"));
        if (_hasHit)
        {
            DealDamage(hit);
        }
        
    }

    private void DealDamage(RaycastHit hitInfo)
    {
        var enemyComponent = hitInfo.collider.GetComponent<Enemy>();
        enemyComponent?.DealDamage(projectileStats.Basedamage);
    }

    private void MovementHandling()
    {
        if(!weaponStats.IsEnemyWeapon && weaponStats.IsAutoFire)
        {
            float speed = projectileStats.BaseSpeed * Time.deltaTime;
            Vector3 moveDirection = transform.forward * speed;
            transform.Translate(moveDirection, Space.World);
        } 
        
        if (weaponStats.IsEnemyWeapon)
        {
            transform.Translate(Vector3.left * (projectileStats.BaseSpeed * Time.deltaTime), Space.World);
        }

        if (!weaponStats.IsEnemyWeapon && !weaponStats.IsAutoFire)
        {
            transform.Translate(Vector3.right * (projectileStats.BaseSpeed * Time.deltaTime), Space.World);
        }
    }

    private void LifetimeHandling()
    {
        if (_localRemainingLifetime <= 0f)
        {
            ReleaseToPool();
        }
        _localRemainingLifetime -= Time.deltaTime;
    }

    public void ReleaseToPool()
    {
        if (_assignedPool != null)
        {
            _assignedPool.Release(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPool(ObjectPool<Projectile> pool)
    {
        _assignedPool = pool;
    }
}
