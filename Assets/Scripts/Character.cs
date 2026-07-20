using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    public CharacterStats Stats;
    public PlayerController CharacterController;

    public static UnityEvent<float, float> OnHealthchange =
        new UnityEvent<float, float>();

    public static UnityEvent OnPlayerDied =
        new UnityEvent();

    private WaterResource _waterResource;
    private InputAction _moveAction;
    private float _cameraMoveSpeed;
    private float _currentHealth;
    private bool _isSubmerged = false;
    private float _fixedZPosition = 0f;

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
        CameraController.MoveAction.AddListener(SetCameraMoveSpeed);
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

        _currentHealth = Stats.MaxHealth;

        OnHealthchange.Invoke(
            _currentHealth,
            Stats.MaxHealth
        );

        _moveAction = InputSystem.actions.FindAction("Move");

        Cursor.visible = true;
    }

    private void SetCameraMoveSpeed(float camSpeed)
    {
        _cameraMoveSpeed = camSpeed;
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
}