using System;
using UnityEngine;

public class WaterSurfaceController : MonoBehaviour
{
    [SerializeField] private float movementMultiplier = 1.2f;
    private float _cameraSpeed;
    private Vector3 _currentPosition;
    private void OnEnable()
    {
        CameraController.MoveAction.AddListener(SetCameraSpeed);
    }

    private void OnDisable()
    {
        CameraController.MoveAction.RemoveListener(SetCameraSpeed);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        _currentPosition.x -= _cameraSpeed * movementMultiplier * Time.deltaTime; 
        transform.position = _currentPosition;
    }
    
    private void SetCameraSpeed(float camSpeed)
    {
        _cameraSpeed = camSpeed;
    }
    
    
}
