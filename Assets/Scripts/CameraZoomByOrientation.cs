using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraZoomByOrientation : MonoBehaviour
{
    [Header("Ortho sizes")]
    [SerializeField] private float landscapeOrthoSize = 11f;
    [SerializeField] private float portraitOrthoSize = 13f; // bigger view for portrait = zoom out

    [Header("Smooth")]
    [SerializeField] private float zoomSpeed = 4f; // higher = snappier , or lower = flowing. change however you like

    private Camera _cam;
    private bool _lastLandscape;
    private float _targetSize;

    private void Awake()
    {
        _cam = GetComponent<Camera>();
        if (!_cam.orthographic)
            Debug.LogWarning("CameraZoomByOrientation expects an Orthographic camera.");

        _lastLandscape = IsLandscape();
        _targetSize = _lastLandscape ? landscapeOrthoSize : portraitOrthoSize;

        // Snap on start so it doesn't animate from a wrong size
        _cam.orthographicSize = _targetSize;
    }

    private void Update()
    {
        bool isLandscape = IsLandscape();
        if (isLandscape != _lastLandscape)
        {
            _lastLandscape = isLandscape;
            _targetSize = isLandscape ? landscapeOrthoSize : portraitOrthoSize;
        }

        // Smoothly move toward target size
        _cam.orthographicSize = Mathf.Lerp(
            _cam.orthographicSize,
            _targetSize,
            1f - Mathf.Exp(-zoomSpeed * Time.deltaTime)
        );
    }

    private static bool IsLandscape()
    {
        return Screen.width > Screen.height;
    }
}
