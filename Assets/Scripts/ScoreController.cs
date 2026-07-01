using UnityEngine;
using UnityEngine.Events;

public class ScoreController : MonoBehaviour
{
    private int _currentScore;

    public static UnityEvent<int> OnScoreChanged;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Enemy.BountyOnDeath.AddListener(ChangeScore);
    }

    private void ChangeScore(int bountyAmount)
    {
        _currentScore += bountyAmount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
