using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class Projectile : MonoBehaviour
{
    public static ProjectileStats Stats;
    private List<GameObject> _list;
    private int _count;
    private GameObject _localProjectilePrefab;
    

    [SerializeField] private float attackCooldown;
    private float _localRemainingLifetime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _localRemainingLifetime = Stats.RemainingLifetime;
        _list = Stats.projectilePrefabs;
        
        _localProjectilePrefab = PickProjectilePrefab(_list);
    }

    private GameObject PickProjectilePrefab(List<GameObject> list)
    {
        Random rnd = new Random();
        if(list != null)
        {
            int index = rnd.NextInt(list.Count);
            GameObject prefab = list[index];
        }
        return _localProjectilePrefab;
    }

    // Update is called once per frame
    void Update()
    {
        LifetimeHandling();
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
