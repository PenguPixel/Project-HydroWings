using System;
using UnityEngine;
using UnityEngine.Events;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private GameObject kamikazeExplosion;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        KamikazeEnemy.OnExplosion?.AddListener(TriggerExplosion);
    }

    private void TriggerExplosion(Vector3 position)
    {
        Instantiate(kamikazeExplosion, position, Quaternion.identity);
    }
}
