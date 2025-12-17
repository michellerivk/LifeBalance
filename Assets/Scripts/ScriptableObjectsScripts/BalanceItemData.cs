using UnityEngine;

[CreateAssetMenu(fileName = "BalanceItemData", menuName = "Scriptable Objects/Blance Item Data")]
public class BalanceItemData : ScriptableObject
{
    public string itemId;
    public GameObject worldPrefab;
    public Sprite iconSprite;
    public int score = 1;
}
