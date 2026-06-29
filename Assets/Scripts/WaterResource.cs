using UnityEngine;
using UnityEngine.Events;

public class WaterResource : MonoBehaviour
{
    [SerializeField] private float maxWater = 100f;
    [SerializeField] private float refillRate = 12.5f;
    private float _currentWater;
    
    public static UnityEvent<float, float> OnWaterChange = new UnityEvent<float, float>();
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentWater = maxWater;
        OnWaterChange?.Invoke(_currentWater, maxWater);
        
    }

    public bool TryConsumeWater(float amount)
    {
        if (_currentWater >= amount)
        {
            _currentWater -= amount;
            OnWaterChange?.Invoke(_currentWater, maxWater);
            return true;
        }
        return false;
    }

    public void Refill(float amount)
    {
        if (_currentWater < maxWater)
        {
            _currentWater = Mathf.Min(_currentWater + amount, maxWater);
            OnWaterChange?.Invoke(_currentWater, maxWater);
        }
    }

    public void RefillOverTime()
    {
        Refill(refillRate *  Time.deltaTime);
    }
}
