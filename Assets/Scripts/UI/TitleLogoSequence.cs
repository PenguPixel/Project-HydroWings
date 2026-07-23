using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleLogoSequence : MonoBehaviour
{
    [Header("Canvas Groups")]
    [SerializeField] private CanvasGroup renderEnderLogo;
    [SerializeField] private CanvasGroup penguPixelLogo;
    [SerializeField] private CanvasGroup presentsText;
    [SerializeField] private CanvasGroup hydroWingsLogo;
    [SerializeField] private CanvasGroup pressAnyKeyText;
    [SerializeField] private CanvasGroup mainMenuButtons;

    [Header("Timing")]
    [SerializeField] private float startFadeTime;
    [SerializeField] private float startDelay;
    [SerializeField] private float fadeTime;
    [SerializeField] private float buttonFadeTime;
    [SerializeField] private float presentsDelay;
    [SerializeField] private float visibleTime;
    [SerializeField] private float hydroLogoDelay;
    [SerializeField] private float pressAnyKeyDelay;

    [Header("Scene Transition")]
    [SerializeField] private Image fadeImage;
    [SerializeField] private float sceneFadeTime;
    [SerializeField] private string characterSelectSceneName = "CharacterSelectScene";

    [Header("Music")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Slider musicSlider;

    private const string MusicVolumeKey = "MusicVolume";

    private bool menuOpened;
    private bool sceneIsLoading;

    private Coroutine sequenceCoroutine;
    private Coroutine blinkCoroutine;
    private Coroutine startFadeCoroutine;

    private void Start()
    {
        SetupMusicVolume();

        HideIntroObjects();
        HideMenu();
        PrepareFadeImageBlack();

        sequenceCoroutine = StartCoroutine(StartWithFade());
    }

    private void Update()
    {
        if (sceneIsLoading)
            return;

        bool pressed =
            (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame) ||
            (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) ||
            (Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame);

        if (pressed && !menuOpened)
        {
            StartCoroutine(SkipToMainMenu());
        }
    }

    private void SetupMusicVolume()
    {
        float savedVolume = PlayerPrefs.GetFloat(MusicVolumeKey, 1f);

        if (musicSource != null)
            musicSource.volume = savedVolume;

        if (musicSlider != null)
        {
            musicSlider.minValue = 0f;
            musicSlider.maxValue = 1f;
            musicSlider.wholeNumbers = false;
            musicSlider.value = savedVolume;

            musicSlider.onValueChanged.RemoveListener(SetMusicVolume);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (musicSource != null)
            musicSource.volume = volume;

        PlayerPrefs.SetFloat(MusicVolumeKey, volume);
        PlayerPrefs.Save();
    }

    private IEnumerator StartWithFade()
    {
        startFadeCoroutine = StartCoroutine(FadeImageTo(0f, startFadeTime));
        yield return startFadeCoroutine;

        sequenceCoroutine = StartCoroutine(FadeSequence());
    }

    private IEnumerator FadeSequence()
    {
        yield return new WaitForSeconds(startDelay);

        StartCoroutine(FadeTo(renderEnderLogo, 1f, fadeTime));
        StartCoroutine(FadeTo(penguPixelLogo, 1f, fadeTime));

        yield return new WaitForSeconds(presentsDelay);

        yield return FadeTo(presentsText, 1f, fadeTime);

        yield return new WaitForSeconds(visibleTime);

        StartCoroutine(FadeTo(renderEnderLogo, 0f, fadeTime));
        StartCoroutine(FadeTo(penguPixelLogo, 0f, fadeTime));
        StartCoroutine(FadeTo(presentsText, 0f, fadeTime));

        yield return new WaitForSeconds(fadeTime + hydroLogoDelay);

        renderEnderLogo.gameObject.SetActive(false);
        penguPixelLogo.gameObject.SetActive(false);
        presentsText.gameObject.SetActive(false);

        yield return FadeTo(hydroWingsLogo, 1f, fadeTime);

        yield return new WaitForSeconds(pressAnyKeyDelay);

        yield return FadeTo(pressAnyKeyText, 1f, fadeTime);

        blinkCoroutine = StartCoroutine(BlinkPressAnyKey());
    }

    private IEnumerator SkipToMainMenu()
    {
        menuOpened = true;

        if (sequenceCoroutine != null)
            StopCoroutine(sequenceCoroutine);

        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        renderEnderLogo.gameObject.SetActive(false);
        penguPixelLogo.gameObject.SetActive(false);
        presentsText.gameObject.SetActive(false);
        pressAnyKeyText.gameObject.SetActive(false);

        hydroWingsLogo.gameObject.SetActive(true);
        hydroWingsLogo.alpha = 1f;

        mainMenuButtons.gameObject.SetActive(true);
        mainMenuButtons.alpha = 0f;
        mainMenuButtons.interactable = true;
        mainMenuButtons.blocksRaycasts = true;

        yield return FadeTo(mainMenuButtons, 1f, buttonFadeTime);
    }

    public void StartGame()
    {
        if (sceneIsLoading)
            return;

        StartCoroutine(FadeOutAndLoadCharacterSelect());
    }

    private IEnumerator FadeOutAndLoadCharacterSelect()
    {
        sceneIsLoading = true;

        if (startFadeCoroutine != null)
            StopCoroutine(startFadeCoroutine);

        if (sequenceCoroutine != null)
            StopCoroutine(sequenceCoroutine);

        if (blinkCoroutine != null)
            StopCoroutine(blinkCoroutine);

        mainMenuButtons.interactable = false;
        mainMenuButtons.blocksRaycasts = false;

        fadeImage.gameObject.SetActive(true);

        float startVolume = musicSource ? musicSource.volume : 0f;
        float timer = 0f;

        StartCoroutine(FadeTo(hydroWingsLogo, 0f, sceneFadeTime));
        StartCoroutine(FadeTo(mainMenuButtons, 0f, sceneFadeTime));

        Color color = fadeImage.color;
        float currentFadeAlpha = color.a;

        while (timer < sceneFadeTime)
        {
            timer += Time.deltaTime;
            float t = timer / sceneFadeTime;

            color.a = Mathf.Lerp(currentFadeAlpha, 1f, t);
            fadeImage.color = color;

            if (musicSource)
                musicSource.volume = Mathf.Lerp(startVolume, 0f, t);

            yield return null;
        }

        color.a = 1f;
        fadeImage.color = color;

        if (musicSource)
            musicSource.volume = 0f;

        SceneManager.LoadScene(characterSelectSceneName);
    }

    private void PrepareFadeImageBlack()
    {
        if (fadeImage == null)
            return;

        fadeImage.gameObject.SetActive(true);

        Color color = fadeImage.color;
        color.a = 1f;
        fadeImage.color = color;
    }

    private IEnumerator FadeImageTo(float targetAlpha, float duration)
    {
        Color color = fadeImage.color;
        float startAlpha = color.a;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            color.a = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            fadeImage.color = color;
            yield return null;
        }

        color.a = targetAlpha;
        fadeImage.color = color;
    }

    private void HideIntroObjects()
    {
        renderEnderLogo.alpha = 0f;
        penguPixelLogo.alpha = 0f;
        presentsText.alpha = 0f;
        hydroWingsLogo.alpha = 0f;
        pressAnyKeyText.alpha = 0f;

        DisableInput(renderEnderLogo);
        DisableInput(penguPixelLogo);
        DisableInput(presentsText);
        DisableInput(hydroWingsLogo);
        DisableInput(pressAnyKeyText);
    }

    private void HideMenu()
    {
        mainMenuButtons.alpha = 0f;
        mainMenuButtons.interactable = false;
        mainMenuButtons.blocksRaycasts = false;
    }

    private IEnumerator FadeTo(CanvasGroup group, float targetAlpha, float duration)
    {
        float startAlpha = group.alpha;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            group.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            yield return null;
        }

        group.alpha = targetAlpha;
    }

    private IEnumerator BlinkPressAnyKey()
    {
        while (true)
        {
            yield return FadeTo(pressAnyKeyText, 0.25f, fadeTime);
            yield return FadeTo(pressAnyKeyText, 1f, fadeTime);
        }
    }

    private void DisableInput(CanvasGroup group)
    {
        group.interactable = false;
        group.blocksRaycasts = false;
    }
}