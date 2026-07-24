using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    [Header("Portrait UI")] 
    [SerializeField] private Image dolphImage;
    [SerializeField] private Image penguImage;
    
    [Header("Water UI")]
    [SerializeField] private Slider waterSlider;

    [Header("Health UI")]
    [SerializeField] private Slider healthSlider;
    
    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("PauseUI")] 
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject musicPlayer;
    
    [Header("GameOverUI")]
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private GameObject endOverlayPanel;
    [SerializeField] private GameObject gameOverButtons;
    
    [Header("Game Win Ui")]
    [SerializeField] private TextMeshProUGUI gameWinText;
    [SerializeField] private TextMeshProUGUI finalScoreHeadText;
    [SerializeField] private TextMeshProUGUI finalScoreNumberText;
    [SerializeField] private GameObject returnToTitleButton;
    
    [Header("Fade Values")]
    [SerializeField] private float fadeDuration = 1.0f;
    [SerializeField] private float panelTargetAlpha = 1f;
    
    private bool isFading = false;
    private bool isWin = false;
    
    private int _currentScore;
    private Image _endOverlayPanelImage;
    private AudioSource _audioSource = null;

    private void OnEnable()
    {
        WaterResource.OnWaterChange.AddListener(UpdateWaterUI);
        Character.OnHealthchange.AddListener(UpdateHealthUI);
        BountyController.OnScoreChange.AddListener(UpdateScoreUI);
        Character.OnPlayerDied.AddListener(TriggerLose);
        LolliOliBotHealth.OnBossDeath.AddListener(TriggerWin);
    }

    private void OnDisable()
    {
        WaterResource.OnWaterChange.RemoveListener(UpdateWaterUI);
        Character.OnHealthchange.RemoveListener(UpdateHealthUI);
        BountyController.OnScoreChange.RemoveListener(UpdateScoreUI);
        Character.OnPlayerDied.RemoveListener(TriggerLose);
        LolliOliBotHealth.OnBossDeath.RemoveListener(TriggerWin);
    }

    private void Awake()
    {
        if (CharacterSelection.SelectedWing == PlayableWing.DolphWing)
        {
            dolphImage.gameObject.SetActive(true);
            penguImage.gameObject.SetActive(false);
        }

        if (CharacterSelection.SelectedWing == PlayableWing.PenguWing)
        {
            dolphImage.gameObject.SetActive(false);
            penguImage.gameObject.SetActive(true);
        }
        
        endOverlayPanel.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
        gameOverButtons.SetActive(false);
        returnToTitleButton.SetActive(false);
        gameWinText.gameObject.SetActive(false);
        finalScoreHeadText.gameObject.SetActive(false);
        finalScoreNumberText.gameObject.SetActive(false);
        settingsWindow.SetActive(false);
        playButton.SetActive(false);
        quitButton.SetActive(false);
        
        _endOverlayPanelImage = endOverlayPanel.GetComponent<Image>();
        if (musicPlayer != null)
        {
            _audioSource = musicPlayer.GetComponent<AudioSource>();
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

    public void OpenSettings()
    {
        Time.timeScale = 0f;
        if(_audioSource != null) _audioSource.Pause();
        endOverlayPanel.gameObject.SetActive(true);
        _endOverlayPanelImage.color = new Color(0f, 0f, 0f, panelTargetAlpha);
        settingsWindow.SetActive(true);
        playButton.SetActive(true);
        quitButton.SetActive(true);
        CharacterPreviewHover.CharacterSelectionLocked = true;
    }

    public void CloseSettings()
    {
        _endOverlayPanelImage.color = new Color(0f, 0f, 0f, panelTargetAlpha);
        endOverlayPanel.gameObject.SetActive(false);
        settingsWindow.SetActive(false);
        playButton.SetActive(false);
        quitButton.SetActive(false);
        CharacterPreviewHover.CharacterSelectionLocked = false;
        if (_audioSource != null) _audioSource.UnPause();
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
        
        
        if (UnityEditor.EditorApplication.isPlaying)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    }
    
    public void TriggerLose()
    {
        if (isFading) return;
        isWin = false;

        if (endOverlayPanel != null && gameOverText != null && gameOverButtons != null)
        {
            StartCoroutine("FadeToEndScreen");
        }
    }
    
    private void TriggerWin()
    {
        if (isFading)  return;
        isWin = true;

        if (endOverlayPanel != null && gameWinText != null && gameOverButtons != null)
        {
            StartCoroutine("FadeToEndScreen");
        }
    }

    private IEnumerator FadeToEndScreen()
    {
        isFading = true;
        endOverlayPanel.gameObject.SetActive(true);

        if (!isWin)
        {
            gameOverText.gameObject.SetActive(true);
            gameOverButtons.SetActive(true);
            returnToTitleButton.SetActive(true);
            
            TextMeshProUGUI loseText = gameOverText.GetComponent<TextMeshProUGUI>();

            Color startPanelColor = new Color(0f, 0f, 0f, 0f);
            Color targetPanelColor = new Color(0f, 0f, 0f, panelTargetAlpha);
            _endOverlayPanelImage.color = startPanelColor;

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

                _endOverlayPanelImage.color = Color.Lerp(startPanelColor, targetPanelColor, currentFactor);
                loseText.color = Color.Lerp(startTextColor, targetTextColor, currentFactor);
                
                Time.timeScale = Mathf.Lerp(startGameSpeed, targetGameSpeed, currentFactor);
                lastTimeScale = Time.timeScale;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _endOverlayPanelImage.color = targetPanelColor;
            if (!canceledLerp) Time.timeScale = targetGameSpeed;
            isFading = false;
        }
        else
        {
            gameWinText.gameObject.SetActive(true);
            returnToTitleButton.SetActive(true);
            finalScoreHeadText.gameObject.SetActive(true);
            finalScoreNumberText.gameObject.SetActive(true);
            
            finalScoreNumberText.text = _currentScore.ToString();
            
            TextMeshProUGUI winText = gameWinText.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI finalScoreText = finalScoreNumberText.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI finalScoreHead = finalScoreHeadText.GetComponent<TextMeshProUGUI>();

            Color startPanelColor = new Color(0f, 0f, 0f, 0f);
            Color targetPanelColor = new Color(0f, 0f, 0f, panelTargetAlpha);
            _endOverlayPanelImage.color = startPanelColor;

            Color startTextColor = new Color(1f, 1f, 1f, 0f);
            Color targetTextColor = new Color(1f, 1f, 1f, 1f);
            winText.color = startTextColor;
            finalScoreText.color = startTextColor;
            finalScoreHead.color = startTextColor;


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

                _endOverlayPanelImage.color = Color.Lerp(startPanelColor, targetPanelColor, currentFactor);
                winText.color = Color.Lerp(startTextColor, targetTextColor, currentFactor);
                finalScoreText.color = Color.Lerp(startTextColor, targetTextColor, currentFactor);
                finalScoreHead.color = Color.Lerp(startTextColor, targetTextColor, currentFactor);
                
                Time.timeScale = Mathf.Lerp(startGameSpeed, targetGameSpeed, currentFactor);
                lastTimeScale = Time.timeScale;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            _endOverlayPanelImage.color = targetPanelColor;
            if (!canceledLerp) Time.timeScale = targetGameSpeed;
            isFading = false;
        }
    }
}
