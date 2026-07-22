using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    public static readonly UnityEvent<float> MoveAction = new();
    public static UnityEvent<int> ReachEndOfLevel = new UnityEvent<int>();
    [SerializeField] public CameraStats stats;
    [SerializeField] private Transform player;

    [SerializeField] private bool stopForBossFight;
    [SerializeField] private float timeUntilStop = 1f;
    [SerializeField] private float endOfLevelX = 720f;

    private float _currentCamSpeed;
    private float _currentX;
    private float _yVelocity = 0.0f;

    private float _stopTimer;
    
    
    void Start()
    {
        if (player == null) return;
        _currentCamSpeed = stats.speed;
        MoveAction.Invoke(_currentCamSpeed);

        _currentX = transform.position.x;
        transform.position = new Vector3(_currentX, player.position.y,transform.position.z);
        
        
    }
    
    void LateUpdate()
    {
        if (!player) return;

        float targetY = player.position.y;
        float currentY = Mathf.SmoothDamp(transform.position.y, targetY, ref _yVelocity, stats.smoothTime);
        currentY = Math.Clamp(currentY, -stats.maxCamOffsetY, stats.maxCamOffsetY);

        transform.position = new Vector3(_currentX, currentY, transform.position.z);
    }

    private void FixedUpdate()
    {
        if (stopForBossFight)
        {
            _stopTimer += Time.fixedDeltaTime;

            if (_stopTimer >= timeUntilStop)
            {
                _currentCamSpeed = 0f;
                MoveAction.Invoke(0f);
            }
        }

        _currentX += _currentCamSpeed * Time.fixedDeltaTime;

        if (_currentX >= endOfLevelX)
        {
            float targetCamSpeed = 0f;
            float lerpTime = 2f;
            _currentCamSpeed = Mathf.Lerp(_currentCamSpeed, targetCamSpeed, lerpTime);
            int currentScene = SceneManager.GetActiveScene().buildIndex;
            Debug.Log("Aktueller Szenen Index von Kamera" + currentScene);
            ReachEndOfLevel.Invoke(currentScene);
        }
    }
}