using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private SceneFader _sceneFader;
    
    public static GameManager Instance { get; private set; }
    

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
        Debug.Log(_sceneFader);
    }

   public void LoadTitleScreen()
    {
        _sceneFader.LoadScene((int) SceneName.TitleScene);
    }

    public void ReloadLevelScene()
    {
        _sceneFader.LoadScene(SceneManager.GetActiveScene().name);
    }
}

enum SceneName
{
    TitleScene = 0,
    CharacterSelectScene = 1,
    Level01Scene = 3,
    BossLevelScene = 4
}

