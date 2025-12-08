using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BalanceItem : MonoBehaviour
{   
    /// <summary>
    /// Define what BalanceItem is , like pizza that holds toppings- it holds the score.
    /// </summary>
    public BalanceItemData data;

    public int Score => data != null ? data.score : 0;
}
