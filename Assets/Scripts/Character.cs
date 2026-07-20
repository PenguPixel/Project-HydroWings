using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    public CharacterStats Stats;
    public PlayerController CharacterController;

    public static UnityEvent<float, float> OnHealthchange = new UnityEvent<float, float>();
    public static UnityEvent OnPlayerDied = new UnityEvent();

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
    

    void Awake()
   {
       CharacterController = GetComponentInParent<PlayerController>();
       CameraController.MoveAction.AddListener(SetCameraMoveSpeed);
   }
   
   void Start()
    {
        _waterResource = GetComponent<WaterResource>(); 
        _currentHealth = Stats.MaxHealth;
        
        OnHealthchange.Invoke(_currentHealth, Stats.MaxHealth);
        
        _moveAction = InputSystem.actions.FindAction("Move");

        Cursor.visible = true;
    }

    private void SetCameraMoveSpeed(float camSpeed)
    {
        _cameraMoveSpeed = camSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (!CharacterController) return;
        
        Vector2 movementVector = _moveAction.ReadValue<Vector2>();
        CharacterController.SetMovementInput(movementVector);
        CharacterController.Rotate(movementVector.y, this.transform);
        
        // Character always stays at Z = 0 
        Vector3 localPos = transform.localPosition;
        localPos.z = _fixedZPosition;
        transform.localPosition = localPos;
        
        // Underwater State and Refill 
        if (_isSubmerged && _waterResource != null)
        {
            _waterResource.RefillOverTime();
        }
    }

    public void TakeDamage(float incomingDamage)    // Incoming damage handling if PC gets hit
    {
        float wouldBeHealth = _currentHealth - incomingDamage;
        if (wouldBeHealth < 0)
        {
            wouldBeHealth = 0;
        }
        
        OnHealthchange?.Invoke(wouldBeHealth, Stats.MaxHealth);

        Debug.Log(
            $"{this.name} wurde getroffen und hat {incomingDamage} Schaden genommen. Verbleibendes Leben: {_currentHealth}");
        if (wouldBeHealth == 0)
        {
            OnPlayerDied.Invoke();
            Debug.Log("Character wurde zerstört!");
            Destroy(gameObject);
            return;
        }

        _currentHealth = wouldBeHealth;
    }
    
    private void RestoreHealth(int healthAmount)
    {
        float wouldBeHealth = _currentHealth + healthAmount;
        if (wouldBeHealth > Stats.MaxHealth) return;
        _currentHealth = wouldBeHealth;
        Debug.Log($"{this.name} wurde geheilt {_currentHealth}");
        OnHealthchange.Invoke(_currentHealth, Stats.MaxHealth);
    }
    
    private void SetSubmerged(bool isSubmerged)
    {
        _isSubmerged = isSubmerged;
        Debug.Log($"Character is Submerged: {_isSubmerged}");
    }
}