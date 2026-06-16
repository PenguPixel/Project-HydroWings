using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Pool;

public class WeaponPoint : MonoBehaviour
{
    [SerializeField] private ProjectileStats stats;

    private ObjectPool<Projectile> _pool;
    
    [Header("LocalStats")]
    public float FireRate = 0.4f;
    private float _fireCooldownTimer = 0f;
    [Header("AutoFireStats")]
    public bool AutoFire = false;
    [SerializeField] private float Range = 0f;
    
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
        
        _pool = new ObjectPool<Projectile>(
            createFunc: CreateProjectile,
            actionOnGet: OnGetProjectile,
            actionOnRelease: OnReleaseProjectile,
            actionOnDestroy: OnDestroyProjectile,
            collectionCheck: true,
            defaultCapacity: 20,
            maxSize: 50);
    }
    
    private Projectile CreateProjectile()
    {
        GameObject projectileGO = Instantiate(stats.projectilePrefab);
        Projectile projectile = projectileGO.GetComponent<Projectile>();
        projectile.SetPool(_pool);
        return projectile;
    }
    
    private void OnGetProjectile(Projectile projectile)
    {
        projectile.transform.position = transform.position;
        projectile.transform.rotation = transform.rotation;
        projectile.gameObject.SetActive(true);
    }
    
    private void OnReleaseProjectile(Projectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyProjectile(Projectile projectile)
    {
        Destroy(projectile.gameObject);
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
        if (_pool != null)
        {
            _pool.Get();
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
