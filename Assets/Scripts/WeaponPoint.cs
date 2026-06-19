using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;

public class WeaponPoint : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private ProjectileStats projectileStats;

    [SerializeField] public WeaponPointStats weaponStats;

    private ObjectPool<Projectile> _pool;
    public bool HasActiveProjectiles => _pool != null && _pool.CountActive > 0;
        
    
    [Header("LocalStats")]
    private float _fireCooldownTimer = 0f;
    
    
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
        GameObject projectileGO = Instantiate(projectileStats.projectilePrefab);
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

    private void FixedUpdate()
    {
        CooldownHandling();
        if (weaponStats.IsAutoFire)
        {
            
            var target = AcquireTarget();
            if (target)
            {
                AutoShooting(target);
            }
        }
    }

    private Enemy AcquireTarget()
    {
        var collidersInRange = Physics.OverlapSphere(transform.position, weaponStats.WeaponRange, LayerMask.GetMask("Enemy"));
        Enemy targetCandidate = null;
        foreach (var col in collidersInRange)
        {
            if (col.GetComponent<Enemy>() != null)
            {
                targetCandidate = col.GetComponent<Enemy>();
                break;
            }
        }

        if (targetCandidate == null) return targetCandidate;

        foreach (var col in collidersInRange)
        {
            Enemy componentOfTarget = col.GetComponent<Enemy>();
            if (componentOfTarget != null)
            {
                targetCandidate = componentOfTarget;
            }
        }
        return targetCandidate;
    }

    private void CooldownHandling()
    {
        if (_fireCooldownTimer <= 0f)
        {
            _fireCooldownTimer = weaponStats.FireRate;
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
        _pool?.Clear();
        _pool?.Dispose();
    }

    private void Fire()
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
    
    
    public void AutoShooting(Enemy target)
    {
        if (_fireCooldownTimer <= 0f && target != null && !weaponStats.IsEnemyWeapon && weaponStats.IsAutoFire)
        {
            transform.LookAt(target.transform.position);
            Fire();
        }

        if (_fireCooldownTimer <= 0f && weaponStats.IsEnemyWeapon)
        {
            Fire();
        }
    }
}
