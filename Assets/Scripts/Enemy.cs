
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

    public static UnityEvent<int> BountyOnDeath;

    public SplineAnimate splineAnimate;

    private IObjectPool<GameObject> _myPool;
    
    public void SetPool(IObjectPool<GameObject> pool) => _myPool = pool;
    
    // TODO Enemy local stats, resistances and PowerUp-Drop logics
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _localMeshRenderer = GetComponent<MeshRenderer>();
        _localCollider = GetComponent<Collider>();
        _localWeaponPoint = GetComponentInChildren<WeaponPoint>();
        _currentHealth = Stats.MaxHealth;
        _remainingLifetime = Stats.MaxLifetime;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDead) return;
        LifetimeHandling();
    }

    protected void LifetimeHandling()
    {
        if (_remainingLifetime <= 0)
        {
            TriggerLocalDeath();
            return;
        }
        _remainingLifetime -= Time.deltaTime;
    }

    public void DealDamage(float incomingDamage)
    {
        if (_isDead) return;
        
        float wouldBeHealth = _currentHealth - incomingDamage;
        if (wouldBeHealth < 0)
        {
            wouldBeHealth = 0;
        }
        
        Debug.Log($"{this.name} wurde getroffen und hat {incomingDamage} Schaden genommen. Verbleibendes Leben: {_currentHealth}");

        if (wouldBeHealth == 0)
        {
            TriggerLocalDeath();
            BountyOnDeath?.Invoke(Stats.Bounty);
            return;
        }

        _currentHealth = wouldBeHealth;
    }

    private void TriggerLocalDeath()
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
