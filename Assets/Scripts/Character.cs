using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    public CharacterStats Stats;
    public CameraStats CameraStats;
    public PlayerController CharacterController;

    private InputAction _moveAction;
    private float _cameraMoveSpeed;
    private float _currentHealth;

   // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentHealth = Stats.MaxHealth;
        _moveAction = InputSystem.actions.FindAction("Move");

        Cursor.visible = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementVector = _moveAction.ReadValue<Vector2>();
        CharacterController.Move(movementVector);
        CharacterController.Rotate(movementVector.y);

        movementVector += new Vector2((CameraStats.speed / CharacterController.MovementSpeed) , 0);     // Movement o x-axis to the right along cam movement
        CharacterController.Move(movementVector);
    }

    public void DealDamage(float incomingDamage)
    {
        float wouldBeHealth = _currentHealth - incomingDamage;
        if (wouldBeHealth < 0)
        {
            wouldBeHealth = 0;
        }

        Debug.Log(
            $"{this.name} wurde getroffen und hat {incomingDamage} Schaden genommen. Verbleibendes Leben: {_currentHealth}");
        if (wouldBeHealth == 0)
        {
            Destroy(gameObject);
            return;
        }

        _currentHealth = wouldBeHealth;
    }
}