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
    public void SwitchToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
    public void SwitchToCurrentDifficulty()
    {
        Difficulty difficulty = (Difficulty)PlayerPrefs.GetInt("difficulty", 0);

        switch (difficulty)
        {
            case Difficulty.Easy:
                SwitchToEasyMode(); 
                break;

            case Difficulty.Normal:
                SwitchToNormalMode(); 
                break;

            case Difficulty.Hard:
                SwitchToHardMode(); 
                break;
        }
    }
    public void SwitchToNormalMode()
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
    /*
    public void QuitApplication()
    {
        Application.Quit();
    }
    */
}
