using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class BountyController : MonoBehaviour
{
    [SerializeField] private BountyProgressionData _bountyProgressionData;

    public static UnityEvent<int> OnScoreChange = new UnityEvent<int>();
    
    private void OnEnable()
    {
        Enemy.BountyOnDeath.AddListener(AddAmount);
    }

    private void OnDisable()
    {
        Enemy.BountyOnDeath.RemoveListener(AddAmount);
    }

    private void Start()
    { 
        OnScoreChange?.Invoke(_bountyProgressionData.currentScore);
    }

    private void AddAmount(int bountyAmount)
    {
        _bountyProgressionData.currentScore += bountyAmount;
        _bountyProgressionData.currentUpgradeScore += bountyAmount;
        
        OnScoreChange?.Invoke(_bountyProgressionData.currentScore);
    }
}
