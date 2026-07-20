using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SceneFader _sceneFader;
    
    public static GameManager Instance { get; private set; }
    public static float GlobalDifficultiyMultiplier { get; private set; } = 1f;

    private void OnEnable()
    {
        CameraController.ReachEndOfLevel.AddListener(LoadUpgradeScreen);
    }

    private void OnDisable()
    {
        CameraController.ReachEndOfLevel.RemoveListener(LoadUpgradeScreen);
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
        Debug.Log(_sceneFader);
    }
    

   public void LoadTitleScreen()
    {
        Time.timeScale = 1.0f;
        Debug.Log("Button funktioniert");
        _sceneFader.LoadScene((int) SceneName.TitleScene);
    }

    public void ReloadLevelScene()
    {
        Time.timeScale = 1.0f;
        _sceneFader.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadUpgradeScreen()
    {
        Time.timeScale = 1f;
        _sceneFader.LoadScene((int) SceneName.TitleScene);  //TODO add new Scene to enum and change here
    }
}

enum SceneName
{
    TitleScene = 0,
    CharacterSelectScene = 1,
    SampleScene = 2,
    Level01Scene = 3,
    Level02Scene = 4,
    BossLevelScene = 5
}

