
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.Splines;

public class Enemy : MonoBehaviour, IPoolableEnemy
{
    [Header("Scriptable Object Scripts")]
    [SerializeField] public EnemyStats Stats;

    protected bool _isDead = false;

    private MeshRenderer _localMeshRenderer;
    private Collider _localCollider;
    private WeaponPoint _localWeaponPoint;
    private EnemyHitFlash _hitFlash;

    private float _currentHealth;
    private float _remainingLifetime;
    private int _scaledBounty;
    private float _scaledHealth;
    private float _scaledLifetime;

    private IObjectPool<GameObject> _myPool;

    public static UnityEvent<int> BountyOnDeath =
        new UnityEvent<int>();

    public SplineAnimate splineAnimate;

    public void SetPool(IObjectPool<GameObject> pool)
    {
        _myPool = pool;
    }

    private void Awake()
    {
        _scaledBounty = Mathf.CeilToInt(Stats.Bounty * GameManager.GlobalDifficultiyMultiplier
        );

        _scaledHealth = Stats.MaxHealth * GameManager.GlobalDifficultiyMultiplier;

        _scaledLifetime = Stats.MaxLifetime * GameManager.GlobalDifficultiyMultiplier;

        _localMeshRenderer = GetComponent<MeshRenderer>();

        _localCollider = GetComponent<Collider>();

        _localWeaponPoint = GetComponentInChildren<WeaponPoint>();

        _hitFlash = GetComponent<EnemyHitFlash>();

        _currentHealth = _scaledHealth;
        _remainingLifetime = _scaledLifetime;
    }

    private void Update()
    {
        if (_isDead)
        {
            return;
        }

        LifetimeHandling();
    }

    protected void LifetimeHandling()
    {
        if (_remainingLifetime <= 0)
        {
            if (splineAnimate.Loop == SplineAnimate.LoopMode.Loop)
            {
                if (splineAnimate.NormalizedTime >= 0.9f)
                {
                    TriggerLocalDeath();
                    return;
                }
            }
            else
            {
                TriggerLocalDeath();
                return;
            }
        }

        _remainingLifetime -= Time.deltaTime;
    }

    public void TakeDamage(float incomingDamage)
    {
        if (_isDead)
        {
            return;
        }

        _currentHealth -= incomingDamage;
        _currentHealth = Mathf.Max(_currentHealth, 0f);

        _hitFlash?.Flash();

        Debug.Log(
            $"{gameObject.name} wurde getroffen und hat " +
            $"{incomingDamage} Schaden genommen. " +
            $"Verbleibendes Leben: {_currentHealth}"
        );

        if (_currentHealth <= 0f)
        {
            BountyOnDeath.Invoke(_scaledBounty);
            
            TriggerLocalDeath();
        }
    }

    protected void TriggerLocalDeath()
    {
        if (_isDead)
        {
            return;
        }

        _isDead = true;

        if (_localMeshRenderer)
        {
            _localMeshRenderer.enabled = false;
        }

        if (_localCollider)
        {
            _localCollider.enabled = false;
        }

        if (_localWeaponPoint)
        {
            _localWeaponPoint.enabled = false;
        }

        if (_myPool != null)
        {
            _myPool.Release(gameObject);
        }
        else
        {
            Debug.LogWarning($"{gameObject.name} besitzt keinen zugewiesenen Pool.");

            gameObject.SetActive(false);
        }
    }

    public void OnSpawn(SplineContainer spline)
    {
        if (!splineAnimate)
        {
            splineAnimate = GetComponent<SplineAnimate>();
        }

        if (!_localMeshRenderer)
        {
            _localMeshRenderer = GetComponent<MeshRenderer>();
        }

        if (!_localCollider)
        {
            _localCollider = GetComponent<Collider>();
        }

        if (!_localWeaponPoint)
        {
            _localWeaponPoint = GetComponentInChildren<WeaponPoint>();
        }

        if (!_hitFlash)
        {
            _hitFlash = GetComponent<EnemyHitFlash>();
        }

        _isDead = false;
        _currentHealth = _scaledHealth;
        _remainingLifetime = _scaledLifetime;

        if (_localMeshRenderer)
        {
            _localMeshRenderer.enabled = true;
        }

        if (_localCollider)
        {
            _localCollider.enabled = true;
        }

        if (_localWeaponPoint)
        {
            _localWeaponPoint.enabled = true;
        }

        splineAnimate.Container = spline;

        splineAnimate.Restart(true);
    }

    public void OnDespawn()
    {
        if (splineAnimate != null)
        {
            splineAnimate.Pause();
        }
    }
}