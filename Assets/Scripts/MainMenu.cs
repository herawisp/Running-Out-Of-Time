using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [Header("Panels")]
    [SerializeField] private GameObject settingsPanel;

    public void PlayGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void OpenSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Settings Panel reference is missing in the Inspector!");
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}