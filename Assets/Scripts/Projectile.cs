using System;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private ProjectileStats projectileStats;
    [SerializeField] private WeaponPointStats weaponStats;

    private ObjectPool<Projectile> _assignedPool;
    private float _localRemainingLifetime;
    private float _currentDamage;
    private bool _hasHit;
    private bool _isEnemy;

    private void OnEnable()
    {
        if (projectileStats != null)
        {
            _localRemainingLifetime =
                projectileStats.RemainingLifetime;

            _isEnemy =
                projectileStats.IsEnemyProjectile;

            // Standardwert, falls kein anderer Schaden gesetzt wird.
            _currentDamage =
                projectileStats.Basedamage;
        }
        else
        {
            Debug.LogWarning(
                $"ProjectileStats fehlen auf {name}."
            );
        }
    }

    private void FixedUpdate()
    {
        LifetimeHandling();

        float distanceThisFrame =
            projectileStats.BaseSpeed *
            Time.deltaTime;

        if (!_isEnemy)
        {
            _hasHit = Physics.Raycast(
                transform.position,
                transform.forward,
                out RaycastHit hit,
                distanceThisFrame + Single.Epsilon,
                LayerMask.GetMask("Enemy"),
                QueryTriggerInteraction.Ignore
            );

            if (_hasHit)
            {
                DealDamageToEnemy(hit);

                if (isActiveAndEnabled)
                {
                    ReleaseToPool();
                }
            }
        }
        else
        {
            _hasHit = Physics.Raycast(
                transform.position,
                Vector3.left,
                out RaycastHit hit,
                distanceThisFrame + Single.Epsilon,
                LayerMask.GetMask("Friendly"),
                QueryTriggerInteraction.Ignore
            );

            if (_hasHit)
            {
                DealDamageToCharacter(hit);

                if (isActiveAndEnabled)
                {
                    ReleaseToPool();
                }
            }
        }

        MovementHandling();
    }

    public void SetDamage(float damage)
    {
        _currentDamage = damage;
    }

    private void DealDamageToCharacter(
        RaycastHit hitInfo)
    {
        Character characterComponent =
            hitInfo.collider.GetComponent<Character>();

        characterComponent?.TakeDamage(
            _currentDamage
        );
    }

    private void DealDamageToEnemy(
        RaycastHit hitInfo)
    {
        if (hitInfo.collider.TryGetComponent<Enemy>(
                out Enemy enemy))
        {
            enemy.TakeDamage(_currentDamage);
            return;
        }

        if (hitInfo.collider.TryGetComponent<BossHitbox>(
                out BossHitbox bossHitbox))
        {
            bossHitbox.DealDamage(
                _currentDamage
            );
        }
    }

    private void MovementHandling()
    {
        if (!weaponStats.IsEnemyWeapon &&
            weaponStats.IsAutoFire)
        {
            float speed =
                projectileStats.BaseSpeed *
                Time.deltaTime;

            Vector3 moveDirection =
                transform.forward * speed;

            transform.Translate(
                moveDirection,
                Space.World
            );
        }

        if (weaponStats.IsEnemyWeapon)
        {
            transform.Translate(
                Vector3.left *
                (projectileStats.BaseSpeed *
                 Time.deltaTime),
                Space.World
            );
        }

        if (!weaponStats.IsEnemyWeapon &&
            !weaponStats.IsAutoFire)
        {
            transform.Translate(
                Vector3.right *
                (projectileStats.BaseSpeed *
                 Time.deltaTime),
                Space.World
            );
        }
    }

    private void LifetimeHandling()
    {
        if (_localRemainingLifetime <= 0f &&
            isActiveAndEnabled)
        {
            ReleaseToPool();
        }

        _localRemainingLifetime -=
            Time.deltaTime;
    }

    public void ReleaseToPool()
    {
        if (_assignedPool != null)
        {
            _assignedPool.Release(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetPool(
        ObjectPool<Projectile> pool)
    {
        _assignedPool = pool;
    }
}