using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradePreviewHover : MonoBehaviour
{
    [Header("Model")]
    [SerializeField] private Transform model;

    [Header("Scene")]
    [SerializeField] private string levelSceneName = "Level_02Scene";
    [SerializeField] private float loadDelay = 0.7f;

    [Header("Hover")]
    [SerializeField] private float hoverHeight = 0.15f;
    [SerializeField] private float hoverSpeed = 1f;

    [Header("Rotation")]
    [SerializeField] private float hoverRotationSpeed = 40f;
    [SerializeField] private float selectionRotationSpeed = 600f;

    private Camera _mainCamera;
    private Vector3 _startLocalPosition;
    private float _hoverPhaseOffset;

    private bool _isHovered;
    private bool _isSelected;

    private static bool _selectionLocked;

    private void Awake()
    {
        _selectionLocked = false;
    }

    private void Start()
    {
        _mainCamera = Camera.main;

        if (model == null)
        {
            Debug.LogError(
                $"UpgradePreviewHover auf {name}: Model wurde nicht zugewiesen."
            );

            enabled = false;
            return;
        }

        _startLocalPosition = model.localPosition;

        _hoverPhaseOffset =
            Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        AnimateHover();

        if (!_selectionLocked)
        {
            CheckMouseHover();
            CheckSelection();
        }

        RotateModel();
    }

    private void AnimateHover()
    {
        float hoverOffset =
            Mathf.Sin(
                Time.time * hoverSpeed +
                _hoverPhaseOffset
            ) * hoverHeight;

        model.localPosition =
            _startLocalPosition +
            Vector3.up * hoverOffset;
    }

    private void CheckMouseHover()
    {
        _isHovered = false;

        if (_mainCamera == null ||
            Mouse.current == null)
        {
            return;
        }

        Vector2 mousePosition =
            Mouse.current.position.ReadValue();

        Ray ray =
            _mainCamera.ScreenPointToRay(
                mousePosition
            );

        if (Physics.Raycast(
                ray,
                out RaycastHit hit))
        {
            if (hit.transform == transform ||
                hit.transform.IsChildOf(transform))
            {
                _isHovered = true;
            }
        }
    }

    private void CheckSelection()
    {
        if (!_isHovered ||
            Mouse.current == null ||
            !Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }

        _selectionLocked = true;
        _isSelected = true;

        Debug.Log($"{name} wurde ausgewählt.");

        StartCoroutine(LoadLevelAfterDelay());
    }

    private IEnumerator LoadLevelAfterDelay()
    {
        yield return new WaitForSeconds(loadDelay);

        if (SceneFader.Instance != null)
        {
            SceneFader.Instance.LoadScene(levelSceneName);
        }
        else
        {
            Debug.LogError(
                "Kein SceneFader in der UpgradeScene gefunden."
            );
        }
    }

    private void RotateModel()
    {
        if (_isSelected)
        {
            model.Rotate(
                Vector3.up,
                selectionRotationSpeed *
                Time.deltaTime,
                Space.Self
            );

            return;
        }

        if (_isHovered)
        {
            model.Rotate(
                Vector3.up,
                hoverRotationSpeed *
                Time.deltaTime,
                Space.Self
            );
        }
    }
}