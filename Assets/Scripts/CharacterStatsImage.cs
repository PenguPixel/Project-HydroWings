using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(CanvasGroup))]
public class CharacterStatsImage : MonoBehaviour
{
    public static CharacterStatsImage Instance { get; private set; }

    [Header("Animation")]
    [SerializeField] private float animationDuration = 0.25f;
    [SerializeField] private float slideDistance = 80f;

    private Image _image;
    private CanvasGroup _canvasGroup;
    private RectTransform _rectTransform;

    private Vector2 _defaultPosition;
    private Coroutine _animationCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        _image = GetComponent<Image>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform = GetComponent<RectTransform>();

        _defaultPosition = _rectTransform.anchoredPosition;

        _canvasGroup.alpha = 0f;
        _image.enabled = false;
    }

    public void Show(Sprite sprite)
    {
        if (!sprite)
            return;

        if (_animationCoroutine != null)
            StopCoroutine(_animationCoroutine);

        _animationCoroutine = StartCoroutine(ShowRoutine(sprite));
    }

    public void Hide()
    {
        if (_animationCoroutine != null)
            StopCoroutine(_animationCoroutine);

        _animationCoroutine = StartCoroutine(HideRoutine());
    }

    private IEnumerator ShowRoutine(Sprite sprite)
    {
        _image.enabled = true;
        _image.sprite = sprite;

        Vector2 startPos = _defaultPosition + Vector2.right * slideDistance;

        _rectTransform.anchoredPosition = startPos;
        _canvasGroup.alpha = 0f;

        float timer = 0f;

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.SmoothStep(0f, 1f, timer / animationDuration);

            _rectTransform.anchoredPosition =
                Vector2.Lerp(startPos, _defaultPosition, t);

            _canvasGroup.alpha = t;

            yield return null;
        }

        _rectTransform.anchoredPosition = _defaultPosition;
        _canvasGroup.alpha = 1f;

        _animationCoroutine = null;
    }

    private IEnumerator HideRoutine()
    {
        Vector2 startPos = _rectTransform.anchoredPosition;
        Vector2 endPos = _defaultPosition + Vector2.left * slideDistance;

        float startAlpha = _canvasGroup.alpha;

        float timer = 0f;

        while (timer < animationDuration)
        {
            timer += Time.deltaTime;

            float t = Mathf.SmoothStep(0f, 1f, timer / animationDuration);

            _rectTransform.anchoredPosition =
                Vector2.Lerp(startPos, endPos, t);

            _canvasGroup.alpha =
                Mathf.Lerp(startAlpha, 0f, t);

            yield return null;
        }

        _canvasGroup.alpha = 0f;
        _rectTransform.anchoredPosition = endPos;

        _image.enabled = false;
        _image.sprite = null;

        _animationCoroutine = null;
    }
}