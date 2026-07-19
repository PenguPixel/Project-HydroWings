using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image fadeImage;

    [Header("Fade In")]
    [SerializeField] private bool fadeInOnStart = true;
    [SerializeField] private float fadeInDuration = 1f;

    [Header("Fade Out")]
    [SerializeField] private float fadeOutDuration = 1f;

    private bool _isTransitioning;

    public static SceneFader Instance { get; private set;}
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        fadeImage.gameObject.SetActive(true);
        fadeImage.raycastTarget = true;

        SetAlpha(fadeInOnStart ? 1f : 0f);
    }

    private void Start()
    {
        if (fadeImage == null)
            return;

        if (fadeInOnStart)
        {
            StartCoroutine(FadeInRoutine());
        }
        else
        {
            fadeImage.gameObject.SetActive(false);
        }
    }

    public void LoadScene(string sceneName)
    {
        if (_isTransitioning)
            return;

        StartCoroutine(FadeOutAndLoadScene(sceneName));
    }
    
    public void LoadScene(int sceneIndex)
    {
        if (_isTransitioning)
            return;
        Debug.Log(sceneIndex);

        IEnumerator coroutine = FadeOutAndLoadScene(sceneIndex);
        StartCoroutine(coroutine);
    }

    public void ReloadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator FadeInRoutine()
    {
        _isTransitioning = true;

        yield return FadeTo(0f, fadeInDuration);

        fadeImage.raycastTarget = false;
        fadeImage.gameObject.SetActive(false);

        _isTransitioning = false;
    }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        _isTransitioning = true;

        fadeImage.gameObject.SetActive(true);
        fadeImage.raycastTarget = true;

        yield return FadeTo(1f, fadeOutDuration);

        SceneManager.LoadScene(sceneName);
    }
    
    private IEnumerator FadeOutAndLoadScene(int sceneIndex)
    {
        _isTransitioning = true;

        Debug.Log("Coroutine funzt");
        fadeImage.gameObject.SetActive(true);
        fadeImage.raycastTarget = true;

        yield return FadeTo(1f, fadeOutDuration);

        SceneManager.LoadScene(sceneIndex);
    }

    private IEnumerator FadeTo(float targetAlpha, float duration)
    {
        Color color = fadeImage.color;
        float startAlpha = color.a;
        float timer = 0f;

        if (duration <= 0f)
        {
            SetAlpha(targetAlpha);
            yield break;
        }

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t = Mathf.Clamp01(timer / duration);
            t = Mathf.SmoothStep(0f, 1f, t);

            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = color;

            yield return null;
        }

        SetAlpha(targetAlpha);
    }

    private void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}