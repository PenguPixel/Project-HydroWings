using UnityEngine;

[CreateAssetMenu(fileName = "WeaponPointStats", menuName = "Scriptable Objects/WeaponPointStats")]
public class WeaponPointStats : ScriptableObject
{
    public bool IsEnemyWeapon = false;
    public bool IsAutoFire = false;
    public float FireRate = 1.0f;
    public float WeaponRange = 5.0f;
}
