using System;
using UnityEngine;

public class DraggableItem : MonoBehaviour
{
    [HideInInspector] public ItemSlot originSlot;

    private bool _isDragging = false;
    private bool _hasNotifiedSlot = false;

    private Vector3 _dragOffset;
    private Camera _cam;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _cam = Camera.main;
        _rb = GetComponent<Rigidbody2D>();

        if (_rb != null)
        {
            _rb.bodyType = RigidbodyType2D.Kinematic; // start as kinematic so it doesn't fall immediately
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("DEBUG: Enter OnMouseClick");
        _isDragging = true;

        // notify slot only once, first time this item is picked up
        if (!_hasNotifiedSlot && originSlot != null)
        {
            originSlot.OnItemTaken();
            _hasNotifiedSlot = true;
        }

        if (_rb != null)
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0f;
            _rb.bodyType = RigidbodyType2D.Kinematic;
        }

        Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = transform.position.z;
        _dragOffset = transform.position - mouseWorldPos;
    }

    private void OnMouseDrag()
    {
        if (!_isDragging) return;

        Vector3 mouseWorldPos = _cam.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = transform.position.z;
        transform.position = mouseWorldPos + _dragOffset;
    }

    private void OnMouseUp()
    {
        _isDragging = false;

        if (_rb != null)
        {
            _rb.bodyType = RigidbodyType2D.Dynamic; // let physics take over, it can fall/stack
        }
    }
}
