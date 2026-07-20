using UnityEngine;
using UnityEngine.Pool;

public class WeaponPoint : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public ProjectileStats projectileStats;
    [SerializeField] public WeaponPointStats weaponStats;
    [SerializeField] private CharacterStats characterStats;

    [Header("Schusssound")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip shootSound;
    [SerializeField, Range(0f, 1f)] private float shootVolume = 1f;

    private ObjectPool<Projectile> _pool;

    public bool HasActiveProjectiles =>
        _pool != null &&
        _pool.CountActive > 0;

    private float _fireCooldownTimer;
    private WaterResource _waterResource;

    private void Start()
    {
        _waterResource = GetComponentInParent<WaterResource>();

        // Falls im Inspector keine AudioSource eingetragen wurde,
        // wird automatisch auf diesem GameObject danach gesucht.
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }

        if (weaponStats == null)
        {
            Debug.LogError(
                $"WeaponPoint auf {name}: WeaponPointStats wurden nicht zugewiesen."
            );

            return;
        }

        if (projectileStats == null)
        {
            Debug.LogError(
                $"WeaponPoint auf {name}: ProjectileStats wurden nicht zugewiesen."
            );

            return;
        }

        if (!weaponStats.IsEnemyWeapon &&
            characterStats == null)
        {
            Debug.LogWarning(
                $"WeaponPoint auf {name}: " +
                "CharacterStats wurden nicht zugewiesen."
            );
        }

        _pool = new ObjectPool<Projectile>(
            createFunc: CreateProjectile,
            actionOnGet: OnGetProjectile,
            actionOnRelease: OnReleaseProjectile,
            actionOnDestroy: OnDestroyProjectile,
            collectionCheck: true,
            defaultCapacity: 20,
            maxSize: 50
        );
    }

    private void OnEnable()
    {
        if (weaponStats != null &&
            !weaponStats.IsAutoFire)
        {
            WeaponController.OnManualShootPressed += Shoot;
        }
    }

    private void OnDisable()
    {
        if (weaponStats != null &&
            !weaponStats.IsAutoFire)
        {
            WeaponController.OnManualShootPressed -= Shoot;
        }
    }

    private void Update()
    {
        if (_fireCooldownTimer > 0f)
        {
            _fireCooldownTimer -= Time.deltaTime;
        }

        if (weaponStats != null &&
            weaponStats.IsAutoFire)
        {
            AutoShoot();
        }
    }

    private Projectile CreateProjectile()
    {
        GameObject projectileObject = Instantiate(
            projectileStats.projectilePrefab
        );

        Projectile projectile =
            projectileObject.GetComponent<Projectile>();

        if (projectile == null)
        {
            Debug.LogError(
                $"Das Projektil-Prefab {projectileStats.projectilePrefab.name} " +
                "besitzt kein Projectile-Script."
            );

            Destroy(projectileObject);
            return null;
        }

        projectileObject.SetActive(false);
        projectile.SetPool(_pool);

        return projectile;
    }

    private void OnGetProjectile(Projectile projectile)
    {
        if (projectile == null)
        {
            return;
        }

        projectile.transform.position = transform.position;
        projectile.transform.rotation = transform.rotation;

        // Schaden vor dem Aktivieren festlegen.
        if (!weaponStats.IsEnemyWeapon &&
            characterStats != null)
        {
            projectile.SetDamage(
                characterStats.AttackDamage
            );
        }
        else
        {
            projectile.SetDamage(
                projectileStats.Basedamage
            );
        }

        projectile.gameObject.SetActive(true);
    }

    private void OnReleaseProjectile(Projectile projectile)
    {
        if (projectile == null)
        {
            return;
        }

        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyProjectile(Projectile projectile)
    {
        if (projectile == null)
        {
            return;
        }

        Destroy(projectile.gameObject);
    }

    private float GetFireRate()
    {
        if (!weaponStats.IsEnemyWeapon &&
            characterStats != null)
        {
            return characterStats.FireRate;
        }

        return weaponStats.FireRate;
    }

    public void Shoot()
    {
        if (_fireCooldownTimer > 0f)
        {
            return;
        }

        if (_pool == null)
        {
            return;
        }

        if (projectileStats.UsesWater &&
            _waterResource != null)
        {
            bool hasWater =
                _waterResource.TryConsumeWater(
                    projectileStats.WaterCostPerShot
                );

            if (!hasWater)
            {
                Debug.Log("Nicht genug Wasser!");
                return;
            }
        }

        _pool.Get();
        PlayShootSound();

        _fireCooldownTimer = GetFireRate();
    }

    private void AutoShoot()
    {
        if (_fireCooldownTimer > 0f)
        {
            return;
        }

        if (_pool == null)
        {
            return;
        }

        _pool.Get();
        PlayShootSound();

        _fireCooldownTimer = GetFireRate();
    }

    private void PlayShootSound()
    {
        if (audioSource == null ||
            shootSound == null)
        {
            return;
        }

        audioSource.PlayOneShot(
            shootSound,
            shootVolume
        );
    }

}