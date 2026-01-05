using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
        Difficulty difficulty = (Difficulty)PlayerPrefs.GetInt("difficulty", (int)Difficulty.Normal);

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
    private void SwitchToNormalMode()
    {
        SceneManager.LoadScene("LevelScene");
    }
    private void SwitchToHardMode()
    {
        SceneManager.LoadScene("HardMode");
    }
    private void SwitchToEasyMode()
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
