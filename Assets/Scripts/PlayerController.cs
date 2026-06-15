using System;
using UnityEngine;
using static UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    private CharacterController _characterConroller;

    public float MovementSpeed = 5f;

    public float RotationSpeed = 50f;
    public float ReturnSpeed = 60f;
    public float MaxRotationAngle = 25f;
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
    }

    public void Rotate(float verticalInput)
    {
        if (Mathf.Abs(verticalInput) > 0.01f)
        {
            _rotationX -= verticalInput * RotationSpeed * Time.deltaTime;
        }
        else
        {
            _rotationX = Mathf.MoveTowards(_rotationX, 0f, ReturnSpeed * Time.deltaTime);
        }
        
        _rotationX = Math.Clamp(_rotationX, -MaxRotationAngle, MaxRotationAngle);
        transform.localRotation = Quaternion.Euler(_rotationX,90f, 0f);
    }
}
