
using Interfaces;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Pool;
using UnityEngine.Splines;

public class Enemy : MonoBehaviour, IPoolableEnemy
{
    [Header("Scriptable Object Scripts")]
    [SerializeField] public EnemyStats  Stats;

    protected bool _isDead = false;
    private MeshRenderer _localMeshRenderer;
    private Collider _localCollider;
    private WeaponPoint _localWeaponPoint;
    
    private float _currentHealth;
    private float _remainingLifetime;

    public static UnityEvent<int> BountyOnDeath = new UnityEvent<int>();

    public SplineAnimate splineAnimate;

    private IObjectPool<GameObject> _myPool;
    
    public void SetPool(IObjectPool<GameObject> pool) => _myPool = pool;
    
    
    void Awake()
    {
        _localMeshRenderer = GetComponent<MeshRenderer>();
        _localCollider = GetComponent<Collider>();
        _localWeaponPoint = GetComponentInChildren<WeaponPoint>();
        _currentHealth = Stats.MaxHealth;
        _remainingLifetime = Stats.MaxLifetime;
    }
    
    void Update()
    {
        if (_isDead) return;
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
        if (_isDead) return;
        
        float wouldBeHealth = _currentHealth - incomingDamage;
        if (wouldBeHealth < 0)
        {
            wouldBeHealth = 0;
        }
        
        if (wouldBeHealth == 0)
        {
            BountyOnDeath.Invoke(Stats.Bounty);
            TriggerLocalDeath();
            Debug.Log($"{gameObject.name} has been defeated. Add Bounty {Stats.Bounty}");
            return;
        }

        _currentHealth = wouldBeHealth;
    }

    protected void TriggerLocalDeath()
    {
        _isDead = true;
        if (_localMeshRenderer != null) _localMeshRenderer.enabled = false;
        if (_localCollider != null) _localCollider.enabled = false;
        if (_localWeaponPoint != null) _localWeaponPoint.enabled = false;
        _myPool.Release(gameObject);
    }

    public void OnSpawn(SplineContainer spline)
    {
        if (!splineAnimate) splineAnimate = GetComponent<SplineAnimate>();
        if (!_localMeshRenderer) _localMeshRenderer = GetComponent<MeshRenderer>();
        if (!_localCollider) _localCollider = GetComponent<Collider>();
        
        _isDead = false;
        _currentHealth = Stats.MaxHealth;
        _remainingLifetime = Stats.MaxLifetime;
        
        if (_localMeshRenderer != null) _localMeshRenderer.enabled = true;
        if (_localCollider != null) _localCollider.enabled = true;
        if (_localWeaponPoint != null) _localWeaponPoint.enabled = true;
        
        splineAnimate.Container = spline;
        splineAnimate.Restart(true);
    }

    public void OnDespawn()
    {
        splineAnimate.Pause();
    }
}
