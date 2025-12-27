using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _playerLost;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void PlayerLost(int points)
    {
        HighscoreManager.TryUpdateHighscore(points); // Update the highscore

        int newHighScore = HighscoreManager.GetHighScore(); // Get the new highscore

        _playerLost.enabled = true;

        // TODO: add a visual (text or something) that shows 'newHighScore'

    }
}
