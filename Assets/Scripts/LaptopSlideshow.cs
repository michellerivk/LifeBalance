using System.Collections;
using UnityEngine;

public class LaptopSlideshow : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private BalanceItemDatabase database;
    [SerializeField] private ItemDiscovery discovery; // the tracker we made earlier

    [Header("Slides (world sprites)")]
    [SerializeField] private SpriteRenderer slideA;
    [SerializeField] private SpriteRenderer slideB;
    [SerializeField] private ShaderEffectFader faderA;
    [SerializeField] private ShaderEffectFader faderB;

    [Header("Motion")]
    [SerializeField] private float slideDistance = 2.0f; // world units across the screen width
    [SerializeField] private float slideDuration = 0.4f;
    [SerializeField] private float pauseSeconds = 2.0f;

    private int _index = 0;
    private bool _aIsCurrent = true;

    private void Awake()
    {
        if (!discovery) discovery = FindFirstObjectByType<ItemDiscovery>();

        if (!faderA) faderA = slideA.GetComponent<ShaderEffectFader>();
        if (!faderB) faderB = slideB.GetComponent<ShaderEffectFader>();
    }

    private void OnEnable()
    {
        if (discovery != null)
            discovery.OnDiscovered += HandleDiscovered;
    }

    private void OnDisable()
    {
        if (discovery != null)
            discovery.OnDiscovered -= HandleDiscovered;
    }

    private void Start()
    {
        if (database == null || database.items == null || database.items.Length == 0)
        {
            Debug.LogError("LaptopSlideshow: database is empty.");
            enabled = false;
            return;
        }

        // Put first icon on current slide
        SetSlideToData(CurrentSlideRenderer(), CurrentSlideFader(), database.items[_index]);

        // Put next icon on incoming slide (offscreen right)
        SetSlideToData(IncomingSlideRenderer(), IncomingSlideFader(), database.items[NextIndex()]);
        PositionSlidesForStart();

        StartCoroutine(LoopRoutine());
    }

    private IEnumerator LoopRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(pauseSeconds);

            yield return SlideOnce();

            // advance index (in order)
            _index = NextIndex();

            // after sliding, swap roles
            _aIsCurrent = !_aIsCurrent;

            // assign NEW incoming slide data (the one after current)
            var incomingData = database.items[NextIndex()];
            SetSlideToData(IncomingSlideRenderer(), IncomingSlideFader(), incomingData);

            // reset positions for next run
            PositionSlidesForStart();
        }
    }

    private IEnumerator SlideOnce()
    {
        Transform cur = CurrentSlideRenderer().transform;
        Transform inc = IncomingSlideRenderer().transform;

        Vector3 curStart = cur.localPosition;
        Vector3 incStart = inc.localPosition;

        Vector3 curEnd = curStart + Vector3.left * slideDistance;
        Vector3 incEnd = incStart + Vector3.left * slideDistance;

        float t = 0f;
        while (t < slideDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.SmoothStep(0f, 1f, t / slideDuration);

            cur.localPosition = Vector3.Lerp(curStart, curEnd, a);
            inc.localPosition = Vector3.Lerp(incStart, incEnd, a);

            yield return null;
        }

        cur.localPosition = curEnd;
        inc.localPosition = incEnd;
    }

    private void PositionSlidesForStart()
    {
        // current slide in center; incoming slide just to the right
        CurrentSlideRenderer().transform.localPosition = Vector3.zero;
        IncomingSlideRenderer().transform.localPosition = Vector3.right * slideDistance;
    }

    private void SetSlideToData(SpriteRenderer sr, ShaderEffectFader fx, BalanceItemData data)
    {
        if (data == null)
        {
            sr.sprite = null;
            return;
        }

        sr.sprite = data.iconSprite;

        // locked = grayscale+noise ON, discovered = OFF
        bool isUnlocked = discovery != null && discovery.IsDiscovered(data.itemId);

        if (fx != null)
        {
            if (isUnlocked)
                fx.ApplyImmediate(grayscaleValue: 0f, noiseValue: 0f);
            else
                fx.ApplyImmediate(grayscaleValue: 1f, noiseValue: 1f);
        }

        // store itemId on renderer name for easy match (simple trick)
        sr.gameObject.name = "Slide_" + data.itemId;
    }

    private void HandleDiscovered(string itemId)
    {
        // If the currently displayed slide matches this item, fade it to color.
        var sr = CurrentSlideRenderer();
        var fx = CurrentSlideFader();

        // We named it Slide_<itemId>
        if (sr != null && sr.gameObject.name == "Slide_" + itemId && fx != null)
        {
            fx.FadeTo(grayscaleTarget: 0f, noiseTarget: 0f);
        }
    }

    private int NextIndex()
    {
        return (_index + 1) % database.items.Length;
    }

    private SpriteRenderer CurrentSlideRenderer() => _aIsCurrent ? slideA : slideB;
    private ShaderEffectFader CurrentSlideFader() => _aIsCurrent ? faderA : faderB;

    private SpriteRenderer IncomingSlideRenderer() => _aIsCurrent ? slideB : slideA;
    private ShaderEffectFader IncomingSlideFader() => _aIsCurrent ? faderB : faderA;
}
