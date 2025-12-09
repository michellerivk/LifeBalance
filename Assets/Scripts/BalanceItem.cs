using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class BalanceItem : MonoBehaviour
{
    /// <summary>
    /// Define what BalanceItem is , like pizza that holds toppings- it holds the score.
    /// Idea here: every time an item spawns or disappears, it “raises its hand” so a manager can track it, without a direct reference.
    /// </summary>
    public BalanceItemData data;

    public static event Action<BalanceItem> OnItemEnabled;
    public static event Action<BalanceItem> OnItemDisabled;

    public int Score => data != null ? data.score : 0;

    private void OnEnable()
    {
        OnItemEnabled?.Invoke(this);
    }

    private void OnDisable()
    {
        OnItemDisabled?.Invoke(this);
    }
}
