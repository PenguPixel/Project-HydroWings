using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] private GameObject mainMenuButtons;
    [SerializeField] private GameObject settingsWindow;

    private void Start()
    {
        mainMenuButtons.SetActive(true);
        settingsWindow.SetActive(false);
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
}