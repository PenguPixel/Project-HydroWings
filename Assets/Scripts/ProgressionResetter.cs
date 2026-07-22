using UnityEngine;

public class ProgressionResetter : MonoBehaviour
{
    [SerializeField] private BountyProgressionData _bountyProgressionData;
    [SerializeField] private LevelFlowData _levelFlowData;
    
    void Awake()
    {
        _bountyProgressionData.ResetToDefaults();
        _levelFlowData.ResetLevelFlow();
    }
}
