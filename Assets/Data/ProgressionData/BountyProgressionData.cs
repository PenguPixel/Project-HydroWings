using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "BountyProgressionData", menuName = "Scriptable Objects/BountyProgressionData")]
public class BountyProgressionData : ScriptableObject
{
    public int currentScore;
    public int currentUpgradeScore;
    
    public int currentHealthUpgradeCost;
    public int currentResourceUpgradeCost;
    public int currentDamageUpgradeCost;

    public void ResetToDefaults()
    {
        currentScore = 0;
        currentUpgradeScore = 0;
        currentHealthUpgradeCost = 100;
        currentResourceUpgradeCost = 100;
        currentDamageUpgradeCost = 100;
    }
}
