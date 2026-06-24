using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
   [Header("Shooting Settings")]

    private InputAction _attackAction;
    public static System.Action OnManualShootPressed;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _attackAction = InputSystem.actions.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        if (_attackAction != null && _attackAction.IsPressed())
        {
            OnManualShootPressed?.Invoke();
        }
    }
}
