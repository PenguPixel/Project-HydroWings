using System;
using UnityEngine;
using UnityEngine.Events;

public class UpgradeSceneHandler : MonoBehaviour
{
    [SerializeField] private int testIndexForDebug;
    public static UnityEvent<int> OnLoadNextLevel = new UnityEvent<int>();

    private int _lastOriginSceneIndex;

    private void OnEnable()
    {
        GameManager.OnUpgradescreenLoad.AddListener(SetOriginSceneIndex);
    }

    private void OnDisable()
    {
        GameManager.OnUpgradescreenLoad.RemoveListener(SetOriginSceneIndex);
    }

    private void Awake()
    {
        _lastOriginSceneIndex = testIndexForDebug;
    }

    private void SetOriginSceneIndex(int originSceneIndex)
    {
        _lastOriginSceneIndex = originSceneIndex;
    }

    public void LoadNextLevel()
    {
        int nextSceneIndex = _lastOriginSceneIndex++;
        OnLoadNextLevel.Invoke(nextSceneIndex);
    }
}
