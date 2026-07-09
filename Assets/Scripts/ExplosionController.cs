using System;
using UnityEngine;
using UnityEngine.Events;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private GameObject kamikazeExplosion;
    [SerializeField] private AudioSource explosionSound;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        KamikazeEnemy.OnExplosion.AddListener(TriggerExplosion);
        explosionSound =  GetComponent<AudioSource>();
    }

    private void TriggerExplosion(Type type, Vector3 position)
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
