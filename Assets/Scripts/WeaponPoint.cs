using System;
using UnityEngine;

public class WeaponPoint : MonoBehaviour
{
    [SerializeField] private ProjectileStats stats;
    [Header("LocalStats")]
    public float FireRate = 0.4f;
    private float _fireCooldownTimer = 0f;
    [Header("AutoFireStats")]
    public bool AutoFire = false;
    public float Range = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        WeaponController controller = GetComponentInParent<WeaponController>();

        if (controller != null)
        {
            controller.RegisterWeaponPoint(this);
        }
        else
        {
            Debug.Log($"No WeaponController on {gameObject.name} ");
        }
    }

    private void Update()
    {
        CooldownHandling();
        if (AutoFire)
        {
            AutoShooting();
        }
    }

    private void CooldownHandling()
    {
        if (_fireCooldownTimer <= 0f)
        {
            _fireCooldownTimer = FireRate;
        }
        else
        {
            _fireCooldownTimer -= Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
        WeaponController controller = GetComponentInParent<WeaponController>();
        if (controller != null)
        {
            controller.UnregisterWeaponPoint(this);
        }
    }

    public void Fire()
    {
        GameObject prefabToSpawn = stats.GetRandomProjectilePrefab();

        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, transform.position, transform.rotation);
        }
    }

    public void Shoot()
    {
        if (_fireCooldownTimer !<=0)
        {
            Fire();
        }
    }

    //TODO Method GetTarget
    
    public void AutoShooting()
    {
        //TODO Check Range and Target
        if (_fireCooldownTimer <= 0f)
        {
            _fireCooldownTimer = FireRate;
            Fire();
        }
        else
        {
            _fireCooldownTimer -= Time.deltaTime;
        }
    }
}
