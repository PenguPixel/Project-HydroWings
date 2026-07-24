using System.Collections;
using UnityEngine;

public class PlayerHitFlash : MonoBehaviour
{
    [Header("Flash")]
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashDuration = 0.12f;
    [SerializeField] private float emissionStrength = 20f;

    private Renderer[] _renderers;
    private Material[][] _materials;

    private Color[][] _originalBaseColors;
    private Color[][] _originalEmissionColors;

    private Coroutine _flashCoroutine;

    private static readonly int BaseColorID =
        Shader.PropertyToID("_BaseColor");

    private static readonly int ColorID =
        Shader.PropertyToID("_Color");

    private static readonly int EmissionColorID =
        Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        // Findet alle sichtbaren Renderer auf diesem Objekt
        // und auf allen Kindobjekten.
        _renderers = GetComponentsInChildren<Renderer>(true);

        _materials = new Material[_renderers.Length][];

        _originalBaseColors = new Color[_renderers.Length][];

        _originalEmissionColors = new Color[_renderers.Length][];

        for (int rendererIndex = 0;
             rendererIndex < _renderers.Length;
             rendererIndex++)
        {
            _materials[rendererIndex] = _renderers[rendererIndex].materials;
            
            int materialCount = _materials[rendererIndex].Length;
            
            _originalBaseColors[rendererIndex] = new Color[materialCount];

            _originalEmissionColors[rendererIndex] = new Color[materialCount];

            for (int materialIndex = 0;
                 materialIndex < materialCount;
                 materialIndex++)
            {
                Material material = _materials[rendererIndex][materialIndex];

                if (!material)
                    continue;

                if (material.HasProperty(BaseColorID))
                {
                    _originalBaseColors[rendererIndex][materialIndex] = material.GetColor(BaseColorID);
                }
                else if (material.HasProperty(ColorID))
                {
                    _originalBaseColors[rendererIndex][materialIndex] = material.GetColor(ColorID);
                }

                if (material.HasProperty(EmissionColorID))
                {
                    _originalEmissionColors[rendererIndex][materialIndex] = material.GetColor(EmissionColorID);
                }
            }
        }
    }

    public void Flash()
    {
        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
            RestoreMaterials();
        }

        _flashCoroutine =
            StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        ApplyFlashColor();

        yield return new WaitForSeconds(flashDuration);

        RestoreMaterials();

        _flashCoroutine = null;
    }

    private void ApplyFlashColor()
    {
        for (int rendererIndex = 0;
             rendererIndex < _materials.Length;
             rendererIndex++)
        {
            for (int materialIndex = 0;
                 materialIndex < _materials[rendererIndex].Length;
                 materialIndex++)
            {
                Material material = _materials[rendererIndex][materialIndex];

                if (!material)
                    continue;

                if (material.HasProperty(BaseColorID))
                {
                    material.SetColor(BaseColorID, flashColor);
                }
                else if (material.HasProperty(ColorID))
                {
                    material.SetColor(ColorID, flashColor);
                }

                if (material.HasProperty(EmissionColorID))
                {
                    material.EnableKeyword("_EMISSION");

                    material.SetColor(EmissionColorID, flashColor * emissionStrength);
                }
            }
        }
    }

    private void RestoreMaterials()
    {
        if (_materials == null)
            return;

        for (int rendererIndex = 0;
             rendererIndex < _materials.Length;
             rendererIndex++)
        {
            for (int materialIndex = 0;
                 materialIndex < _materials[rendererIndex].Length;
                 materialIndex++)
            {
                Material material = _materials[rendererIndex][materialIndex];

                if (!material)
                    continue;

                if (material.HasProperty(BaseColorID))
                {
                    material.SetColor(BaseColorID, _originalBaseColors[rendererIndex][materialIndex]);
                }
                else if (material.HasProperty(ColorID))
                {
                    material.SetColor(ColorID, _originalBaseColors[rendererIndex][materialIndex]);
                }

                if (material.HasProperty(EmissionColorID))
                {
                    material.SetColor(EmissionColorID, _originalEmissionColors[rendererIndex][materialIndex]);
                }
            }
        }
    }

    private void OnDisable()
    {
        RestoreMaterials();
    }
}