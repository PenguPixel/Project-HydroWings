using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    
    [SerializeField] private List<WeaponPoint> activeWeaponPoints = new List<WeaponPoint>();

    [Header("Shooting Settings")]
    [SerializeField] private float fireRate = 0.4f;
    private float _fireCooldownTimer;

    private InputAction _attackAction;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _attackAction = InputSystem.actions.FindAction("Attack");
    }

    // Update is called once per frame
    void Update()
    {
        CooldownHandling();
        InputHandling();
    }

    private void InputHandling()
    {
        if (_attackAction != null && _attackAction.IsPressed() &&  _fireCooldownTimer <= 0f)
        {
            Shoot();
            _fireCooldownTimer = fireRate;
        }
    }

    private void Shoot()
    {
        foreach (WeaponPoint point in activeWeaponPoints)
        {
            if (point != null)
            {
                point.Fire();
            }
        }
    }

    private void CooldownHandling()
    {
        if (_fireCooldownTimer > 0f)
        {
            _fireCooldownTimer -= Time.deltaTime;
        }
    }

    public void RegisterWeaponPoint(WeaponPoint point)
        {
            if (!activeWeaponPoints.Contains(point))
            {
                activeWeaponPoints.Add(point);
            }
        }

    public void UnregisterWeaponpoint(WeaponPoint weaponPoint)
    {
        if (activeWeaponPoints.Contains(weaponPoint))
        {
            activeWeaponPoints.Remove(weaponPoint);
        }
    }
}
