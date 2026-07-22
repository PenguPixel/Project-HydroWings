using UnityEngine;

public class ScoreResetter : MonoBehaviour
{
    [SerializeField] private BountyProgressionData _bountyProgressionData;
    
    void Awake()
    {
        _bountyProgressionData.ResetToDefaults();
    }
}
