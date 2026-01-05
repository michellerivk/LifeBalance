using UnityEngine;

[ExecuteAlways] // Run monobehaviour methods during editting
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteScaleToScreen : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;
    [SerializeField] private SpriteRenderer sr;

    [Header("Size as % of screen height")]
    [Range(0.05f, 1f)][SerializeField] private float portraitPercent = 0.18f;
    [Range(0.05f, 1f)][SerializeField] private float landscapePercent = 0.30f;

    private bool lastLandscape;

    private void Awake()
    {
        if (targetCamera == null) targetCamera = Camera.main;
    }

    private void OnEnable() => Apply();
    private void Start() => Apply();

    private void Update()
    {
        // In play mode, update only when orientation changes.
        // In editor, also update when Game view changes.
        #if UNITY_EDITOR
                Apply();
        #else
                bool isLandscape = Screen.width > Screen.height;
                if (isLandscape != lastLandscape)
                    Apply();
        #endif
    }

    private void Apply()
    {
        if (sr == null || sr.sprite == null) return;
        if (targetCamera == null) targetCamera = Camera.main;
        if (targetCamera == null) return;
        if (!targetCamera.orthographic) return; // intended for 2D ortho

        bool isLandscape = Screen.width > Screen.height;
        lastLandscape = isLandscape;

        float percent = isLandscape ? landscapePercent : portraitPercent;

        // Visible world height
        float worldScreenHeight = targetCamera.orthographicSize * 2f;

        // Desired world height for the sprite
        float desiredHeight = worldScreenHeight * percent;

        // Sprite's unscaled world height
        float spriteHeight = sr.sprite.bounds.size.y;

        float scale = desiredHeight / spriteHeight;
        transform.localScale = new Vector3(scale, scale, 1f);
    }
}
