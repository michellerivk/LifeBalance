using System.Collections;
using UnityEngine;

public class ShaderEffectFader : MonoBehaviour
{
    [SerializeField] private SpriteRenderer rend;
    [SerializeField] private float duration = 0.6f;

    [Header("Shader property names")]
    [SerializeField] private string grayscaleProp = "_GrayscaleAmount";
    [SerializeField] private string noiseProp = "_NoiseActive";

    MaterialPropertyBlock mpb;
    Coroutine routine;

    private void Awake()
    {
        if (!rend) rend = GetComponentInChildren<SpriteRenderer>();
        mpb = new MaterialPropertyBlock();
    }

    public void FadeTo(float grayscaleTarget, float noiseTarget)
    {
        //Debug.Log($"Enter FadeTo");
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FadeRoutine(grayscaleTarget, noiseTarget));
        //Debug.Log($"Exit FadeTo");
    }

    private IEnumerator FadeRoutine(float grayTarget, float noiseTarget)
    {
        //Debug.Log($"Entered coroutine FadeRoutine");

        rend.GetPropertyBlock(mpb);

        float grayStart = mpb.GetFloat(grayscaleProp);
        float noiseStart = mpb.GetFloat(noiseProp);

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.SmoothStep(0f, 1f, t / duration);

            mpb.SetFloat(grayscaleProp, Mathf.Lerp(grayStart, grayTarget, a));
            mpb.SetFloat(noiseProp, Mathf.Lerp(noiseStart, noiseTarget, a));

            rend.SetPropertyBlock(mpb);
            yield return null;
        }

        mpb.SetFloat(grayscaleProp, grayTarget);
        mpb.SetFloat(noiseProp, noiseTarget);
        rend.SetPropertyBlock(mpb);
        //Debug.Log($"Exit coroutine FadeRoutine. gray:{grayscaleProp}:{grayTarget}, noise:{noiseProp}:{noiseTarget}");
    }

    public void ApplyImmediate(float grayscaleValue, float noiseValue)
    {
        if (!rend) rend = GetComponentInChildren<SpriteRenderer>();
        if (mpb == null) mpb = new MaterialPropertyBlock();

        rend.GetPropertyBlock(mpb);
        mpb.SetFloat(grayscaleProp, grayscaleValue);
        mpb.SetFloat(noiseProp, noiseValue);
        rend.SetPropertyBlock(mpb);
    }
}
