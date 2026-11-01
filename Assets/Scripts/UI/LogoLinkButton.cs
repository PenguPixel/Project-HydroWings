
using UnityEngine;
using UnityEngine.EventSystems;

public class LogoLinkButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private string url;
    [SerializeField] private float hoverScale = 1.15f;
    [SerializeField] private float animationSpeed = 10f;

    private Vector3 originalScale;
    private Vector3 targetScale;

    private void Awake()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.unscaledDeltaTime * animationSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && !string.IsNullOrWhiteSpace(url))
        {
            Application.OpenURL(url);
        }
    }

    private void OnDisable()
    {
        transform.localScale = originalScale;
        targetScale = originalScale;
    }
}
