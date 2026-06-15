using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    
    [SerializeField] private List<WeaponPoint> activeWeaponPoints = new List<WeaponPoint>();

    [Header("Shooting Settings")]
    //[SerializeField] private float fireRate = 0.4f;
    //private float _fireCooldownTimer;

    private InputAction _attackAction;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _attackAction = InputSystem.actions.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        InputHandling();
    }

    private void InputHandling()
    {
        if (_attackAction != null && _attackAction.IsPressed())
        {
            foreach (WeaponPoint point in activeWeaponPoints)
            {
                if (point != point.AutoFire)
                {
                    point.Shoot();
                }
                else return;
            }
        }
    }


    public void RegisterWeaponPoint(WeaponPoint point)
        {
            if (!activeWeaponPoints.Contains(point))
            {
                activeWeaponPoints.Add(point);
            }
        }

    public void UnregisterWeaponPoint(WeaponPoint weaponPoint)
    {
        if (activeWeaponPoints.Contains(weaponPoint))
        {
            activeWeaponPoints.Remove(weaponPoint);
        }
    }
}
