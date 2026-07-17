using UnityEngine;
using UnityEngine.Pool;

public class BossLiquidWeapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BossLiquidProjectile projectilePrefab;
    [SerializeField] private Transform spawnPoint;

    [Header("Audio")]
    [SerializeField] private AudioClip liquidShotSound;
    [SerializeField] [Range(0f, 1f)] private float shotVolume = 1f;

    [Header("Spread")]
    [SerializeField] private int projectilesPerVolley = 5;
    [SerializeField] private float spreadAngle = 30f;

    [Tooltip("Lokale Schussrichtung des Spawnpunkts")]
    [SerializeField] private Vector3 localFireDirection = Vector3.left;

    [Header("Pool")]
    [SerializeField] private int defaultCapacity = 20;
    [SerializeField] private int maxSize = 60;

    private ObjectPool<BossLiquidProjectile> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<BossLiquidProjectile>(
            createFunc: CreateProjectile,
            actionOnGet: OnGetProjectile,
            actionOnRelease: OnReleaseProjectile,
            actionOnDestroy: OnDestroyProjectile,
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    private BossLiquidProjectile CreateProjectile()
    {
        BossLiquidProjectile projectile =
            Instantiate(projectilePrefab);

        projectile.gameObject.SetActive(false);

        return projectile;
    }

    private void OnGetProjectile(
        BossLiquidProjectile projectile)
    {
        projectile.gameObject.SetActive(true);
    }

    private void OnReleaseProjectile(
        BossLiquidProjectile projectile)
    {
        projectile.gameObject.SetActive(false);
    }

    private void OnDestroyProjectile(
        BossLiquidProjectile projectile)
    {
        if (projectile != null)
        {
            Destroy(projectile.gameObject);
        }
    }

    public void FireLiquidSpread()
    {
        if (spawnPoint == null)
        {
            Debug.LogWarning(
                "Beim BossLiquidWeapon fehlt der Spawn Point!"
            );

            return;
        }

        if (projectilePrefab == null)
        {
            Debug.LogWarning(
                "Beim BossLiquidWeapon fehlt das Projectile Prefab!"
            );

            return;
        }

        Vector3 baseDirection = Vector3.left;

        if (projectilesPerVolley <= 1)
        {
            SpawnProjectile(baseDirection);
            return;
        }

        for (int i = 0;
             i < projectilesPerVolley;
             i++)
        {
            float t =
                i /
                (float)(projectilesPerVolley - 1);

            float angle = Mathf.Lerp(
                -spreadAngle * 0.5f,
                spreadAngle * 0.5f,
                t
            );

            Vector3 direction =
                Quaternion.AngleAxis(
                    angle,
                    Vector3.forward
                ) * baseDirection;

            SpawnProjectile(direction);
        }
    }

    private void SpawnProjectile(
        Vector3 direction)
    {
        BossLiquidProjectile projectile = _pool.Get();

        projectile.transform.position = spawnPoint.position;

        projectile.transform.rotation = Quaternion.LookRotation(direction);

        projectile.Initialize(
            direction,
            _pool
        );

        if (liquidShotSound != null)
        {
            AudioSource.PlayClipAtPoint(
                liquidShotSound,
                spawnPoint.position,
                shotVolume
            );
        }
    }

    private void OnDestroy()
    {
        if (_pool != null)
        {
            _pool.Clear();
            _pool.Dispose();
        }
    }
}