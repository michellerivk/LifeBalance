using UnityEngine;

public class ScreenGroupFlicker : MonoBehaviour
{
    [Header("Sprite Renderer")]
    [SerializeField] private SpriteRenderer[] _renderers;

    [Header("Brightness range")]
    [Range(0f, 2f)] public float baseBrightness = 1f;
    [Range(0f, 1f)] public float flickerAmount = 0.08f;
    public float flickerSpeed = 10f;

    [Header("Slow pulse")]
    [Range(0f, 1f)] public float slowPulseAmount = 0.05f;
    public float slowPulseSpeed = 1.2f;

    private MaterialPropertyBlock _mpb;
    private static readonly int ColorProp = Shader.PropertyToID("_Color");

    private void Awake()
    {
        if (_renderers == null) { GetComponentsInChildren<SpriteRenderer>(includeInactive: true); }
        _mpb = new MaterialPropertyBlock();
    }

    private void LateUpdate()
    {
        float slow = Mathf.Sin(Time.time * slowPulseSpeed) * slowPulseAmount;
        float fast = (Mathf.PerlinNoise(Time.time * flickerSpeed, 0.37f) - 0.5f) * 2f * flickerAmount;

        float b = Mathf.Clamp(baseBrightness + slow + fast, 0f, 2f);

        // Apply multiplier to each renderer without changing its alpha
        for (int i = 0; i < _renderers.Length; i++)
        {
            var r = _renderers[i];
            if (r == null) continue;

            // Keep original alpha, only multiply RGB brightness
            Color original = r.color;

            // Use property block tint: (b,b,b,1) multiplies material tint
            r.GetPropertyBlock(_mpb);
            _mpb.SetColor(ColorProp, new Color(b, b, b, 1f));
            r.SetPropertyBlock(_mpb);

            // NOTE: We do NOT overwrite r.color here; that keeps artist-set alpha intact.
        }
    }
}
