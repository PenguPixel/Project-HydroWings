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
    [SerializeField] private GameObject loseOverlayPanel;
    [SerializeField] private GameObject gameOverButtons;
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float panelTargetAlpha = 1f;
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
        if (loseOverlayPanel != null && gameOverText != null)
        {
            loseOverlayPanel.gameObject.SetActive(false);
            gameOverText.gameObject.SetActive(false);
            gameOverButtons.SetActive(false);
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
    
    public void TriggerLose()
    {
        if (isFading) return;

        if (loseOverlayPanel != null && loseOverlayPanel != null)
        {
            StartCoroutine("FadeToLoseScreen");
        }
    }

    private IEnumerator FadeToLoseScreen()
    {
        isFading = true;
        loseOverlayPanel.gameObject.SetActive(true);
        gameOverText.gameObject.SetActive(true);
        gameOverButtons.SetActive(true);
        
        Image panelImage = loseOverlayPanel.GetComponent<Image>();
        TextMeshProUGUI loseText = gameOverText.GetComponent<TextMeshProUGUI>();
        
        Color startPanelColor = new Color(0f,0f,0f,0f);
        Color targetPanelColor = new Color(0f, 0f, 0f, panelTargetAlpha);
        panelImage.color = startPanelColor;
        
        Color startTextColor = new Color(1f, 1f, 1f, 0f);
        Color targetTextColor = new Color(1f, 1f, 1f, 1f);
        loseText.color = startTextColor;
        

        float startGameSpeed = 1.0f;
        float targetGameSpeed = 0.0f;

        float elapsedTime = 0f;
        float lastTimeScale = 1f;

        bool canceledLerp = false;
        
        while (elapsedTime + 0.02f < fadeDuration)
        {
            if (Time.timeScale > lastTimeScale)
            {
                canceledLerp = true;
                break;
            }
            
            float currentFactor = elapsedTime / fadeDuration;
            
            panelImage.color = Color.Lerp(startPanelColor, targetPanelColor, currentFactor);
            loseText.color = Color.Lerp(startTextColor, targetTextColor, currentFactor);
            Time.timeScale = Mathf.Lerp(startGameSpeed, targetGameSpeed, currentFactor);
            lastTimeScale = Time.timeScale;
            
            lastGameTime = Time.timeScale;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        panelImage.color = targetPanelColor;
        if (!canceledLerp) Time.timeScale = targetGameSpeed;
        isFading = false;
    }
}