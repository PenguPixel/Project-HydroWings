using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    private WaterResource _waterResource;
    private InputAction _moveAction;
    
    public float CurrentMaxHealth {get; private set;}
    public float CurrentMaxWaterResource { get; private set; }
    public float CurrentAttackDamage { get; private set; }
    
    private float _currentHealth;
    private bool _isSubmerged = false;
    private float _fixedZPosition = 0f;

    private bool _isTouchingBackwall;
    private bool _isTouchingObstacle;

    private void OnEnable()
    {
        UnderwaterController.OnSubmerged.AddListener(SetSubmerged);
        HeartPowerUp.OnHeartCollected.AddListener(RestoreHealth);
        /*UpgradeSceneController.OnHealthUpgraded.AddListener(IncreaseMaxHealth);
        UpgradeSceneController.OnWaterResourceUpgraded.AddListener(IncreaseMaxResource);
        UpgradeSceneController.OnAttackDamageUpgraded.AddListener(IncreaseAttackDamage);
        */
    }

    private void OnDisable()
    {
        UnderwaterController.OnSubmerged.RemoveListener(SetSubmerged);
        HeartPowerUp.OnHeartCollected.RemoveListener(RestoreHealth);
        /*UpgradeSceneController.OnHealthUpgraded.RemoveListener(IncreaseMaxHealth);
        UpgradeSceneController.OnWaterResourceUpgraded.RemoveListener(IncreaseMaxResource);
        UpgradeSceneController.OnAttackDamageUpgraded.RemoveListener(IncreaseAttackDamage);
        */
    }

    private void Awake()
    {
        CharacterController = GetComponentInParent<PlayerController>();
        /*if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            CurrentMaxHealth = Stats.MaxHealth; 
            CurrentMaxWaterResource = Stats.MaxWaterAmount; 
            CurrentAttackDamage = Stats.AttackDamage;
        }
        else
        {
            CurrentMaxHealth = _playerProgressionData.maxHealth;
            CurrentMaxWaterResource = _playerProgressionData.maxResource;
            CurrentAttackDamage = _playerProgressionData.attackDamage;
        }*/
        
        CurrentMaxHealth = PlayerProgressionData.maxHealth;
        CurrentMaxWaterResource = PlayerProgressionData.maxResource;
        CurrentAttackDamage = PlayerProgressionData.attackDamage;
        Debug.Log($"Aktuelle Stats - Health: {CurrentMaxHealth}, Resource: {CurrentMaxWaterResource}, Damage: {CurrentAttackDamage}");
    }

    private void Start()
    {
        if (Stats == null)
        {
            Debug.LogError(
                $"Character auf {gameObject.name}: CharacterStats fehlen."
            );

            return;
        }

        _waterResource = GetComponent<WaterResource>();

        _currentHealth =CurrentMaxHealth;

        OnHealthchange.Invoke(
            _currentHealth,
            Stats.MaxHealth
        );

        _moveAction = InputSystem.actions.FindAction("Move");

        Cursor.visible = true;
    }

    private void Update()
    {
        if (!CharacterController || _moveAction == null)
        {
            return;
        }

        Vector2 movementVector =
            _moveAction.ReadValue<Vector2>();

        CharacterController.SetMovementInput(movementVector);

        CharacterController.Rotate(
            movementVector.y,
            transform
        );

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
        float wouldBeHealth =
            _currentHealth - incomingDamage;

        if (wouldBeHealth < 0)
        {
            wouldBeHealth = 0;
        }

        _currentHealth = wouldBeHealth;

        OnHealthchange?.Invoke(
            _currentHealth,
            Stats.MaxHealth
        );

        Debug.Log(
            $"{name} wurde getroffen und hat " +
            $"{incomingDamage} Schaden genommen. " +
            $"Verbleibendes Leben: {_currentHealth}"
        );

        if (_currentHealth == 0)
        {
            OnPlayerDied.Invoke();

            Debug.Log("Character wurde zerstört!");

            Destroy(gameObject);
        }
    }

    private void RestoreHealth(int healthAmount)
    {
        float wouldBeHealth =
            _currentHealth + healthAmount;

        _currentHealth = Mathf.Min(wouldBeHealth, Stats.MaxHealth);

        Debug.Log(
            $"{name} wurde geheilt. Leben: {_currentHealth}"
        );

        OnHealthchange.Invoke(_currentHealth, Stats.MaxHealth);
    }

    private void SetSubmerged(bool isSubmerged)
    {
        _isSubmerged = isSubmerged;

        Debug.Log(
            $"Character is Submerged: {_isSubmerged}"
        );
    }
    
    private void IncreaseAttackDamage(int cost, float increaseAmount)
    {
        CurrentAttackDamage += increaseAmount;
        OnMaxAttackDamageChanged.Invoke(CurrentAttackDamage);
    }

    private void IncreaseMaxResource(int cost, float increaseAmount)
    {
        CurrentMaxWaterResource += increaseAmount;
        OnMaxResourceChanged.Invoke(CurrentMaxWaterResource);
    }

    private void IncreaseMaxHealth(int cost, float increaseAmount)
    {
        CurrentMaxHealth += increaseAmount;
        OnMaxHealthChanged.Invoke(CurrentMaxHealth);
    }
}