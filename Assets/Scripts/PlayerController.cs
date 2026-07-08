using System;
using UnityEngine;
using static UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    //private CharacterController _characterController;
    private Rigidbody _rigidbody;

    [SerializeField]public float MovementSpeed = 30f;
    [SerializeField] private float acceleration = 10f;

    [SerializeField]private float RotationSpeed = 50f;
    [SerializeField]private float ReturnSpeed = 60f;
    [SerializeField]private float MaxRotationAngle = 25f;
    [SerializeField] private float MaxCharacterOffsetY = 10f;
    [SerializeField] private float MinCharacterOffsetY = -10f;
    
    private float _rotationX = 0f;
    private float _camSpeed;
    private Vector2 _currentMovementInput;
    
    private bool _isTouchingObstacle;
    private bool _isTouchingBackwall;
    private Character _character;

    void Awake()
    {
        _character = GetComponentInChildren<Character>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
        _rigidbody.WakeUp();
        CameraController.MoveAction.AddListener(SetCamSpeed);
    }

    private void SetCamSpeed(float cameraSpeedValue)
    {
        _camSpeed = cameraSpeedValue;
        Debug.Log("Set cam speed: " + cameraSpeedValue);
    }

    public void SetMovementInput(Vector2 input)
    {
        _currentMovementInput = input;
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log($"Character OnCollisionStay {collision.gameObject.name}");
        if (collision.collider.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            _isTouchingObstacle = true;
            Debug.Log("Touching Obstacle");
        }

        if (collision.collider.TryGetComponent<Backwall>(out Backwall backwall))
        {
            _isTouchingBackwall = true;
            Debug.Log("Touching Backwall");
        }
        
        if (_isTouchingObstacle && _isTouchingBackwall)
        {
            _character.DealDamage(_character.Stats.MaxHealth);
        }
    }

    void FixedUpdate()
    {
        Vector3 inputDirection = up * _currentMovementInput.y + right * _currentMovementInput.x;
        Vector3 targetVelocity = inputDirection * MovementSpeed;
        
        Vector3 currentVelocity = _rigidbody.linearVelocity;
        
        float smoothedVelocityX = Mathf.MoveTowards(currentVelocity.x, targetVelocity.x, acceleration * Time.fixedDeltaTime);
        float smoothedVelocityY = Mathf.MoveTowards(currentVelocity.y, targetVelocity.y, acceleration * Time.fixedDeltaTime);

        _rigidbody.linearVelocity = new Vector3(smoothedVelocityX, smoothedVelocityY, 0f);
        
        Vector3 currentPos = _rigidbody.position;
        currentPos.x += _camSpeed * Time.fixedDeltaTime;
        currentPos.y = Mathf.Clamp(currentPos.y, MinCharacterOffsetY, MaxCharacterOffsetY);
        currentPos.z = 0f;
        
        _rigidbody.position = currentPos;
        
        _isTouchingObstacle = false;
        _isTouchingBackwall = false;
    }

    public void Rotate(float verticalInput, Transform visualTransform)
    {
        if (!visualTransform) return;
        float currentY = _rigidbody.position.y;
        bool hittingUpperBoundary = (currentY >= MaxCharacterOffsetY && verticalInput > 0f);
        bool hittingLowerBoundary = (currentY <= MinCharacterOffsetY && verticalInput < 0f);
        
        if (Mathf.Abs(verticalInput) > 0.01f && !hittingUpperBoundary && !hittingLowerBoundary)
        {
            _rotationX -= verticalInput * RotationSpeed * Time.fixedDeltaTime;
        }
        else
        {
            _rotationX = Mathf.MoveTowards(_rotationX, 0f, ReturnSpeed * Time.deltaTime);
        }
        
        _rotationX = Mathf.Clamp(_rotationX, -MaxRotationAngle, MaxRotationAngle);
        visualTransform.localRotation = Quaternion.Euler(_rotationX, 90f, 0f);
    }
}
