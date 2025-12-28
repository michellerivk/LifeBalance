using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlotUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private BalanceItemDatabase database;
    [SerializeField] private Image iconImage;
    [SerializeField] private Transform worldParent;
    [SerializeField] private Camera worldCamera;
    [SerializeField] private GameObject _popupWindow;

    private BalanceItemData _currentData;
    private DraggableItem _currentItem;

    private void Start()
    {
        if (worldCamera == null)
            worldCamera = Camera.main;

        RollNewItem();
    }

    private void RollNewItem()
    {
        _currentData = database.GetRandom();
        if (_currentData == null)
        {
            iconImage.enabled = false;
            return;
        }

        iconImage.enabled = true;

        if (_currentData.iconSprite != null)
        {
            iconImage.sprite = _currentData.iconSprite;
        }
        else
        {
            iconImage.sprite = null; // or some default
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_currentData == null || _currentItem != null)
            return;

        // Instantiate world object at pointer position
        if (!_popupWindow.gameObject.activeInHierarchy)
        {
            Vector3 startWorldPos = ScreenToWorld(eventData.position);
            GameObject go = Instantiate(_currentData.worldPrefab, startWorldPos, Quaternion.identity, worldParent);

            var draggable = go.GetComponent<DraggableItem>();
            if (draggable == null)
                draggable = go.AddComponent<DraggableItem>();

            draggable.Initialize(worldCamera);
            _currentItem = draggable;
            _currentItem.BeginDrag(eventData.position);

            // Immediately roll next item for the slot UI
            RollNewItem();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_currentItem != null)
        {
            _currentItem.Drag(eventData.position);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (_currentItem != null)
        {
            _currentItem.EndDrag();

            // Check how many items where placed and play a sound if 10 were placed successfully
            if (PlacementMilestones.instance != null)
                PlacementMilestones.instance.RegisterPlacedItem();

            _currentItem = null;
        }
    }

    private Vector3 ScreenToWorld(Vector2 screenPos)
    {
        float distance = -worldCamera.transform.position.z; // for 2D cam at -10
        Vector3 sp = new Vector3(screenPos.x, screenPos.y, distance);
        Vector3 world = worldCamera.ScreenToWorldPoint(sp);
        world.z = 0f; // your 2D plane
        return world;
    }
}
