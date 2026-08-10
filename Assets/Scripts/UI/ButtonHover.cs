using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerClickHandler,
    ISelectHandler,
    IDeselectHandler
{
    [SerializeField] private float hoverScale = 1.1f;
    [SerializeField] private float speed = 10f;

    private Vector3 normalScale;
    private Vector3 targetScale;

    private void Awake()
    {
        normalScale = transform.localScale;
        targetScale = normalScale;
    }

    private void OnEnable()
    {
        ResetScale();
    }

    private void OnDisable()
    {
        ResetScale();
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * speed
        );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = normalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = normalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ResetScale();
    }

    private void ResetScale()
    {
        targetScale = normalScale;
        transform.localScale = normalScale;
    }

    public void OnSelect(BaseEventData eventData)
    {
        targetScale = normalScale * hoverScale;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        targetScale = normalScale;
    }
}