using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Pool;
using Random = Unity.Mathematics.Random;

public class Projectile : MonoBehaviour
{
    public  ProjectileStats Stats;
    
    private ObjectPool<Projectile> _assignedPool;
    private float _localRemainingLifetime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        if (Stats != null)
        {
            _localRemainingLifetime = Stats.RemainingLifetime;
        }
        else
        {
            Debug.Log("Projectile Stats not set on {name}");
        }
    }

    

    // Update is called once per frame
    void Update()
    {
        MovementHandling();
        LifetimeHandling();
    }

    private void MovementHandling()
    {
        if (Stats.EnemyProjectile)
        {
            transform.Translate(Vector3.left * (Stats.Speed * Time.deltaTime), Space.World);
        }
        else
        {
            transform.Translate(Vector3.right * (Stats.Speed * Time.deltaTime), Space.World);
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
