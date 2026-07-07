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

    void Awake()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
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

    void FixedUpdate()
    {
        Vector3 inputDirection = up * _currentMovementInput.y + right * _currentMovementInput.x;
        Vector3 targetVelocity = new Vector3(
            (inputDirection.x * MovementSpeed) + _camSpeed,
            inputDirection.y * MovementSpeed,
            0f
        );
        
        Vector3 currentVelocity = _rigidbody.linearVelocity;
        
        Vector3 velocityDiff = targetVelocity - currentVelocity;
        
        Vector3 force = velocityDiff * (acceleration * Time.fixedDeltaTime);
        force.z = 0f;
        
        _rigidbody.AddForce(force, ForceMode.VelocityChange);
        
        /*float smoothedInputX = Mathf.MoveTowards(currentVelocity.x - _camSpeed, targetVelocity.x, acceleration * Time.deltaTime);
        float smoothedInputY = Mathf.MoveTowards(currentVelocity.y, targetVelocity.y, acceleration * Time.deltaTime);

        Vector3 finalVelocity = new Vector3(smoothedInputX + _camSpeed, smoothedInputY, 0f);
        _rigidbody.linearVelocity = finalVelocity;*/
        
        Vector3 currentPos = _rigidbody.position;
        currentPos.y = Mathf.Clamp(currentPos.y, MinCharacterOffsetY, MaxCharacterOffsetY);
        currentPos.z = 0f;
        
        _rigidbody.position = currentPos;
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
