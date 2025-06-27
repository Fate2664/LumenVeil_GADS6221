using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        AudioManager.Instance?.PlaySFX("GameplayMusic");
        AudioManager.Instance?.StopSFX("MenuMusic");
        SceneManager.LoadScene("Level 1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void LoadSettingMenu()
    {
        SceneManager.LoadScene("Settings");
    }
}
