using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class Projectile : MonoBehaviour
{
    public  ProjectileStats Stats;
    
    private float _localRemainingLifetime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
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
        transform.Translate(Vector3.right * (Stats.Speed * Time.deltaTime), Space.World);
    }

    private void LifetimeHandling()
    {
        if (_localRemainingLifetime <= 0)
        {
            Destroy(gameObject);
        }
        _localRemainingLifetime -= Time.deltaTime;
    }
}
