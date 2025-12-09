using UnityEngine;
using System;
using System.Collections.Generic;

public class StackManager : MonoBehaviour
{
    private readonly List<BalanceItem> _items = new List<BalanceItem>();

    public float MaxHeight { get; private set; } = 0f;

    // Other systems (like Camera) can subscribe to this
    public event Action<float> OnMaxHeightChanged;

    private void OnEnable()
    {
        BalanceItem.OnItemEnabled += HandleItemEnabled;
        BalanceItem.OnItemDisabled += HandleItemDisabled;
    }

    private void OnDisable()
    {
        BalanceItem.OnItemEnabled -= HandleItemEnabled;
        BalanceItem.OnItemDisabled -= HandleItemDisabled;
    }

    private void HandleItemEnabled(BalanceItem item)
    {
        if (!_items.Contains(item))
        {
            _items.Add(item);
            RecalculateMaxHeight();
        }
    }

    private void HandleItemDisabled(BalanceItem item)
    {
        if (_items.Remove(item))
        {
            RecalculateMaxHeight();
        }
    }

    private void RecalculateMaxHeight()
    {
        float old = MaxHeight;

        if (_items.Count == 0)
        {
            MaxHeight = 0f;
        }
        else
        {
            float maxY = _items[0].transform.position.y;
            for (int i = 1; i < _items.Count; i++)
            {
                float y = _items[i].transform.position.y;
                if (y > maxY)
                    maxY = y;
            }
            MaxHeight = maxY;
        }

        if (!Mathf.Approximately(old, MaxHeight))
        {
            OnMaxHeightChanged?.Invoke(MaxHeight);
        }
    }
}
