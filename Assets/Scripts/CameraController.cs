using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float speed = 2f;
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float maxCamOffsetY = 30;

    private float _currentX;
    private float _yVelocity = 0.0f;
    
    void Start()
    {
        if (player == null) return;

        _currentX = player.position.x;
        transform.position = new Vector3(_currentX, player.position.y,transform.position.z);
    }
    
    void LateUpdate()
    {
        if (player == null) return;

        _currentX += speed * Time.deltaTime;

        float targetY = player.position.y;
        float currentY = Mathf.SmoothDamp(transform.position.y, targetY, ref _yVelocity, smoothTime);
        currentY = Math.Clamp(currentY, -maxCamOffsetY, maxCamOffsetY);

        transform.position = new Vector3(_currentX, currentY, transform.position.z);
    }
}
