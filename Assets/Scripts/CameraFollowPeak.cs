using UnityEngine;

public class CameraFollowPeak : MonoBehaviour
{
    [SerializeField] private StackManager stackManager;

    [Header("Where to place peak on screen")]
    [Range(0f, 1f)]
    [SerializeField] private float peakViewportY = 0.85f; // 0.5 = center
    //[SerializeField] private float yOffset = 2f;      // Older- delete after testing screen flip
    [SerializeField] private float followSpeed = 3f;

    [Header("Clamp downwards")]
    [SerializeField] private bool clampMinY = true;     // ensuring it does not exceed range limit
    [SerializeField] private float minY = 0f;
    [SerializeField] private Camera Cam;

    private float _targetY;

    private void Awake()
    {
        // Default initial target is current Y (so no jump)
        _targetY = transform.position.y;
    }

    private void OnEnable()
    {
        if (stackManager != null)
        {
            stackManager.OnMaxHeightChanged += HandleMaxHeightChanged;
        }
    }

    private void OnDisable()
    {
        if (stackManager != null)
        {
            stackManager.OnMaxHeightChanged -= HandleMaxHeightChanged;
        }
    }

    private void HandleMaxHeightChanged(float maxHeight)
    {
        /// <summary>
        /// The camera now follows up to ViewportY manually set, so the camera won't move too high where the UI is.
        /// should work or Horizontal and Vertical (math won't break), need to test after implementing.
        /// </summary>


        float ortho = Cam.orthographicSize;

        // Offset from camera center to where we want the peak to appear
        float offset = (peakViewportY - 0.5f) * 2f * ortho;

        // Solve for cameraY so that maxHeight appears at peakViewportY
        float desiredCamY = maxHeight - offset;

        if (clampMinY)
            desiredCamY = Mathf.Max(desiredCamY, minY);

        _targetY = desiredCamY;

        // Older- delete after testing screen flip
        //float desired = maxHeight + yOffset;
        //if (clampMinY)
        //    desired = Mathf.Max(desired, minY);

        //_targetY = desired;
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(pos.y, _targetY, followSpeed * Time.deltaTime);
        transform.position = pos;
    }
}

