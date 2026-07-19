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
    private bool _isEnemy;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        if (projectileStats != null)
        {
            _localRemainingLifetime = projectileStats.RemainingLifetime;
            _isEnemy = projectileStats.IsEnemyProjectile;
        }
        else
        {
            Debug.Log("Projectile Stats not set on {name}");
        }
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        
        LifetimeHandling();
        
        float distanceThisFrame = projectileStats.BaseSpeed * Time.deltaTime;
        
        if (!_isEnemy)
        {
            _hasHit = Physics.Raycast(transform.position, transform.forward, out RaycastHit hit,
                distanceThisFrame + Single.Epsilon, LayerMask.GetMask("Enemy"), QueryTriggerInteraction.Ignore);
            if (_hasHit)
            {
                //Debug.Log($"{this.name} hat ein Ziel getroffen: {hit.transform.name}");
                DealDamageToEnemy(hit);
                if (isActiveAndEnabled)
                {
                    ReleaseToPool();
                }
            }
        }
        else
        {
            _hasHit = Physics.Raycast(transform.position, Vector3.left, out RaycastHit hit,
                distanceThisFrame + Single.Epsilon, LayerMask.GetMask("Friendly"), QueryTriggerInteraction.Ignore);
            if (_hasHit)
            {
                //Debug.Log($"{this.name} hat ein Ziel getroffen: {hit.transform.name}");
                DealDamageToCharacter(hit);
                if (isActiveAndEnabled)
                {
                    ReleaseToPool();
                }
            }
        }
        
        MovementHandling();
    }

    private void DealDamageToCharacter(RaycastHit hitInfo)
    {
        var characterComponent = hitInfo.collider.GetComponent<Character>();
        characterComponent?.TakeDamage(projectileStats.Basedamage);
    }

    private void DealDamageToEnemy(RaycastHit hitInfo)
    {
        if (hitInfo.collider.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.TakeDamage(projectileStats.Basedamage);
            return;
        }

        if (hitInfo.collider.TryGetComponent<BossHitbox>(out BossHitbox bossHitbox))
        {
            bossHitbox.DealDamage(projectileStats.Basedamage);
        }
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
        if (_localRemainingLifetime <= 0f && isActiveAndEnabled)
        {
            ReleaseToPool();
        }
        _localRemainingLifetime -= Time.deltaTime;
    }

    public void ReleaseToPool()
    {
        if (_assignedPool != null)
        {
            //Debug.Log($"{this.name} kehrt zum Pool zurück");
            _assignedPool.Release(this);
        }
        else
        {
            //Debug.Log($"{this.name} hat keinen Pool mehr");
            Destroy(gameObject);
        }
    }

    public void SetPool(ObjectPool<Projectile> pool)
    {
        _assignedPool = pool;
    }
}
