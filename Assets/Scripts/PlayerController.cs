using System;
using UnityEngine;
using static UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    //private CharacterController _characterController;
    private Rigidbody _rigidbody;

    [SerializeField]public float MovementSpeed = 5f;

    [SerializeField]private float RotationSpeed = 50f;
    [SerializeField]private float ReturnSpeed = 60f;
    [SerializeField]private float MaxRotationAngle = 25f;
    [SerializeField] private float MaxCharacterOffsetY = 10f;
    [SerializeField] private float MinCharacterOffsetY = -10f;
    
    private float _rotationX = 0f;

    void Awake()
    {
        _rigidbody = GetComponentInChildren<Rigidbody>();
        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = false;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

    }
    

    public void Move(Vector2 movementVector, float cameraMoveSpeed)
    {
        Vector3 inputDirection = up * movementVector.y + right * movementVector.x;
        Vector3 desiredMovement = inputDirection * MovementSpeed;
        
        desiredMovement.x += cameraMoveSpeed;
        
        _rigidbody.linearVelocity = desiredMovement;
        
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
