using System;
using System.Collections.Generic;
using UnityEngine;

public class ItemDiscovery : MonoBehaviour
{
    private readonly HashSet<string> discovered = new HashSet<string>();

    public event Action<string> OnDiscovered; // itemId

    public bool IsDiscovered(string itemId) => discovered.Contains(itemId);

    public void MarkDiscovered(string itemId)
    {
        Debug.Log($"[Discovery] MarkDiscovered({itemId})");

        if (string.IsNullOrEmpty(itemId)) return;
        if (discovered.Add(itemId))
        {
            Debug.Log($"[Discovery] NEW discovered: {itemId}");
            OnDiscovered?.Invoke(itemId);
        }
        else
        {
            Debug.Log($"[Discovery] Already discovered: {itemId}");
        }
    }
}
