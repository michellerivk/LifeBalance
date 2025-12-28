using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class DraggableItem : MonoBehaviour
{
    private bool _isDragging = false;
    private Vector3 _dragOffset;
    private Rigidbody2D _rb;
    private Camera _cam;

    public void Initialize(Camera cam)
    {
        _cam = cam;
        if (_rb == null)
            _rb = GetComponent<Rigidbody2D>();

        _rb.bodyType = RigidbodyType2D.Kinematic; // start under our control
    }

    public void BeginDrag(Vector2 screenPos)
    {
        if (_cam == null) _cam = Camera.main;
        if (_rb == null) _rb = GetComponent<Rigidbody2D>();

        _isDragging = true;

        AudioManager.instance.PlayLowerSFXVolume(5, 0.3f);
        AudioManager.instance.PlaySFXPitchAdjusted(5);

        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;
        _rb.bodyType = RigidbodyType2D.Kinematic;

        Vector3 worldPos = ScreenToWorld(screenPos);
        worldPos.z = transform.position.z;
        _dragOffset = transform.position - worldPos;
    }

    public void Drag(Vector2 screenPos)
    {
        if (!_isDragging) return;

        Vector3 worldPos = ScreenToWorld(screenPos);
        worldPos.z = transform.position.z;
        transform.position = worldPos + _dragOffset;
    }

    public void EndDrag()
    {
        _isDragging = false;
        //if (_rb == null) _rb = GetComponent<Rigidbody2D>();   // extra safety
        _rb.bodyType = RigidbodyType2D.Dynamic; // physics takes over
    }

    private Vector3 ScreenToWorld(Vector2 screenPos)
    {
        // For 2D: camera usually at z = -10, world at z = 0
        float distance = -_cam.transform.position.z;
        Vector3 sp = new Vector3(screenPos.x, screenPos.y, distance);
        return _cam.ScreenToWorldPoint(sp);
    }
}
