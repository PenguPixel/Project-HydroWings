using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "BountyProgressionData", menuName = "Scriptable Objects/BountyProgressionData")]
public class BountyProgressionData : ScriptableObject
{
    public int currentScore;
    public int currentUpgradeScore;

    public void ResetToDefaults()
    {
        currentScore = 0;
        currentUpgradeScore = 0;
    }
}
