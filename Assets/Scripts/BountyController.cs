using UnityEngine;
using UnityEngine.Events;

public class BountyController : MonoBehaviour
{
    private int _currentScore;
    private int _currentUpgradePoints;

    public static UnityEvent<int> OnScoreChanged;
    public static UnityEvent<int> OnUpgradePointsChanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Enemy.BountyOnDeath.AddListener(AddAmount);
    }

    private void AddAmount(int bountyAmount)
    {
        _currentScore += bountyAmount;
        _currentUpgradePoints += bountyAmount;
        OnScoreChanged?.Invoke(_currentScore);
        OnUpgradePointsChanged?.Invoke(_currentUpgradePoints);
    }

    private void SpendAmount(int amount)
    {
        _currentUpgradePoints -= amount;
        OnUpgradePointsChanged?.Invoke(_currentUpgradePoints);
    }
    
}
