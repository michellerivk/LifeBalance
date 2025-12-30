using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FallZone : MonoBehaviour
{
    //[SerializeField] private GameManager gameManager;
    [SerializeField] private int fallsToLose = 3;
    [SerializeField] private TextMeshProUGUI _playerLost;
    [SerializeField] private Canvas _endGame;

    // Making the vars static to share them between the 3 fall zones
    private int fallsCount;
    private bool gameOver;

    // Prevent counting the same item multiple times (bounces / Stay calls / multiple colliders)
    private readonly HashSet<int> counted = new HashSet<int>();

    private void OnTriggerEnter2D(Collider2D other) => TryCount(other);
    // Take a life even if directly on the board
    private void OnTriggerStay2D(Collider2D other) => TryCount(other);

    private void TryCount(Collider2D other)
    {
        if (gameOver) return;

        BalanceItem item = other.GetComponentInParent<BalanceItem>();
        if (item == null) return;

        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
        if (rb == null || rb.bodyType != RigidbodyType2D.Dynamic) return;

        int id = item.GetInstanceID();
        if (!counted.Add(id)) return; // already counted


        var fx = item.GetComponentInChildren<ShaderEffectFader>();      // shader effect on hit
        if (fx != null)
        {
            fx.FadeTo(grayscaleTarget: 1f, noiseTarget: 1f);
            Debug.Log($"Fade Item {fx.name}");
        }

        fallsCount++;

        AudioManager.instance.PlayLowerSFXVolume(1, 0.3f);
        AudioManager.instance.PlaySFXPitchAdjusted(1); // Play item fell sound

        if (fallsCount >= fallsToLose) 
        {
            gameOver = true;

            Debug.Log("Game Over");

            int points = CalculateStackScore();

            PlayerLost(points);

            AudioManager.instance.PlayLowerSFXVolume(2, 0.3f);
            AudioManager.instance.PlaySFX(2); // Play losing sound
        }

    }

    private void PlayerLost(int points)
    {
        HighscoreManager.TryUpdateHighscore(points); // Update the highscore

        int newHighScore = HighscoreManager.GetHighScore(); // Get the new highscore

        _playerLost.text = $"Current Score: {points},Highscore: {newHighScore}";

        _endGame.gameObject.SetActive(true);
    }

    private int CalculateStackScore()
    {
        int total = 0;

        // Find all BalanceItems currently in the scene.

        BalanceItem[] allItems = Object.FindObjectsByType<BalanceItem>(FindObjectsSortMode.None);
        
        foreach (var item in allItems)
        {
            if (item == null) continue;

            // Exclude fallen items
            if (counted.Contains(item.GetInstanceID()))
                continue;

            // Exclude kinematic items
            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            if (rb == null || rb.bodyType != RigidbodyType2D.Dynamic)
                continue;

            total += item.Score;
        }

        return total;
    }
}
