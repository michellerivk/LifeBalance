using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    /*
    private void Start()
    {
        AudioManager.instance.PlayTitleMusic(); // Play the title music
    }
    */

    public void SwitchToLevel()
    {
        SceneManager.LoadScene("LevelScene");
    }
    public void SwitchToHardMode()
    {
        SceneManager.LoadScene("HardMode");
    }
    public void SwitchToEasyMode()
    {
        SceneManager.LoadScene("EasyMode");
    }
    public void SwitchToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
