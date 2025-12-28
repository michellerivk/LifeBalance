using UnityEngine;

public class PlacementMilestones : MonoBehaviour
{
    public static PlacementMilestones instance;

    [SerializeField] private int milestoneStep = 10;
    [SerializeField] private int milestoneSfxIndex = 6;

    private int placedCount = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        ResetCounter();
    }

    public void RegisterPlacedItem()
    {
        placedCount++;

        if (placedCount % milestoneStep == 0)
        {
            AudioManager.instance.PlayLowerSFXVolume(milestoneSfxIndex, 0.3f);
        }
    }

    public void ResetCounter()
    {
        placedCount = 0;
    }
}
