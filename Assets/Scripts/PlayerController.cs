using System;
using UnityEngine;
using static UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    private CharacterController _characterConroller;

    [SerializeField]public float MovementSpeed = 5f;

    [SerializeField]private float RotationSpeed = 50f;
    [SerializeField]private float ReturnSpeed = 60f;
    [SerializeField]private float MaxRotationAngle = 25f;
    [SerializeField] private float MaxCharacterOffsetY = 10f;
    [SerializeField] private float MinCharacterOffsetY = -10f;
    
    private float _rotationX = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterConroller = GetComponent<CharacterController>();
    }
    

    public void Move(Vector2 movementVector)
    {
        Vector3 moveDirection = up * movementVector.y + right * movementVector.x;

        Vector3 movement = moveDirection * (MovementSpeed * Time.deltaTime);
        
        _characterConroller.Move(movement);

        Vector3 clampedPosition = transform.position;
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, MinCharacterOffsetY, MaxCharacterOffsetY);
        transform.position = clampedPosition;
    }

    public void Rotate(float verticalInput)
    {
        float currentY = transform.position.y;
        bool hittingUpperBoundary = (currentY >= MaxCharacterOffsetY && verticalInput > 0f);
        bool hittingLowerBoundary = (currentY <= MinCharacterOffsetY && verticalInput < 0f);
        
        if (Mathf.Abs(verticalInput) > 0.01f && !hittingUpperBoundary && !hittingLowerBoundary)
        {
            _rotationX -= verticalInput * RotationSpeed * Time.deltaTime;
        }
        else
        {
            _rotationX = Mathf.MoveTowards(_rotationX, 0f, ReturnSpeed * Time.deltaTime);
        }
        
        _rotationX = Mathf.Clamp(_rotationX, -MaxRotationAngle, MaxRotationAngle);
        transform.localRotation = Quaternion.Euler(_rotationX,90f, 0f);
    }
}
