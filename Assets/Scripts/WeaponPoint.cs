using System;
using UnityEngine;

public class WeaponPoint : MonoBehaviour
{
    [SerializeField] private ProjectileStats stats;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        WeaponController controller = GetComponentInParent<WeaponController>();

        if (controller != null)
        {
            controller.RegisterWeaponPoint(this);
        }
        else
        {
            Debug.Log($"No WeaponController on {gameObject.name} ");
        }
    }

    private void OnDestroy()
    {
        WeaponController controller = GetComponentInParent<WeaponController>();
        if (controller != null)
        {
            controller.UnregisterWeaponpoint(this);
        }
    }

    public void Fire()
    {
        GameObject prefabToSpawn = stats.GetRandomProjectilePrefab();

        if (prefabToSpawn != null)
        {
            Instantiate(prefabToSpawn, transform.position, transform.rotation);
        }
    }
}
