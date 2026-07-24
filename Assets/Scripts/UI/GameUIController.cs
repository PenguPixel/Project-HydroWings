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

    [Header("Dynamic Slider Width")]
    [Tooltip("Der ursprüngliche Max-Health-Wert ohne Upgrade.")]
    [SerializeField] private float baseHealthValue = 100f;

    [Tooltip("Der ursprüngliche maximale Wasserwert ohne Upgrade.")]
    [SerializeField] private float baseWaterValue = 100f;

    [Tooltip("Optional. Leer lassen, dann wird der RectTransform des Health-Sliders benutzt.")]
    [SerializeField] private RectTransform healthSliderRect;

    [Tooltip("Optional. Leer lassen, dann wird der RectTransform des Water-Sliders benutzt.")]
    [SerializeField] private RectTransform waterSliderRect;

    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI scoreText;

    [Header("Pause UI")]
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private GameObject quitButton;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject musicPlayer;

    [Header("Game Over UI")]
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private GameObject endOverlayPanel;
    [SerializeField] private GameObject gameOverButtons;

    [Header("Game Win UI")]
    [SerializeField] private TextMeshProUGUI gameWinText;
    [SerializeField] private TextMeshProUGUI finalScoreHeadText;
    [SerializeField] private TextMeshProUGUI finalScoreNumberText;
    [SerializeField] private GameObject returnToTitleButton;

    [Header("Fade Values")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float panelTargetAlpha = 1f;

    private bool isFading;
    private bool isWin;

    private int _currentScore;
    private Image _endOverlayPanelImage;
    private AudioSource _audioSource;

    private float _baseHealthSliderWidth;
    private float _baseWaterSliderWidth;

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
        SetupPortrait();
        SetupSliderReferences();
        SetupMenus();

        if (endOverlayPanel)
        {
            _endOverlayPanelImage = endOverlayPanel.GetComponent<Image>();
        }

        if (musicPlayer)
        {
            _audioSource = musicPlayer.GetComponent<AudioSource>();
        }
    }

    private void SetupPortrait()
    {
        if (CharacterSelection.SelectedWing == PlayableWing.DolphWing)
        {
            if (dolphImage)
            {
                dolphImage.gameObject.SetActive(true);
            }

            if (penguImage)
            {
                penguImage.gameObject.SetActive(false);
            }
        }
        else if (CharacterSelection.SelectedWing == PlayableWing.PenguWing)
        {
            if (dolphImage)
            {
                dolphImage.gameObject.SetActive(false);
            }

            if (penguImage)
            {
                penguImage.gameObject.SetActive(true);
            }
        }
    }

    private void SetupSliderReferences()
    {
        /*
         * Wenn die RectTransforms im Inspector nicht manuell
         * eingetragen wurden, werden automatisch die RectTransforms
         * der Slider verwendet.
         */

        if (!healthSliderRect && healthSlider)
        {
            healthSliderRect = healthSlider.GetComponent<RectTransform>();
        }

        if (!waterSliderRect && waterSlider)
        {
            waterSliderRect = waterSlider.GetComponent<RectTransform>();
        }

        /*
         * Die aktuelle Breite in der Szene wird als ursprüngliche
         * Basisbreite gespeichert.
         */

        if (healthSliderRect)
        {
            _baseHealthSliderWidth = healthSliderRect.rect.width;
        }

        if (waterSliderRect)
        {
            _baseWaterSliderWidth = waterSliderRect.rect.width;
        }

        if (healthSlider)
        {
            healthSlider.minValue = 0f;
        }

        if (waterSlider)
        {
            waterSlider.minValue = 0f;
        }
    }

    private void SetupMenus()
    {
        if (endOverlayPanel)
        {
            endOverlayPanel.SetActive(false);
        }

        if (gameOverText)
        {
            gameOverText.gameObject.SetActive(false);
        }

        if (gameOverButtons)
        {
            gameOverButtons.SetActive(false);
        }

        if (returnToTitleButton)
        {
            returnToTitleButton.SetActive(false);
        }

        if (gameWinText)
        {
            gameWinText.gameObject.SetActive(false);
        }

        if (finalScoreHeadText)
        {
            finalScoreHeadText.gameObject.SetActive(false);
        }

        if (finalScoreNumberText)
        {
            finalScoreNumberText.gameObject.SetActive(false);
        }

        if (settingsWindow)
        {
            settingsWindow.SetActive(false);
        }

        if (playButton)
        {
            playButton.SetActive(false);
        }

        if (quitButton)
        {
            quitButton.SetActive(false);
        }
    }

    private void UpdateScoreUI(int newScore)
    {
        _currentScore = newScore;

        if (scoreText)
        {
            scoreText.text = _currentScore.ToString();
        }
    }

    private void UpdateWaterUI(float currentWater, float maxWater)
    {
        if (!waterSlider)
        {
            return;
        }

        waterSlider.maxValue = maxWater;
        waterSlider.value = currentWater;

        UpdateWaterSliderWidth(maxWater);
    }

    private void UpdateHealthUI(float currentHealth, float maxHealth)
    {
        if (!healthSlider)
        {
            return;
        }

        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;

        UpdateHealthSliderWidth(maxHealth);
    }

    private void UpdateHealthSliderWidth(float currentMaxHealth)
    {
        if (!healthSliderRect)
        {
            return;
        }

        if (baseHealthValue <= 0f)
        {
            Debug.LogWarning("GameUIController: Base Health Value muss größer als 0 sein.");
            return;
        }

        float healthMultiplier = currentMaxHealth / baseHealthValue;

        float newWidth = _baseHealthSliderWidth * healthMultiplier;

        float oldWidth = healthSliderRect.rect.width;

        float widthDifference = newWidth - oldWidth;

        Vector2 anchoredPosition = healthSliderRect.anchoredPosition;

        anchoredPosition.x += widthDifference * healthSliderRect.pivot.x;

        healthSliderRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);

        healthSliderRect.anchoredPosition = anchoredPosition;
    }

    private void UpdateWaterSliderWidth(float currentMaxWater)
    {
        if (!waterSliderRect)
        {
            return;
        }

        if (baseWaterValue <= 0f)
        {
            Debug.LogWarning("GameUIController: Base Water Value muss größer als 0 sein.");
            return;
        }

        float waterMultiplier = currentMaxWater / baseWaterValue;

        float newWidth = _baseWaterSliderWidth * waterMultiplier;

        float oldWidth = waterSliderRect.rect.width;

        float widthDifference = newWidth - oldWidth;

        Vector2 anchoredPosition = waterSliderRect.anchoredPosition;

        anchoredPosition.x += widthDifference * waterSliderRect.pivot.x;

        waterSliderRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, newWidth);

        waterSliderRect.anchoredPosition = anchoredPosition;
    }

    public void OpenSettings()
    {
        Time.timeScale = 0f;

        if (_audioSource)
        {
            _audioSource.Pause();
        }

        if (endOverlayPanel)
        {
            endOverlayPanel.SetActive(true);
        }

        if (_endOverlayPanelImage)
        {
            _endOverlayPanelImage.color = new Color(0f, 0f, 0f, panelTargetAlpha);
        }

        if (settingsWindow)
        {
            settingsWindow.SetActive(true);
        }

        if (playButton)
        {
            playButton.SetActive(true);
        }

        if (quitButton)
        {
            quitButton.SetActive(true);
        }

        CharacterPreviewHover.CharacterSelectionLocked = true;
    }

    public void CloseSettings()
    {
        if (_endOverlayPanelImage)
        {
            _endOverlayPanelImage.color = new Color(0f, 0f, 0f, panelTargetAlpha);
        }

        if (endOverlayPanel)
        {
            endOverlayPanel.SetActive(false);
        }

        if (settingsWindow)
        {
            settingsWindow.SetActive(false);
        }

        if (playButton)
        {
            playButton.SetActive(false);
        }

        if (quitButton)
        {
            quitButton.SetActive(false);
        }

        CharacterPreviewHover.CharacterSelectionLocked = false;

        if (_audioSource)
        {
            _audioSource.UnPause();
        }

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
        if (isFading)
        {
            return;
        }

        isWin = false;

        if (endOverlayPanel && gameOverText && gameOverButtons)
        {
            StartCoroutine(FadeToEndScreen());
        }
    }

    private void TriggerWin()
    {
        if (isFading)
        {
            return;
        }

        isWin = true;

        if (endOverlayPanel && gameWinText && returnToTitleButton)
        {
            StartCoroutine(FadeToEndScreen());
        }
    }

    private IEnumerator FadeToEndScreen()
    {
        isFading = true;

        if (endOverlayPanel)
        {
            endOverlayPanel.SetActive(true);
        }

        if (!isWin)
        {
            yield return FadeLoseScreen();
        }
        else
        {
            yield return FadeWinScreen();
        }

        isFading = false;
    }

    private IEnumerator FadeLoseScreen()
    {
        gameOverText.gameObject.SetActive(true);
        gameOverButtons.SetActive(true);
        returnToTitleButton.SetActive(true);

        Color startPanelColor = new Color(0f, 0f, 0f, 0f);

        Color targetPanelColor = new Color(0f, 0f, 0f, panelTargetAlpha);

        Color startTextColor = new Color(1f, 1f, 1f, 0f);

        Color targetTextColor = new Color(1f, 1f, 1f, 1f);

        if (_endOverlayPanelImage)
        {
            _endOverlayPanelImage.color = startPanelColor;
        }

        gameOverText.color = startTextColor;

        float elapsedTime = 0f;
        float lastTimeScale = 1f;
        bool canceledLerp = false;

        while (elapsedTime < fadeDuration)
        {
            if (Time.timeScale > lastTimeScale)
            {
                canceledLerp = true;
                break;
            }

            float factor = Mathf.Clamp01(elapsedTime / fadeDuration);

            if (_endOverlayPanelImage)
            {
                _endOverlayPanelImage.color = Color.Lerp(startPanelColor, targetPanelColor, factor);
            }

            gameOverText.color = Color.Lerp(startTextColor, targetTextColor, factor);

            Time.timeScale = Mathf.Lerp(1f, 0f, factor);

            lastTimeScale = Time.timeScale;

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        if (_endOverlayPanelImage)
        {
            _endOverlayPanelImage.color = targetPanelColor;
        }

        gameOverText.color = targetTextColor;

        if (!canceledLerp)
        {
            Time.timeScale = 0f;
        }
    }

    private IEnumerator FadeWinScreen()
    {
        gameWinText.gameObject.SetActive(true);
        returnToTitleButton.SetActive(true);
        finalScoreHeadText.gameObject.SetActive(true);
        finalScoreNumberText.gameObject.SetActive(true);

        finalScoreNumberText.text = _currentScore.ToString();

        Color startPanelColor = new Color(0f, 0f, 0f, 0f);

        Color targetPanelColor = new Color(0f, 0f, 0f, panelTargetAlpha);

        Color startTextColor = new Color(1f, 1f, 1f, 0f);

        Color targetTextColor = new Color(1f, 1f, 1f, 1f);

        if (_endOverlayPanelImage)
        {
            _endOverlayPanelImage.color = startPanelColor;
        }

        gameWinText.color = startTextColor;

        finalScoreNumberText.color = startTextColor;

        finalScoreHeadText.color = startTextColor;

        float elapsedTime = 0f;
        float lastTimeScale = 1f;
        bool canceledLerp = false;

        while (elapsedTime < fadeDuration)
        {
            if (Time.timeScale > lastTimeScale)
            {
                canceledLerp = true;
                break;
            }

            float factor = Mathf.Clamp01(elapsedTime / fadeDuration);

            if (_endOverlayPanelImage)
            {
                _endOverlayPanelImage.color = Color.Lerp(startPanelColor, targetPanelColor, factor);
            }

            gameWinText.color = Color.Lerp(startTextColor, targetTextColor, factor);

            finalScoreNumberText.color = Color.Lerp(startTextColor, targetTextColor, factor);

            finalScoreHeadText.color = Color.Lerp(startTextColor, targetTextColor, factor);

            Time.timeScale = Mathf.Lerp(1f, 0f, factor);

            lastTimeScale = Time.timeScale;

            elapsedTime += Time.unscaledDeltaTime;

            yield return null;
        }

        if (_endOverlayPanelImage)
        {
            _endOverlayPanelImage.color = targetPanelColor;
        }

        gameWinText.color = targetTextColor;

        finalScoreNumberText.color = targetTextColor;

        finalScoreHeadText.color = targetTextColor;

        if (!canceledLerp)
        {
            Time.timeScale = 0f;
        }
    }
}