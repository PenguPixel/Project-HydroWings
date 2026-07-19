using System;
using System.Collections;
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
    
    [Header("GameOverUI")]
    [SerializeField] private TextMeshProUGUI gameOverText;

    [SerializeField] private Image loseOverlayPanel;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float targetAlpha = 0.9f;
    private bool isFading = false;
    

    private int _currentScore;

    private void OnEnable()
    {
        WaterResource.OnWaterChange.AddListener(UpdateWaterUI);
        Character.OnHealthchange.AddListener(UpdateHealthUI);
        BountyController.OnScoreChange.AddListener(UpdateScoreUI);
        Character.OnPlayerDied.AddListener(TriggerLose);
    }

    private void OnDisable()
    {
        WaterResource.OnWaterChange.RemoveListener(UpdateWaterUI);
        Character.OnHealthchange.RemoveListener(UpdateHealthUI);
        BountyController.OnScoreChange.RemoveListener(UpdateScoreUI);
        Character.OnPlayerDied.RemoveListener(TriggerLose);
    }

    private void Awake()
    {
        if (loseOverlayPanel != null)
        {
            loseOverlayPanel.gameObject.SetActive(false);
        }
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
    
    private void TriggerLose()
    {
        if (isFading) return;

        if (loseOverlayPanel != null && loseOverlayPanel != null)
        {
            StartCoroutine(FadeToDark());
        }
    }

    private IEnumerator FadeToDark()
    {
        isFading = true;
        
        Color startColor = new Color(0f,0f,0f,0f);
        loseOverlayPanel.color = startColor;

        float elapsedTime = 0f;
        Color targetColor = new Color(0f, 0f, 0f, targetAlpha);

        while (elapsedTime < fadeDuration)
        {
            float curentFactor = elapsedTime / fadeDuration;
            
            loseOverlayPanel.color = Color.Lerp(startColor, targetColor, curentFactor);
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        loseOverlayPanel.color = targetColor;
        isFading = false;
    }
}