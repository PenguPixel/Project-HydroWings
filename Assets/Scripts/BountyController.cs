using System;
using UnityEngine;
using UnityEngine.Events;

public class BountyController : MonoBehaviour
{
    private int _currentScore;
    public int CurrentUpgradePoints { get; private set; }

    public static UnityEvent<int> OnScoreChange = new UnityEvent<int>();
    public static UnityEvent<int> OnUpgradePointsChange = new UnityEvent<int>();
    
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
        _currentScore = 0;
        CurrentUpgradePoints = 0;
        OnScoreChange?.Invoke(_currentScore);
    }

    private void AddAmount(int bountyAmount)
    {
        _currentScore += bountyAmount;
        CurrentUpgradePoints += bountyAmount;
        OnScoreChange?.Invoke(_currentScore);
        OnUpgradePointsChange?.Invoke(CurrentUpgradePoints);
    }

    private void SpendAmount(int amount)
    {
        CurrentUpgradePoints -= amount;
        OnUpgradePointsChange?.Invoke(CurrentUpgradePoints);
    }
}
