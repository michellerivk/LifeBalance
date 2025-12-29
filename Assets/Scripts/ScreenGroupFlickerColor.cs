using UnityEngine;

public class ScreenGroupFlickerColor : MonoBehaviour
{
    [Header("Brightness range")]
    [Range(0f, 2f)] public float baseBrightness = 1f;
    [Range(0f, 1f)] public float flickerAmount = 0.08f;
    public float flickerSpeed = 10f;

    [Header("Slow pulse")]
    [Range(0f, 1f)] public float slowPulseAmount = 0.05f;
    public float slowPulseSpeed = 1.2f;

    [Header("Sprite Renderer")]
    [SerializeField] private SpriteRenderer[] _renderers;
    private Color[] _baseColors;

    private void Awake()
    {
        //_renderers = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
        if (_renderers == null) { GetComponentsInChildren<SpriteRenderer>(includeInactive: true); }
        _baseColors = new Color[_renderers.Length];

        for (int i = 0; i < _renderers.Length; i++)
            _baseColors[i] = _renderers[i] ? _renderers[i].color : Color.white;
    }

    private void LateUpdate()
    {
        float slow = Mathf.Sin(Time.time * slowPulseSpeed) * slowPulseAmount;
        float fast = (Mathf.PerlinNoise(Time.time * flickerSpeed, 0.37f) - 0.5f) * 2f * flickerAmount;

        float b = Mathf.Clamp(baseBrightness + slow + fast, 0f, 2f);

        for (int i = 0; i < _renderers.Length; i++)
        {
            var r = _renderers[i];
            if (!r) continue;

            Color baseC = _baseColors[i];
            // Multiply RGB only, keep alpha
            r.color = new Color(baseC.r * b, baseC.g * b, baseC.b * b, baseC.a);
        }
    }

    // Call this if you change colors at runtime and want flicker to respect new base colors.
    public void RebindBaseColors()
    {
        for (int i = 0; i < _renderers.Length; i++)
            if (_renderers[i]) _baseColors[i] = _renderers[i].color;
    }
}

