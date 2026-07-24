using System.Collections;
using UnityEngine;

public class EnemyHitFlash : MonoBehaviour
{
    [Header("Flash Settings")]
    [SerializeField] private Color flashColor = Color.white;
    [SerializeField] private float flashDuration = 0.1f;
    [SerializeField] private float emissionStrength = 8f;

    private Renderer[] _renderers;
    private Material[][] _materials;

    private Color[][] _originalBaseColors;
    private Color[][] _originalEmissionColors;

    private bool[][] _hasBaseColor;
    private bool[][] _hasColor;
    private bool[][] _hasEmission;

    private Coroutine _flashCoroutine;

    private static readonly int BaseColorID =
        Shader.PropertyToID("_BaseColor");

    private static readonly int ColorID =
        Shader.PropertyToID("_Color");

    private static readonly int EmissionColorID =
        Shader.PropertyToID("_EmissionColor");

    private void Awake()
    {
        FindRenderers();
        SaveOriginalMaterialValues();
    }

    private void FindRenderers()
    {
        _renderers = GetComponentsInChildren<Renderer>(true);

        if (_renderers.Length == 0)
        {
            Debug.LogWarning(
                $"EnemyHitFlash auf {gameObject.name}: " +
                "Keine Renderer in den Kindobjekten gefunden."
            );
        }
    }

    private void SaveOriginalMaterialValues()
    {
        _materials = new Material[_renderers.Length][];

        _originalBaseColors = new Color[_renderers.Length][];

        _originalEmissionColors = new Color[_renderers.Length][];

        _hasBaseColor = new bool[_renderers.Length][];

        _hasColor = new bool[_renderers.Length][];

        _hasEmission = new bool[_renderers.Length][];

        for (int rendererIndex = 0;
             rendererIndex < _renderers.Length;
             rendererIndex++)
        {
            Renderer currentRenderer = _renderers[rendererIndex];

            // Erstellt eigene Materialinstanzen für diesen Gegner.
            _materials[rendererIndex] = currentRenderer.materials;

            int materialCount = _materials[rendererIndex].Length;

            _originalBaseColors[rendererIndex] = new Color[materialCount];

            _originalEmissionColors[rendererIndex] = new Color[materialCount];

            _hasBaseColor[rendererIndex] = new bool[materialCount];

            _hasColor[rendererIndex] = new bool[materialCount];

            _hasEmission[rendererIndex] = new bool[materialCount];

            for (int materialIndex = 0;
                 materialIndex < materialCount;
                 materialIndex++)
            {
                Material material = _materials[rendererIndex][materialIndex];

                if (material == null)
                {
                    continue;
                }

                if (material.HasProperty(BaseColorID))
                {
                    _hasBaseColor[rendererIndex][materialIndex] = true;

                    _originalBaseColors[rendererIndex][materialIndex] = material.GetColor(BaseColorID);
                }
                else if (material.HasProperty(ColorID))
                {
                    _hasColor[rendererIndex][materialIndex] = true;

                    _originalBaseColors[rendererIndex][materialIndex] = material.GetColor(ColorID);
                }

                if (material.HasProperty(EmissionColorID))
                {
                    _hasEmission[rendererIndex][materialIndex] = true;

                    _originalEmissionColors[rendererIndex][materialIndex] = material.GetColor(EmissionColorID);
                }
            }
        }
    }

    public void Flash()
    {
        if (_materials == null ||
            _materials.Length == 0)
        {
            return;
        }

        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
        }

        RestoreMaterials();

        _flashCoroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        ApplyFlash();

        yield return new WaitForSeconds(flashDuration);

        RestoreMaterials();

        _flashCoroutine = null;
    }

    private void ApplyFlash()
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
                {
                    continue;
                }

                if (_hasBaseColor[rendererIndex][materialIndex])
                {
                    material.SetColor(BaseColorID, flashColor);
                }
                else if (_hasColor[rendererIndex][materialIndex])
                {
                    material.SetColor(ColorID, flashColor);
                }

                if (_hasEmission[rendererIndex][materialIndex])
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
        {
            return;
        }

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
                {
                    continue;
                }

                if (_hasBaseColor[rendererIndex][materialIndex])
                {
                    material.SetColor(BaseColorID, _originalBaseColors[rendererIndex][materialIndex]);
                }
                else if (_hasColor[rendererIndex][materialIndex])
                {
                    material.SetColor(ColorID, _originalBaseColors[rendererIndex][materialIndex]);
                }

                if (_hasEmission[rendererIndex][materialIndex])
                {
                    material.SetColor(EmissionColorID, _originalEmissionColors[rendererIndex][materialIndex]);
                }
            }
        }
    }

    private void OnDisable()
    {
        if (_flashCoroutine != null)
        {
            StopCoroutine(_flashCoroutine);
            _flashCoroutine = null;
        }

        RestoreMaterials();
    }
}