using UnityEngine;

public static class HighscoreManager
{
    const string HighScoreKey = "HighScore"; // The key of the highscore PlayerPref

    // Player prefs -> a way to save a small piece of data between sessions.
    public static int GetHighScore()
    {
        // Return the value that's connected to the "Highscore" key.
        // If there's no value yet (first playthrough), return a 0
        return PlayerPrefs.GetInt(HighScoreKey, 0); 
    }

    public static void UpdateHighscore(int newScore)
    {
        int currentHighscore = GetHighScore();
        if (currentHighscore < newScore) // If the new score is greater than the current highscore
        {
            PlayerPrefs.SetInt(HighScoreKey, newScore); // Insert the new score inside the save file
            PlayerPrefs.Save(); // Save the new data
        }
    }
}
