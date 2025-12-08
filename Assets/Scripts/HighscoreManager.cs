using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    const string HighScoreKey = "HighScore"; // The key of the highscroe PlayerPref
    public int HighScore { get; private set; } // The Highscore itself

    /// Player prefs -> a way to save a small piece of data between sessions.
    void Awake()
    {
        HighScore = PlayerPrefs.GetInt(HighScoreKey, 0); // Highscore = the value that's connected to the "Highscore" key.
                                                         // If there's no value yet (first playthrough), put a 0
    }

    public void UpdateHighscore(int newScore)
    {
        if (HighScore < newScore) // If the new score is greater than the high score
        {
            PlayerPrefs.SetInt(HighScoreKey, newScore); // Insert the new score inside the save file
            PlayerPrefs.Save(); // Save the new data
            HighScore = newScore;
        }
    }
}
