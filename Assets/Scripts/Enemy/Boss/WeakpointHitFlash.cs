using System.Collections;
using UnityEngine;

public class WeakpointHitFlash : MonoBehaviour
{
    [Header("Renderer des sichtbaren Weakpoints")]
    [SerializeField] private Renderer weakpointRenderer;

    [Header("Flash")]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.12f;
    [SerializeField] private float emissionStrength = 5f;

    private Material flashMaterial;
    private Coroutine flashCoroutine;

    private Color originalColor;
    private Color originalEmission;

    private int colorPropertyID;
    private bool hasColorProperty;
    private bool hasEmissionProperty;

    private static readonly int BaseColorID =
        Shader.PropertyToID("_BaseColor");

    private static readonly int ColorID =
        Shader.PropertyToID("_Color");

    private static readonly int EmissionColorID =
        Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        flashMaterial = weakpointRenderer.material;

        if (flashMaterial.HasProperty(BaseColorID))
        {
            colorPropertyID = BaseColorID;
            hasColorProperty = true;
        }
        else if (flashMaterial.HasProperty(ColorID))
        {
            colorPropertyID = ColorID;
            hasColorProperty = true;
        }

        if (hasColorProperty)
        {
            originalColor = flashMaterial.GetColor(colorPropertyID);
        }
        

        hasEmissionProperty =
            flashMaterial.HasProperty(EmissionColorID);

        if (hasEmissionProperty)
        {
            originalEmission =
                flashMaterial.GetColor(EmissionColorID);
        }
    }

    public void Flash()
    {
        if (!enabled || !flashMaterial)
            return;

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);

            // Vor einem neuen Flash erst sicher zurücksetzen
            RestoreMaterial();
        }

        flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        if (hasColorProperty)
        {
            flashMaterial.SetColor(colorPropertyID, flashColor);
        }

        if (hasEmissionProperty)
        {
            flashMaterial.EnableKeyword("_EMISSION");

            flashMaterial.SetColor(EmissionColorID, flashColor * emissionStrength);
        }

        yield return new WaitForSeconds(flashDuration);

        RestoreMaterial();

        flashCoroutine = null;
    }

    private void RestoreMaterial()
    {
        if (!flashMaterial)
            return;

        if (hasColorProperty)
        {
            flashMaterial.SetColor(colorPropertyID, originalColor);
        }

        if (hasEmissionProperty)
        {
            flashMaterial.SetColor(EmissionColorID, originalEmission);
        }
    }

    private void OnDisable()
    {
        RestoreMaterial();
    }
}