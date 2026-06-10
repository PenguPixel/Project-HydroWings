using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController _characterConroller;

    public float MovementSpeed = 2f;

    public float AccelerationSpeed = 1.5f;

    public float RotationSpeed = 5f;

    private float _rotationX;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _characterConroller = GetComponent<CharacterController>();
    }

    public void Move(Vector2 movementVector)
    {
        Vector3 move = transform.up * movementVector.y + transform.forward * movementVector.x;
        move = move * (MovementSpeed * Time.deltaTime);
        _characterConroller.Move(move);
    }

    public void Rotate(Vector2 rotationVector)
    {
        _rotationX += rotationVector.y * RotationSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(_rotationX,90, 0);
    }
}
