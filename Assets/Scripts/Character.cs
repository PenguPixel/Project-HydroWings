using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    public CharacterStats Stats;
    public PlayerController CharacterController;

    public static UnityEvent<float, float> OnHealthchange;

    private WaterResource _waterResource;
    private InputAction _moveAction;
    private float _cameraMoveSpeed;
    private float _currentHealth;
    private bool _isSubmerged = false;
    private float _fixedZPosition = 0f;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
   void Awake()
   {
       CameraController.MoveAction.AddListener(SetCameraMoveSpeed);
   }
   
   void Start()
    {
        _waterResource = GetComponent<WaterResource>(); 
        _currentHealth = Stats.MaxHealth;
        _moveAction = InputSystem.actions.FindAction("Move");

        Cursor.visible = true;
        UnderwaterController.OnSubmerged.AddListener(SetSubmerged);
    }

    private void SetCameraMoveSpeed(float camSpeed)
    {
        _cameraMoveSpeed = camSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementVector = _moveAction.ReadValue<Vector2>();
        CharacterController.Move(movementVector);
        CharacterController.Rotate(movementVector.y);
        
        movementVector += new Vector2((_cameraMoveSpeed / CharacterController.MovementSpeed) , 0);
        CharacterController.Move(movementVector);
        
        // Character always stays at Z = 0 
        Vector3 currentPos = transform.position;
        currentPos.z = _fixedZPosition;
        transform.position = currentPos;
        
        // Underwater State and Refill 
        if (_isSubmerged && _waterResource != null)
        {
            _waterResource.RefillOverTime();
        }
    }
    
    public void DealDamage(float incomingDamage)    // Incoming damage handling if PC gets hit
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
            Destroy(gameObject);
            return;
        }

        _currentHealth = wouldBeHealth;
    }
    
    private void SetSubmerged(bool isSubmerged)
    {
        _isSubmerged = isSubmerged;
        Debug.Log($"Character is Submerged: {_isSubmerged}");
    }
}