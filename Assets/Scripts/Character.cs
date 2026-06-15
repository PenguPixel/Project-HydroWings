using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    public CharacterStats Stats;
    public PlayerController CharacterController;

    private InputAction _moveAction;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _moveAction = InputSystem.actions.FindAction("Move");
        
        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementVector = _moveAction.ReadValue<Vector2>();
        CharacterController.Move(movementVector);
        CharacterController.Rotate(movementVector.y);

        movementVector += new Vector2((CameraController.speed / CharacterController.MovementSpeed) , 0);
        CharacterController.Move(movementVector);
    }
}