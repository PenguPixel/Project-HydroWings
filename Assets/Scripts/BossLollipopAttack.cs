using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class BossLollipopAttack : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private BossLollipopProjectile lollipopPrefab;
    [SerializeField] private Transform spawnAreaCenter;

    [Header("Audio")]
    [SerializeField] private AudioClip lollipopSpawnSound;
    [SerializeField] [Range(0f, 1f)] private float spawnVolume = 1f;

    [Header("Spawn Settings")]
    [SerializeField] private int lollipopCount = 6;
    [SerializeField] private float spawnWidth = 30f;
    [SerializeField] private float timeBetweenSpawns = 0.2f;

    [Header("Pool")]
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxSize = 20;

    private ObjectPool<BossLollipopProjectile> _pool;
    private bool _isSpawning;

    private void Awake()
    {
        _pool = new ObjectPool<BossLollipopProjectile>(
            createFunc: CreateLollipop,
            actionOnGet: OnGetLollipop,
            actionOnRelease: OnReleaseLollipop,
            actionOnDestroy: OnDestroyLollipop,
            collectionCheck: true,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    private BossLollipopProjectile CreateLollipop()
    {
        BossLollipopProjectile lollipop = Instantiate(lollipopPrefab);
        lollipop.gameObject.SetActive(false);
        return lollipop;
    }

    private void OnGetLollipop(BossLollipopProjectile lollipop)
    {
        lollipop.gameObject.SetActive(true);
    }

    private void OnReleaseLollipop(BossLollipopProjectile lollipop)
    {
        lollipop.gameObject.SetActive(false);
    }

    private void OnDestroyLollipop(BossLollipopProjectile lollipop)
    {
        if (lollipop != null)
        {
            Destroy(lollipop.gameObject);
        }
    }

    public void StartLollipopRain()
    {
        if (_isSpawning) return;

        if (lollipopSpawnSound != null)
        {
            AudioSource.PlayClipAtPoint(
                lollipopSpawnSound,
                spawnAreaCenter.position,
                spawnVolume
            );
        }

        StartCoroutine(SpawnLollipops());
    }

    private IEnumerator SpawnLollipops()
    {
        _isSpawning = true;

        for (int i = 0; i < lollipopCount; i++)
        {
            SpawnLollipop();

            if (i < lollipopCount - 1)
            {
                yield return new WaitForSeconds(timeBetweenSpawns);
            }
        }

        _isSpawning = false;
    }

    private void SpawnLollipop()
    {
        if (!spawnAreaCenter || !lollipopPrefab) return;

        float randomX = Random.Range(-spawnWidth * 0.5f, spawnWidth * 0.5f);

        Vector3 spawnPosition = spawnAreaCenter.position + new Vector3(randomX, 0f, 0f);

        BossLollipopProjectile lollipop = _pool.Get();
        lollipop.transform.position = spawnPosition;
        lollipop.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        lollipop.Initialize(_pool);
    }

    private void OnDestroy()
    {
        _pool?.Clear();
        _pool?.Dispose();
    }
}