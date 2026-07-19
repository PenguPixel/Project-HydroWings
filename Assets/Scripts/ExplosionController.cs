using System;
using UnityEngine;
using UnityEngine.Events;

public class ExplosionController : MonoBehaviour
{
    [SerializeField] private GameObject kamikazeExplosion;

    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private float explosionVolume = 1f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        KamikazeEnemy.OnExplosion?.AddListener(TriggerExplosion);
    }

    private void TriggerExplosion(Vector3 position)
    {
        Instantiate(kamikazeExplosion, position, Quaternion.identity);

        if (explosionSound != null)
        {
            AudioSource.PlayClipAtPoint(
                explosionSound, 
                position, 
                explosionVolume * SFXVolumeManager.Volume
                );
        }
    }
}