using UnityEngine;
using UnityEngine.Pool;

public class BossLiquidProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float hitRadius = 0.15f;

    [Header("Collision")]
    [SerializeField] private LayerMask playerLayer;

    private ObjectPool<BossLiquidProjectile> _pool;
    private Vector3 _direction;
    private float _remainingLifetime;
    private bool _wasReleased;

    private void OnEnable()
    {
        _remainingLifetime = lifetime;
        _wasReleased = false;
    }

    private void FixedUpdate()
    {
        float distance = speed * Time.fixedDeltaTime;

        if (Physics.SphereCast(
                transform.position,
                hitRadius,
                _direction,
                out RaycastHit hit,
                distance,
                playerLayer,
                QueryTriggerInteraction.Ignore))
        {
            Character character = FindCharacter(hit.collider);

            if (character != null)
            {
                character.DealDamage(damage);
            }

            Release();
            return;
        }

        transform.position += _direction * distance;

        _remainingLifetime -= Time.fixedDeltaTime;

        if (_remainingLifetime <= 0f)
        {
            Release();
        }
    }

    public void Initialize(
        Vector3 direction,
        ObjectPool<BossLiquidProjectile> pool)
    {
        _direction = direction.normalized;
        _pool = pool;
    }

    private Character FindCharacter(Collider hitCollider)
    {
        Character character = hitCollider.GetComponent<Character>();

        if (character == null)
        {
            character = hitCollider.GetComponentInParent<Character>();
        }

        if (character == null)
        {
            character = hitCollider.GetComponentInChildren<Character>();
        }

        return character;
    }

    private void Release()
    {
        if (_wasReleased)
        {
            return;
        }

        _wasReleased = true;

        if (_pool != null)
        {
            _pool.Release(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}