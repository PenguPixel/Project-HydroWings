using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterPreviewHover : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] private PlayableWing wingType;
    [SerializeField] private Transform model;
    [SerializeField] private Sprite statsSprite;

    [Header("Scene")]
    [SerializeField] private string levelSceneName = "Level_01Scene";

    [Header("Hover")]
    [SerializeField] private float hoverHeight = 0.15f;
    [SerializeField] private float hoverSpeed = 1f;

    [Header("Rotation")]
    [SerializeField] private float hoverRotationSpeed = 40f;
    [SerializeField] private float selectionRotationSpeed = 600f;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip selectSound;

    private Vector3 _startPosition;
    private float _hoverPhaseOffset;

    private bool _isHovered;
    private bool _wasHovered;
    private bool _isSelected;

    private Camera _mainCamera;

    // Diese Sperre gilt gemeinsam für alle CharacterPreviewHover-Objekte.
    private static bool _characterSelectionLocked;

    private void Awake()
    {
        // Beim erneuten Laden der Character-Select-Szene zurücksetzen.
        _characterSelectionLocked = false;
    }

    private void Start()
    {
        if (model == null)
        {
            model = transform;
        }

        _mainCamera = Camera.main;

        _startPosition = model.localPosition;
        _hoverPhaseOffset = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        HoverAnimation();

        if (!_isSelected && !_characterSelectionLocked)
        {
            CheckMouseHover();
            UpdateStatsImage();
            CheckSelectionInput();
        }

        RotateModel();
    }

    private void HoverAnimation()
    {
        float hoverOffset =
            Mathf.Sin(Time.time * hoverSpeed + _hoverPhaseOffset)
            * hoverHeight;

        model.localPosition =
            _startPosition + Vector3.up * hoverOffset;
    }

    private void CheckMouseHover()
    {
        _isHovered = false;

        if (!_mainCamera || Mouse.current == null)
        {
            return;
        }

        Vector2 mousePosition = Mouse.current.position.ReadValue();
        Ray ray = _mainCamera.ScreenPointToRay(mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
        {
            return;
        }

        if (hit.transform == transform ||
            hit.transform.IsChildOf(transform))
        {
            _isHovered = true;
        }
    }

    private void RotateModel()
    {
        if (_isSelected)
        {
            model.Rotate(
                Vector3.up,
                selectionRotationSpeed * Time.deltaTime,
                Space.Self
            );

            return;
        }

        if (_isHovered && !_characterSelectionLocked)
        {
            model.Rotate(
                Vector3.up,
                hoverRotationSpeed * Time.deltaTime,
                Space.Self
            );
        }
    }

    private void UpdateStatsImage()
    {
        if (_isHovered && !_wasHovered)
        {
            CharacterStatsImage.Instance?.Show(statsSprite);
        }

        if (!_isHovered && _wasHovered)
        {
            CharacterStatsImage.Instance?.Hide();
        }

        _wasHovered = _isHovered;
    }

    private void CheckSelectionInput()
    {
        if (_characterSelectionLocked ||
            !_isHovered ||
            Mouse.current == null ||
            !Mouse.current.leftButton.wasPressedThisFrame)
        {
            return;
        }

        SelectCharacter();
    }

    private void SelectCharacter()
    {
        if (_characterSelectionLocked)
        {
            return;
        }

        // Sofort sperren, bevor Sound, Animation oder Szenenwechsel starten.
        _characterSelectionLocked = true;
        _isSelected = true;

        CharacterStatsImage.Instance?.Hide();

        CharacterSelection.SelectWing(wingType);

        if (audioSource && selectSound)
        {
            audioSource.PlayOneShot(
                selectSound,
                SFXVolumeManager.Volume
            );
        }

        Debug.Log($"{wingType} wurde ausgewählt.");

        SceneFader.Instance.LoadScene(levelSceneName);
    }
}