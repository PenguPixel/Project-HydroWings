using UnityEngine;
using static UnityEngine.Vector3;

public class PlayerController : MonoBehaviour
{
    private Rigidbody _rigidbody;

    [SerializeField] public float MovementSpeed = 30f;
    [SerializeField] private float acceleration = 10f;

    [SerializeField] private float RotationSpeed = 50f;
    [SerializeField] private float ReturnSpeed = 60f;
    [SerializeField] private float MaxRotationAngle = 25f;
    [SerializeField] private float MaxCharacterOffsetY = 10f;
    [SerializeField] private float MinCharacterOffsetY = -10f;

    private float _rotationX = 0f;
    private float _camSpeed;
    private Vector2 _currentMovementInput;

    private bool _isTouchingObstacle;
    private bool _isTouchingBackwall;

    private ContactPoint[] _contactPoints =
        new ContactPoint[4];

    private Character _character;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();

        _rigidbody.useGravity = false;
        _rigidbody.isKinematic = false;

        _rigidbody.constraints =
            RigidbodyConstraints.FreezeRotation |
            RigidbodyConstraints.FreezePositionZ;

        _rigidbody.WakeUp();

        CameraController.MoveAction.AddListener(SetCamSpeed);
    }

    private void Start()
    {
        _character = GetComponentInChildren<Character>();

        if (_character != null &&
            _character.Stats != null)
        {
            MovementSpeed =
                _character.Stats.MovementSpeed;

            Debug.Log(
                $"MovementSpeed übernommen: {MovementSpeed}"
            );
        }
        else
        {
            Debug.LogWarning(
                "PlayerController: Kein aktiver Character " +
                "mit CharacterStats gefunden."
            );
        }
    }

    private void SetCamSpeed(float cameraSpeedValue)
    {
        _camSpeed = cameraSpeedValue;
    }

    public void SetMovementInput(Vector2 input)
    {
        _currentMovementInput = input;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.TryGetComponent<Obstacle>(
                out Obstacle obstacle))
        {
            int contactCount =
                collision.GetContacts(_contactPoints);

            for (int i = 0; i < contactCount; i++)
            {
                Vector3 normal =
                    _contactPoints[i].normal;

                Vector3 localNormal =
                    transform.InverseTransformDirection(normal);

                if (localNormal.x < -0.5f)
                {
                    _isTouchingObstacle = true;

                    Debug.Log(
                        "Touching Obstacle at the front"
                    );
                }
            }
        }

        if (collision.collider.TryGetComponent<Backwall>(
                out Backwall backwall))
        {
            _isTouchingBackwall = true;
            Debug.Log("Touching Backwall");
        }

        if (_isTouchingObstacle && _isTouchingBackwall)
        {
            if (_character == null)
            {
                _character =
                    GetComponentInChildren<Character>();
            }

            if (_character != null &&
                _character.Stats != null)
            {
                _character.TakeDamage(
                    _character.Stats.MaxHealth
                );
            }
        }
    }

    private void FixedUpdate()
    {
        Vector3 inputDirection =
            up * _currentMovementInput.y +
            right * _currentMovementInput.x;

        Vector3 targetVelocity =
            inputDirection * MovementSpeed;

        Vector3 currentPosition =
            _rigidbody.position;

        if (currentPosition.y >= MaxCharacterOffsetY &&
            targetVelocity.y > 0f)
        {
            targetVelocity.y = 0f;
        }

        if (currentPosition.y <= MinCharacterOffsetY &&
            targetVelocity.y < 0f)
        {
            targetVelocity.y = 0f;
        }

        Vector3 currentVelocity =
            _rigidbody.linearVelocity;

        float smoothedVelocityX =
            Mathf.MoveTowards(
                currentVelocity.x,
                targetVelocity.x,
                acceleration * Time.fixedDeltaTime
            );

        float smoothedVelocityY =
            Mathf.MoveTowards(
                currentVelocity.y,
                targetVelocity.y,
                acceleration * Time.fixedDeltaTime
            );

        _rigidbody.linearVelocity =
            new Vector3(
                smoothedVelocityX,
                smoothedVelocityY,
                0f
            );

        currentPosition.x +=
            _camSpeed * Time.fixedDeltaTime;

        currentPosition.y =
            Mathf.Clamp(
                currentPosition.y, MinCharacterOffsetY, MaxCharacterOffsetY);

        currentPosition.z = 0f;

        _rigidbody.MovePosition(currentPosition);

        _isTouchingObstacle = false;
        _isTouchingBackwall = false;
    }

    public void Rotate(float verticalInput, Transform visualTransform)
    {
        if (!visualTransform)
        {
            return;
        }

        float currentY = _rigidbody.position.y;

        bool hittingUpperBoundary = currentY >= MaxCharacterOffsetY && verticalInput > 0f;

        bool hittingLowerBoundary = currentY <= MinCharacterOffsetY && verticalInput < 0f;

        if (Mathf.Abs(verticalInput) > 0.01f && !hittingUpperBoundary && !hittingLowerBoundary)
        {
            _rotationX -= verticalInput * RotationSpeed * Time.fixedDeltaTime;
        }
        else
        {
            _rotationX = Mathf.MoveTowards(_rotationX, 0f, ReturnSpeed * Time.deltaTime);
        }

        _rotationX = Mathf.Clamp(_rotationX, -MaxRotationAngle, MaxRotationAngle);

        visualTransform.localRotation =
            Quaternion.Euler(_rotationX, 90f, 0f);
    }
}