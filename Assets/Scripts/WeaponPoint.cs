using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;

public class WeaponPoint : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public ProjectileStats projectileStats;

    [SerializeField] public WeaponPointStats weaponStats;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip waterShootSound;
    [SerializeField] [Range(0f, 1f)] private float shootVolume = 1f;

    private ObjectPool<Projectile> _pool;
    public bool HasActiveProjectiles => _pool != null && _pool.CountActive > 0;

    private float _fireCooldownTimer = 0f;
    private WaterResource _waterResource;

    public UnityEvent OnEmpty;
    private bool _isShuttingDown = false;

    private void Start()
    {
        _waterResource = GetComponentInParent<WaterResource>();

        _pool = new ObjectPool<Projectile>(
            createFunc: CreateProjectile,
            actionOnGet: OnGetProjectile,
            actionOnRelease: OnReleaseProjectile,
            actionOnDestroy: OnDestroyProjectile,
            collectionCheck: true,
            defaultCapacity: 20,
            maxSize: 50);
    }

    private void OnEnable()
    {
        // Nur für das manuelle Event anmelden, wenn es KEINE Auto-Waffe ist
        if (weaponStats != null && !weaponStats.IsAutoFire)
        {
            WeaponController.OnManualShootPressed += Shoot;
        }
    }

    private void OnDisable()
    {
        if (weaponStats != null && !weaponStats.IsAutoFire)
        {
            WeaponController.OnManualShootPressed -= Shoot;
        }
    }

    private void Update()
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

        if (_isShuttingDown && !HasActiveProjectiles)
        {
            OnEmpty?.Invoke();
        }
    }

    private void OnDestroyProjectile(Projectile projectile)
    {
        if (projectile != null)
        {
            Destroy(projectile.gameObject);
        }
    }

    private Enemy AcquireTarget()
    {
        var collidersInRange = Physics.OverlapSphere(
            transform.position,
            weaponStats.WeaponRange,
            LayerMask.GetMask("Enemy"));

        Enemy targetCandidate = null;

        foreach (var col in collidersInRange)
        {
            if (col.GetComponent<Enemy>() != null)
            {
                targetCandidate = col.GetComponent<Enemy>();
                break;
            }
        }

        if (targetCandidate == null)
            return targetCandidate;

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
        if (_fireCooldownTimer > 0f)
        {
            _fireCooldownTimer -= Time.deltaTime;
        }
    }

    private void OnDestroy()
    {
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

    private void PlayShootSound()
    {
        if (audioSource != null && waterShootSound != null)
        {
            audioSource.PlayOneShot(
                waterShootSound,
                shootVolume * SFXVolumeManager.Volume
            );
        }
    }

    public void Shoot()
    {
        if (_fireCooldownTimer <= 0)
        {
            if (projectileStats.UsesWater && _waterResource != null)
            {
                if (_waterResource.TryConsumeWater(projectileStats.WaterCostPerShot))
                {
                    Fire();
                    PlayShootSound();
                    _fireCooldownTimer = weaponStats.FireRate;
                }
                else
                {
                    Debug.Log("Keine Munition mehr!");
                }
            }
            else
            {
                Fire();
                PlayShootSound();
                _fireCooldownTimer = weaponStats.FireRate;
            }
        }
    }

    public void AutoShooting(Enemy target)
    {
        if (_fireCooldownTimer <= 0f &&
            target != null &&
            !weaponStats.IsEnemyWeapon &&
            weaponStats.IsAutoFire)
        {
            transform.LookAt(target.transform.position);
            Fire();
            PlayShootSound();
            _fireCooldownTimer = weaponStats.FireRate;
        }

        if (_fireCooldownTimer <= 0f &&
            weaponStats.IsEnemyWeapon)
        {
            Fire();
            PlayShootSound();
            _fireCooldownTimer = weaponStats.FireRate;
        }
    }

    public void ShutdownAndNotify()
    {
        _isShuttingDown = true;

        if (!HasActiveProjectiles)
        {
            OnEmpty?.Invoke();
        }
    }
}