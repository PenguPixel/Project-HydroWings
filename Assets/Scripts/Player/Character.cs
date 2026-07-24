using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    public CharacterStats Stats;
    public PlayerController CharacterController;
    public PlayerProgressionData PlayerProgressionData;

    public static UnityEvent<float, float> OnHealthchange = new UnityEvent<float, float>();

    public static UnityEvent OnPlayerDied = new UnityEvent();

    public static UnityEvent<float> OnMaxHealthChanged = new UnityEvent<float>();

    public static UnityEvent<float> OnMaxResourceChanged = new UnityEvent<float>();

    public static UnityEvent<float> OnMaxAttackDamageChanged = new UnityEvent<float>();

    public static UnityEvent<float, float, float> OnUpgradeScreenActive = new UnityEvent<float, float, float>();

    [Header("Hit Sound")]
    [SerializeField] private AudioClip hitSound;

    [SerializeField] [Range(0f, 1f)] private float hitVolume = 1f;

    [Header("Death Explosion")]
    [SerializeField] private GameObject deathExplosionPrefab;

    private WaterResource _waterResource;
    private InputAction _moveAction;
    private PlayerHitFlash _hitFlash;

    public float CurrentMaxHealth { get; private set; }
    public float CurrentMaxWaterResource { get; private set; }
    public float CurrentAttackDamage { get; private set; }

    private float _currentHealth;
    private bool _isSubmerged;
    private float _fixedZPosition;
    private bool _isDead;

    private bool _isTouchingBackwall;
    private bool _isTouchingObstacle;

    private void OnEnable()
    {
        UnderwaterController.OnSubmerged.AddListener(SetSubmerged);
        HeartPowerUp.OnHeartCollected.AddListener(RestoreHealth);
    }

    private void OnDisable()
    {
        UnderwaterController.OnSubmerged.RemoveListener(SetSubmerged);
        HeartPowerUp.OnHeartCollected.RemoveListener(RestoreHealth);
    }

    private void Awake()
    {
        CharacterController = GetComponentInParent<PlayerController>();

        _hitFlash = GetComponent<PlayerHitFlash>();

        if (!PlayerProgressionData)
        {
            Debug.LogError($"Character auf {gameObject.name}: " + "PlayerProgressionData wurde nicht zugewiesen.");
            return;
        }

        CurrentMaxHealth = PlayerProgressionData.maxHealth;
        CurrentMaxWaterResource = PlayerProgressionData.maxResource;
        CurrentAttackDamage = PlayerProgressionData.attackDamage;

        Debug.Log($"Aktuelle Stats - Health: {CurrentMaxHealth}, " + $"Resource: {CurrentMaxWaterResource}, " + $"Damage: {CurrentAttackDamage}");
    }

    private void Start()
    {
        if (!Stats)
        {
            Debug.LogError($"Character auf {gameObject.name}: " + "CharacterStats fehlen.");
            return;
        }

        _waterResource = GetComponent<WaterResource>();
        
        _currentHealth = CurrentMaxHealth;
        
        OnHealthchange?.Invoke(_currentHealth, CurrentMaxHealth);
        
        _moveAction = InputSystem.actions.FindAction("Move");
        
        Cursor.visible = true;
    }

    private void Update()
    {
        if (_isDead)
        {
            return;
        }

        if (!CharacterController || _moveAction == null)
        {
            return;
        }

        Vector2 movementVector = _moveAction.ReadValue<Vector2>();

        CharacterController.SetMovementInput(movementVector);

        CharacterController.Rotate(movementVector.y, transform);

        Vector3 localPos = transform.localPosition;

        localPos.z = _fixedZPosition;

        transform.localPosition = localPos;

        if (_isSubmerged && _waterResource)
        {
            _waterResource.RefillOverTime();
        }
    }

    public void TakeDamage(float incomingDamage)
    {
        if (_isDead)
        {
            return;
        }

        _currentHealth = Mathf.Max(_currentHealth - incomingDamage, 0f);

        _hitFlash?.Flash();

        if (hitSound)
        {
            AudioSource.PlayClipAtPoint(hitSound, transform.position, hitVolume * SFXVolumeManager.Volume);
        }

        OnHealthchange?.Invoke(_currentHealth, CurrentMaxHealth);

        Debug.Log($"{name} wurde getroffen und hat " + $"{incomingDamage} Schaden genommen. " + $"Verbleibendes Leben: {_currentHealth}");

        if (_currentHealth <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        if (_isDead)
        {
            return;
        }

        _isDead = true;

        OnPlayerDied?.Invoke();

        if (deathExplosionPrefab)
        {
            Instantiate(deathExplosionPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning($"Character auf {gameObject.name}: " + "Kein Death Explosion Prefab eingetragen.");
        }

        Debug.Log("Character wurde zerstört!");

        Destroy(gameObject);
    }

    private void RestoreHealth(int healthAmount)
    {
        if (_isDead)
        {
            return;
        }

        _currentHealth = Mathf.Min(_currentHealth + healthAmount, CurrentMaxHealth);

        Debug.Log($"{name} wurde geheilt. " + $"Leben: {_currentHealth}"
        );

        OnHealthchange?.Invoke(_currentHealth, CurrentMaxHealth);
    }

    private void SetSubmerged(bool isSubmerged)
    {
        _isSubmerged = isSubmerged;

        Debug.Log($"Character is Submerged: " + $"{_isSubmerged}");
    }

    public void SetMaxHealth(float newMaxHealth)
    {
        CurrentMaxHealth = newMaxHealth;

        _currentHealth = Mathf.Min(_currentHealth, CurrentMaxHealth);

        OnMaxHealthChanged?.Invoke(CurrentMaxHealth);

        OnHealthchange?.Invoke(_currentHealth, CurrentMaxHealth);
    }

    public void SetMaxWaterResource(float newMaxWaterResource)
    {
        CurrentMaxWaterResource = newMaxWaterResource;

        OnMaxResourceChanged?.Invoke(CurrentMaxWaterResource);

        if (_waterResource)
        {
            _waterResource.SetMaxWater(CurrentMaxWaterResource);
        }
    }

    public void SetAttackDamage(float newAttackDamage)
    {
        CurrentAttackDamage = newAttackDamage;

        OnMaxAttackDamageChanged?.Invoke(CurrentAttackDamage);
    }

    private void IncreaseAttackDamage(int cost, float increaseAmount)
    {
        SetAttackDamage(CurrentAttackDamage + increaseAmount);
    }

    private void IncreaseMaxResource(int cost, float increaseAmount)
    {
        SetMaxWaterResource(CurrentMaxWaterResource + increaseAmount);
    }

    private void IncreaseMaxHealth(int cost, float increaseAmount)
    {
        SetMaxHealth(CurrentMaxHealth + increaseAmount);
    }
}