using System.Collections.Generic;
using System.IO;
using Interfaces;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.Splines;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemyPrefabs;
    public List<SplineContainer> availableEnemyPaths;
    public List<SplineContainer> availableKamikazePaths;

    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxSize = 50;

    private Dictionary<GameObject, IObjectPool<GameObject>> _pools = new();

    private float _currentX;
    private float _currentCamSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        CameraController.MoveAction.AddListener(SetCamMoveSpeed);
        foreach (var prefab in enemyPrefabs)
        {
            if (!prefab) continue;

            // Set enemy values for level 02 
            if (SceneManager.GetActiveScene().name == "Level_02Scene")
            {
                SetLevelValues(prefab);
            }
            
            _pools[prefab] = new ObjectPool<GameObject>(
                createFunc: () => Instantiate(prefab),
                actionOnGet: (obj) =>
                {
                    obj.SetActive(true);
                },
                actionOnRelease: (obj) =>
                {
                    obj.SetActive(false);
                },
                actionOnDestroy: (obj) =>
                {
                    Destroy(obj);
                },
                defaultCapacity: defaultCapacity,
                maxSize: maxSize
            );
        }
    }

    private static void SetLevelValues(GameObject prefab)
    {
        var enemyPrefab = prefab.GetComponent<Enemy>();
        enemyPrefab.Stats.MaxHealth = 30;
        enemyPrefab.Stats.MaxLifetime = 40;
        enemyPrefab.Stats.MovementSpeed = 5;

        if (enemyPrefab.Stats.IsKamikaze)
        {
            enemyPrefab.Stats.KamikazeDamage = 10;
            enemyPrefab.Stats.Bounty = 80;
        }

        if (!enemyPrefab.Stats.IsKamikaze)
        {
            enemyPrefab.Stats.Bounty = 120;

            var enemyWeapon = enemyPrefab.GetComponentInChildren<WeaponPoint>();
            enemyWeapon.weaponStats.FireRate = 2;
            enemyWeapon.projectileStats.Basedamage = 10;
            enemyWeapon.projectileStats.BaseSpeed = 20;
        }
    }

    void Start()
    {
        
        if (enemyPrefabs == null || enemyPrefabs.Count == 0)
        {
            Debug.Log("Keine Enemy Prefabs zugewiesen!");
            return;
        }

        if (availableEnemyPaths == null || availableEnemyPaths.Count == 0)
        {
            Debug.Log("Keine Pfade zugewiesen!");
            return;
        }
        
        _currentX = transform.position.x;
        transform.position = new Vector3(_currentX, transform.position.y, transform.position.z);
        
    }

    private void SetCamMoveSpeed(float camSpeed)
    {
        _currentCamSpeed = camSpeed;
        // Debug.Log(_currentCamSpeed);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _currentX += _currentCamSpeed * Time.fixedDeltaTime;   // Movement on the x-axis to the right along camera movement
        transform.position = new Vector3(_currentX, transform.position.y, transform.position.z);
    }

    public void SpawnEnemy(GameObject prefab, SplineContainer spline, float  movementSpeed)
    {
        if (!prefab) return;
        
        if (_pools.TryGetValue(prefab, out IObjectPool<GameObject> pool))
        {
            GameObject enemyObj = pool.Get();
            enemyObj.transform.position = transform.position;
            enemyObj.transform.rotation = Quaternion.identity;

            if (enemyObj.TryGetComponent<Enemy>(out var enemy))
            {
                enemy.SetPool(pool);
                enemy.gameObject.GetComponent<SplineAnimate>().MaxSpeed = movementSpeed;
            }

            if (enemyObj.TryGetComponent<IPoolableEnemy>(out var poolable))
            {
                poolable.OnSpawn(spline);
            }
        }
        else
        {
            Debug.LogWarning($"Prefab {prefab.name} hat keinen zugewiesenen Pool!");
        }
    }
}
