using UnityEngine;

public static class HighscoreManager
{
    const string EasyHighScoreKey = "EasyHighScore"; // The key of the Easy highscore PlayerPref
    const string NormalHighScoreKey = "NormalHighScore"; // The key of the Normal highscore PlayerPref
    const string HardHighScoreKey = "HardHighScore"; // The key of the Hard highscore PlayerPref

    // Player prefs -> a way to save a small piece of data between sessions.
    public static int GetEasyHighScore()
    {
        // Return the value that's connected to the "EasyHighScore" key.
        // If there's no value yet (first playthrough), return a 0
        return PlayerPrefs.GetInt(EasyHighScoreKey, 0);
    }
    public static int GetNormalHighScore()
    {
        return PlayerPrefs.GetInt(NormalHighScoreKey, 0); 
    }
    public static int GetHardHighScore()
    {
        return PlayerPrefs.GetInt(HardHighScoreKey, 0);
    }

    public static void TryUpdateEasyHighscore(int newScore)
    {
        int currentHighscore = GetEasyHighScore();
        if (currentHighscore < newScore) // If the new score is greater than the current highscore
        {
            PlayerPrefs.SetInt(EasyHighScoreKey, newScore); // Insert the new score inside the save file
            PlayerPrefs.Save(); // Save the new data
            Debug.Log("Easy Highscore Changed");
        }
    }

    public static void TryUpdateNormalHighscore(int newScore)
    {
        int currentHighscore = GetNormalHighScore();
        if (currentHighscore < newScore) // If the new score is greater than the current highscore
        {
            PlayerPrefs.SetInt(NormalHighScoreKey, newScore); // Insert the new score inside the save file
            PlayerPrefs.Save(); // Save the new data
            Debug.Log("Normal Highscore Changed");
        }
    }

    public static void TryUpdateHardHighscore(int newScore)
    {
        int currentHighscore = GetHardHighScore();
        if (currentHighscore < newScore) // If the new score is greater than the current highscore
        {
            PlayerPrefs.SetInt(HardHighScoreKey, newScore); // Insert the new score inside the save file
            PlayerPrefs.Save(); // Save the new data
            Debug.Log("Hard Highscore Changed");
        }
    }
}
