using UnityEngine;

public class ScreenNoiseApplier : MonoBehaviour
{
    [Header("Noise-only settings")]
    [SerializeField] private float grayscaleAmount = 0f; // keep grayscale off
    [SerializeField] private float noiseActive = 1f;      // noise on (0/1 or 0..1 depending on shader)

    [Header("Shader property names (must match your shader)")]
    [SerializeField] private string grayscaleProp = "_GrayscaleAmount";
    [SerializeField] private string noiseProp = "_NoiseActive";

    [SerializeField] private SpriteRenderer[] _renderers;
    private MaterialPropertyBlock _mpb;

    private void Awake()
    {
        //_renderers = GetComponentsInChildren<SpriteRenderer>(includeInactive: true);
        _mpb = new MaterialPropertyBlock();
        Apply();
    }

    [ContextMenu("Apply Now")]
    public void Apply()
    {
        foreach (var r in _renderers)
        {
            if (!r) continue;

            r.GetPropertyBlock(_mpb);
            _mpb.SetFloat(grayscaleProp, grayscaleAmount);
            _mpb.SetFloat(noiseProp, noiseActive);
            r.SetPropertyBlock(_mpb);
        }
    }
}
