using UnityEngine;
using UnityEngine.Events;

public class WaterState : MonoBehaviour
{
    [SerializeField] private float maxWater = 100f;
    private float _currentWater;
    
    public static UnityEvent<float, float> OnWaterChange;
    
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

    public void RefillWater(float amount)
    {
        _currentWater += amount;
        OnWaterChange?.Invoke(_currentWater, maxWater);
    }   
}
