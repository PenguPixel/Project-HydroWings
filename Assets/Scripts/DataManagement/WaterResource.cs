using UnityEngine;
using UnityEngine.Events;

public class WaterResource : MonoBehaviour
{
    private float maxWater;
    private float refillRate;

    private float _currentWater;

    public static UnityEvent<float, float> OnWaterChange =
        new UnityEvent<float, float>();

    private void Start()
    {
        Character character =
            GetComponent<Character>();

        if (character != null &&
            character.Stats != null)
        {
            maxWater =
                character.CurrentMaxWaterResource;

            refillRate =
                character.Stats.WaterRefillRate;

            Debug.Log(
                $"{name}: MaxWater = {maxWater}, " +
                $"RefillRate = {refillRate}"
            );
        }
        else
        {
            Debug.LogWarning(
                $"WaterResource auf {name}: " +
                "Keine CharacterStats gefunden."
            );
        }

        _currentWater = maxWater;

        OnWaterChange?.Invoke(
            _currentWater,
            maxWater
        );
    }

    public bool TryConsumeWater(float amount)
    {
        if (_currentWater >= amount)
        {
            _currentWater -= amount;

            OnWaterChange?.Invoke(
                _currentWater,
                maxWater
            );

            return true;
        }

        return false;
    }

    public void Refill(float amount)
    {
        if (_currentWater < maxWater)
        {
            _currentWater =
                Mathf.Min(
                    _currentWater + amount,
                    maxWater
                );

            OnWaterChange?.Invoke(
                _currentWater,
                maxWater
            );
        }
    }

    public void RefillOverTime()
    {
        Refill(
            refillRate * Time.deltaTime
        );
    }
}