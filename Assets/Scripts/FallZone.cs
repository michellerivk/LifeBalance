using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System;

[RequireComponent(typeof(Collider2D))]
public class FallZone : MonoBehaviour
{
    //[SerializeField] private GameManager gameManager;
    [SerializeField] private int fallsToLose = 3;
    [SerializeField] private TextMeshProUGUI _playerLost;
    [SerializeField] private Canvas _endGame;
    
    [Header("Shader noise range")]
    [Range(0f, 2f)] [SerializeField] private float noiseTarget = 1;

    public static event Action<BalanceItem> OnItemFell;
    public static event Action OnGameOver;

    private int fallsCount;
    private bool gameOver;

    // Prevent counting the same item multiple times (bounces / Stay calls / multiple colliders)
    private readonly HashSet<int> counted = new HashSet<int>();

    private void OnTriggerEnter2D(Collider2D other) => TryCount(other);
    //private void OnTriggerStay2D(Collider2D other) => TryCount(other);

    private void TryCount(Collider2D other)
    {
        if (gameOver) return;

        BalanceItem item = other.GetComponentInParent<BalanceItem>();
        if (item == null) return;

        Debug.Log($"[FallZone] TriggerEnter by {other.name}");
        //Debug.Log("Got Item");

        var fx = item.GetComponentInChildren<ShaderEffectFader>();      // shader effect on hit
        if (fx != null)
        {
            fx.FadeTo(grayscaleTarget: 1f, noiseTarget);
            //Debug.Log($"Fade Item {fx.name}");
        }

        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
        if (rb == null || rb.bodyType != RigidbodyType2D.Dynamic) return;

        int id = item.GetInstanceID();
        if (!counted.Add(id)) return; // already counted

        OnItemFell?.Invoke(item);       // subscries to discovered items in ItemDiscovery
        Debug.Log($"[FallZone] OnItemFell fired for {item.data.itemId} id={item.GetInstanceID()} body={rb.bodyType}");

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

            OnGameOver?.Invoke();
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

        BalanceItem[] allItems = GameObject.FindObjectsByType<BalanceItem>(FindObjectsSortMode.None); // Object -> Gameobject (error?)
        
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
