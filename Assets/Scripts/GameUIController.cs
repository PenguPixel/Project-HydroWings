using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [Header("Water UI")]
    [SerializeField] private Slider waterSlider;

    [Header("Health UI")]
    [SerializeField] private Slider healthSlider;
    
    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    private int _currentScore;

    private void OnEnable()
    {
        WaterResource.OnWaterChange.AddListener(UpdateWaterUI);
        Character.OnHealthchange.AddListener(UpdateHealthUI);
        BountyController.OnScoreChange.AddListener(UpdateScoreUI);
    }

    private void OnDisable()
    {
        WaterResource.OnWaterChange.RemoveListener(UpdateWaterUI);
        Character.OnHealthchange.RemoveListener(UpdateHealthUI);
        BountyController.OnScoreChange.RemoveListener(UpdateScoreUI);
    }
    
    private void UpdateScoreUI(int newScore)
    {
        _currentScore = newScore;
        scoreText.text = _currentScore.ToString();
    }

    private void UpdateWaterUI(float currentWater, float maxWater)
    {
        waterSlider.maxValue = maxWater;
        waterSlider.value = currentWater;
    }

    private void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }
}