using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraStats stats;
    [SerializeField] private Transform player;

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

        _currentX += stats.speed * Time.deltaTime;

        float targetY = player.position.y;
        float currentY = Mathf.SmoothDamp(transform.position.y, targetY, ref _yVelocity, stats.smoothTime);
        currentY = Math.Clamp(currentY, -stats.maxCamOffsetY, stats.maxCamOffsetY);

        transform.position = new Vector3(_currentX, currentY, transform.position.z);
    }
}
