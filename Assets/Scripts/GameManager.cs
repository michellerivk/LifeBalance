using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Item source")]
    [SerializeField] private BalanceItemDatabase database;

    [Header("UI slots")]
    [SerializeField] private ItemSlotUI[] slots;

    [Header("Placement")]
    [SerializeField] private Collider2D _placementZone;

    private BalanceItemData[] itemList;
    private int _lives = 3;
    private int _score = 0;

    private void Start()
    {
        itemList = new BalanceItemData[slots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            itemList[i] = database.GetRandom();
            slots[i].SetData(itemList[i]);
        }
    }

    // Checks if the item fell on the good zone
    public void OnItemReleased(int slotIndex, DraggableItem item)
    {
        item.EndDrag();

        bool fellInZone = IsInPlacementZone(item.GetComponent<Collider2D>()); // Checks if the item fell inside the wanted zone

        if (!fellInZone)
        {
            _lives = Mathf.Max(0, _lives - 1);
        }

        if( _lives == 0)
        {
            PlayerLost(_score);
            // TODO: play sad music, probably will add an event for that
            return;
        }

        ConsumeSlotAndRefill(slotIndex);
        RefreshSlots();
    }

    // Returns whether 
    private bool IsInPlacementZone(Collider2D itemCol)
    {
        if (_placementZone == null || itemCol == null) return false;

        var filter = new ContactFilter2D();
        filter.useTriggers = true;
        filter.useLayerMask = false;

        Collider2D[] results = new Collider2D[16];
        int count = itemCol.Overlap(filter, results); // checks which colliders are currently overlapping itemCol using filter, and writes them into results.

        for (int i = 0; i < count; i++)
            if (results[i] == _placementZone) // Checks if one of the colliders is the placement zone.
                return true; // returns true if yes

        return false; // If none of the overlaps matched _placementZone, then the item is not in the zone.
    }

    private void ConsumeSlotAndRefill(int slotIndex)
    {
        // Remove used slot and shift left from that point
        for (int i = slotIndex; i < itemList.Length - 1; i++)
            itemList[i] = itemList[i + 1];

        itemList[itemList.Length - 1] = database.GetRandom();
    }

    private void RefreshSlots()
    {
        for (int i = 0; i < slots.Length; i++)
            slots[i].SetData(itemList[i]);
    }

    private void PlayerLost(int points)
    {
        HighscoreManager.TryUpdateHighscore(points); // Update the highscore

        int newHighScore = HighscoreManager.GetHighScore(); // Get the new highscore

        // TODO: add a visual (text or something) that shows 'newHighScore' - Probably in the death menu

    }
}
