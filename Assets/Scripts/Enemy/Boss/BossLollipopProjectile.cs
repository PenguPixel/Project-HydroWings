using UnityEngine;
using UnityEngine.Pool;

public class BossLollipopProjectile : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 10f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 6f;
    [SerializeField] private float hitRadius = 0.25f;
    [SerializeField] private LayerMask playerLayer;

    private ObjectPool<BossLollipopProjectile> _pool;
    private float _remainingLifetime;
    private bool _wasReleased;

    private void OnEnable()
    {
        _remainingLifetime = lifetime;
        _wasReleased = false;
    }

    private void FixedUpdate()
    {
        float distance = fallSpeed * Time.fixedDeltaTime;

        if (Physics.SphereCast(transform.position, hitRadius, Vector3.down, out RaycastHit hit, distance, playerLayer, QueryTriggerInteraction.Ignore))
        {
            Debug.Log(hit.collider.gameObject.name);
            
            Character character = hit.collider.GetComponent<Character>();

            if (character == null)
            {
                character = hit.collider.GetComponentInParent<Character>();
            }

            if (character == null)
            {
                character = hit.collider.GetComponentInChildren<Character>();
            }

            if (character != null)
            {
                character.TakeDamage(damage);
            }

            Release();
            return;
        }

        transform.position += Vector3.down * distance;

        _remainingLifetime -= Time.fixedDeltaTime;

        if (_remainingLifetime <= 0f)
        {
            Release();
        }
    }

    public void Initialize(ObjectPool<BossLollipopProjectile> pool)
    {
        _pool = pool;
    }

    private void Release()
    {
        if (_wasReleased) return;

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