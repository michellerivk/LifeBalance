using NUnit.Framework.Interfaces;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    /*
        slots: InventorySlot[3]
        availableItems: ItemData[]
        level: LevelData
        gameManager: GameManager

        Initialize(LevelData level) : void
        FillAllSlots() : void
        GetRandomItemData() : ItemData
        OnSlotUsed(int index) : void // called by InventorySlot when drag confirmed
        SpawnWorldItem(ItemData data, Vector3 worldPosition) : Item
    */


}
