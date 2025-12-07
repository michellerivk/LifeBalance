using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    /* For later
    private void Start()
    {
        AudioManager.instance.PlayTitle(); // Play the title music
    }
    */
    public void SwitchToFirstLevel() 
    {
        SceneManager.LoadScene("FirstLevelScene");
    }
    public void SwitchToSecondLevel()
    {
        SceneManager.LoadScene("SecondLevelScene");
    }
    public void SwitchToThirdLevel()
    {
        SceneManager.LoadScene("ThirdLevelScene");
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
