using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FallZone : MonoBehaviour
{
    //[SerializeField] private GameManager gameManager;
    [SerializeField] private int fallsToLose = 3;
    [SerializeField] private TextMeshProUGUI _playerLost;

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

        Debug.Log("Got Item");

        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
        if (rb == null || rb.bodyType != RigidbodyType2D.Dynamic) return;

        int id = item.GetInstanceID();
        if (!counted.Add(id)) return; // already counted

        fallsCount++;

        if (fallsCount >= fallsToLose) 
        {
            gameOver = true;

            Debug.Log("Game Over");

            int points = 0; // keep 0 for now, or we’ll hook score later

            PlayerLost(points);
        }

    }

    private void PlayerLost(int points)
    {
        HighscoreManager.TryUpdateHighscore(points); // Update the highscore

        int newHighScore = HighscoreManager.GetHighScore(); // Get the new highscore

        _playerLost.gameObject.SetActive(true);

        // TODO: add a visual (text or something) that shows 'newHighScore'

    }
}
