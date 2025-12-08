using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [Header("Data")]
    public BalanceItemDatabase database;

    [Header("Spawn point (where the item appears)")]
    public Transform spawnPoint;

    private GameObject _spawnedItem;

    private void Start()
    {
        FillSlotIfEmpty();
    }

    public void FillSlotIfEmpty()
    {
        if (_spawnedItem != null) return;

        var data = database.GetRandom();
        if (data == null) return;

        _spawnedItem = Instantiate(data.prefab, spawnPoint.position, Quaternion.identity);

        // parent under slot so it moves with UI/world element if needed
        // _spawnedItem.transform.SetParent(transform);

        var draggable = _spawnedItem.GetComponent<DraggableItem>();
        if (draggable != null)
        {
            draggable.originSlot = this;
        }
    }

    // Called by the item when the player first takes it out of the slot
    public void OnItemTaken()
    {
        _spawnedItem = null;
        FillSlotIfEmpty();  // Immediately refill with a new random item
    }
}
