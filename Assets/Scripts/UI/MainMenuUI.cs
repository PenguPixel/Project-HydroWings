using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject mainMenuButtons;
    [SerializeField] private GameObject settingsWindow;
    [SerializeField] private GameObject creditsWindow;  

    private void Start()
    {
        mainMenuButtons.SetActive(true);
        settingsWindow.SetActive(false);
        creditsWindow.SetActive(false);
    }

    public void OpenSettings()
    {
        mainMenuButtons.SetActive(false);
        settingsWindow.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsWindow.SetActive(false);
        mainMenuButtons.SetActive(true);
    }

    public void OpenCredits()
    {
        creditsWindow.SetActive(true);
        mainMenuButtons.SetActive(false);
    }

    public void CloseCredits()
    {
        creditsWindow.SetActive(false);
        mainMenuButtons.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
        
        if (UnityEditor.EditorApplication.isPlaying) UnityEditor.EditorApplication.isPlaying = false;
    }
}