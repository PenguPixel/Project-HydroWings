using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Scene Management")]
    [SerializeField] private SceneFader sceneFader;
    [SerializeField] private LevelFlowData levelFlowData;
    
    public static UnityEvent<int> OnUpgradescreenLoad = new UnityEvent<int>();
    public static GameManager Instance { get; private set; }
    public static float GlobalDifficultiyMultiplier { get; private set; } = 1f;

    private void OnEnable()
    {
        CameraController.ReachEndOfLevel.AddListener(LoadUpgradeScreen);
        UpgradeSceneController.OnLoadNextLevel.AddListener(LoadNextLevel);
    }

    private void OnDisable()
    {
        CameraController.ReachEndOfLevel.RemoveListener(LoadUpgradeScreen);
        UpgradeSceneController.OnLoadNextLevel.RemoveListener(LoadNextLevel);
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        
        if (SceneManager.GetActiveScene().name == "Level_01Scene") GlobalDifficultiyMultiplier = 1f;
        if (SceneManager.GetActiveScene().name == "Level_02Scene") GlobalDifficultiyMultiplier = 1.5f;
        if (SceneManager.GetActiveScene().name == "BossLevelScene") GlobalDifficultiyMultiplier = 1f;
    }
    

   public void LoadTitleScreen()
    {
        Time.timeScale = 1.0f;
        Debug.Log("Button funktioniert");
        sceneFader.LoadScene((int) SceneName.TitleScene);
    }

    public void ReloadLevelScene()
    {
        Time.timeScale = 1.0f;
        sceneFader.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadUpgradeScreen(int originSceneIndex)
    {
        Time.timeScale = 1f;
        levelFlowData.currentLevelIndex = originSceneIndex;
        sceneFader.LoadScene((int) SceneName.UpgradeScene);  
    }

    public void LoadNextLevel(int nextSceneIndex)
    {
        Time.timeScale = 1.0f;
        sceneFader.LoadScene(nextSceneIndex);
    }
}

enum SceneName
{
    TitleScene = 0,
    CharacterSelectScene = 1,
    UpgradeScene = 2,
    Level_01Scene = 3,
    Level_02Scene = 4,
    BossLevelScene = 5,
    ScoreboardScene = 6
}

