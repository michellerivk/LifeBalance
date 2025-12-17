using UnityEngine;

[CreateAssetMenu(fileName = "BalanceItemDatabase", menuName = "Scriptable Objects/Balance Item Database")]
public class BalanceItemDatabase : ScriptableObject
{
    public BalanceItemData[] items;

    public BalanceItemData GetRandom()
    {
        if (items == null || items.Length == 0)     // Debug if empty
        {
            Debug.LogError("Item database is empty!");
            return null;
        }

        int index = Random.Range(0, items.Length);
        return items[index];
    }
}
