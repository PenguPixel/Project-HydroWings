using System;
using UnityEngine;
using UnityEngine.Events;

public class BountyController : MonoBehaviour
{
    private int _currentScore;
    private int _currentUpgradePoints;

    public static UnityEvent<int> OnScoreChange = new UnityEvent<int>();
    public static UnityEvent<int> OnUpgradePointsChange = new UnityEvent<int>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
        _currentUpgradePoints = 0;
        OnScoreChange?.Invoke(_currentScore);
    }

    private void AddAmount(int bountyAmount)
    {
        _currentScore += bountyAmount;
        _currentUpgradePoints += bountyAmount;
        OnScoreChange?.Invoke(_currentScore);
        OnUpgradePointsChange?.Invoke(_currentUpgradePoints);
    }

    private void SpendAmount(int amount)
    {
        _currentUpgradePoints -= amount;
        OnUpgradePointsChange?.Invoke(_currentUpgradePoints);
    }
    
}
