using UnityEngine;

[CreateAssetMenu(fileName = "PlayerProgressionData", menuName = "Scriptable Objects/PlayerProgressionData")]
public class PlayerProgressionData : ScriptableObject
{
    public float maxHealth;
    public float maxResource;
    public float attackDamage;

    public void ResetToDefaults()
    {
        maxHealth = 0f;
        maxResource = 0f;
        attackDamage = 0f;
    }
}
