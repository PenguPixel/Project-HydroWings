using UnityEngine;

[CreateAssetMenu(fileName = "LevelFlowData", menuName = "Scriptable Objects/LevelFlowData")]
public class LevelFlowData : ScriptableObject
{
    public int currentLevelIndex = 0;

    public void ResetLevelFlow()
    {
        currentLevelIndex = 0;
    }
}
