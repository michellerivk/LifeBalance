using UnityEngine;

public class CameraFollowPeak : MonoBehaviour
{
    [SerializeField] private StackManager stackManager;

    [SerializeField] private float yOffset = 2f;
    [SerializeField] private float followSpeed = 3f;

    [SerializeField] private bool clampMinY = true;     // ensuring it does not exceed range limit
    [SerializeField] private float minY = 0f;

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
        float desired = maxHeight + yOffset;
        if (clampMinY)
            desired = Mathf.Max(desired, minY);

        _targetY = desired;
    }

    private void LateUpdate()
    {
        Vector3 pos = transform.position;
        pos.y = Mathf.Lerp(pos.y, _targetY, followSpeed * Time.deltaTime);
        transform.position = pos;
    }
}


//using UnityEngine;

//public class CameraFollowPeak : MonoBehaviour
//{
//    [SerializeField] private StackManager stackManager;

//    [SerializeField] private float yOffset = 2f;      // how far above the peak to stay
//    [SerializeField] private float followSpeed = 3f;  // smoothing

//    [SerializeField] private bool clampMinY = true;
//    [SerializeField] private float minY = 0f;         // don't go below this Y

//    private void LateUpdate()
//    {
//        // Find all items currently in the scene
//        BalanceItem[] items = FindObjectsOfType<BalanceItem>();
//        if (items.Length == 0)
//            return;

//        // Find max Y among them
//        float maxY = items[0].transform.position.y;
//        for (int i = 1; i < items.Length; i++)
//        {
//            float y = items[i].transform.position.y;
//            if (y > maxY)
//                maxY = y;
//        }

//        float targetY = maxY + yOffset;

//        if (clampMinY)
//            targetY = Mathf.Max(targetY, minY);

//        // Smoothly move camera's Y toward targetY
//        Vector3 pos = transform.position;
//        pos.y = Mathf.Lerp(pos.y, targetY, followSpeed * Time.deltaTime);
//        transform.position = pos;
//    }
//}
