using UnityEngine;

public class BackwallMovementController : MonoBehaviour
{
    private float _currentX;

    private float _currentCamSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Awake()
    {
        CameraController.MoveAction.AddListener(SetCamMoveSpeed);
    }

    private void SetCamMoveSpeed(float camSpeed)
    {
        _currentCamSpeed = camSpeed;
    }

    void Start()
    {
        _currentX = transform.position.x;
        transform.position = new Vector3(_currentX, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        _currentX += _currentCamSpeed * Time.deltaTime;
        transform.position = new Vector3(_currentX, transform.position.y, transform.position.z);
    }
}
