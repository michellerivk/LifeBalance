using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ScaleZone : MonoBehaviour
{
    [SerializeField] private ItemDiscovery discovery;
    [SerializeField] private float successDelay = 0.5f;

    // Track running “success checks” per item instance id
    private readonly Dictionary<int, Coroutine> pending = new Dictionary<int, Coroutine>();
    private readonly HashSet<int> fallen = new HashSet<int>();

    private void Awake()
    {
        if (!discovery) discovery = FindFirstObjectByType<ItemDiscovery>();
    }

    private void OnEnable()
    {
        FallZone.OnItemFell += HandleItemFell;
    }

    private void OnDisable()
    {
        FallZone.OnItemFell -= HandleItemFell;
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    var item = other.GetComponentInParent<BalanceItem>();
    //    if (item == null || item.data == null) return;

    //    var rb = item.GetComponent<Rigidbody2D>();
    //    if (rb == null || rb.bodyType != RigidbodyType2D.Dynamic) return;

    //    int id = item.GetInstanceID();
        
    //    Debug.Log($"[ScaleZone] Enter: {item.name} id={item.GetInstanceID()} itemId={item.data.itemId} body={rb.bodyType}");

    //    // If it already fell, don’t start success check
    //    if (fallen.Contains(id)) return;

    //    // Don’t start duplicate coroutine
    //    if (pending.ContainsKey(id)) return;
        
    //    Debug.Log($"[ScaleZone] Start success timer for {item.data.itemId}");

    //    pending[id] = StartCoroutine(SuccessAfterDelay(item, id));
    //}
    private void OnTriggerStay2D(Collider2D other)
    {
        var item = other.GetComponentInParent<BalanceItem>();
        if (item == null || item.data == null) return;

        int id = item.GetInstanceID();
        if (pending.ContainsKey(id)) return;
        if (fallen.Contains(id)) return;

        pending[id] = StartCoroutine(SuccessAfterDelay(item, id));
    }

    private void HandleItemFell(BalanceItem item)
    {
        if (item == null) return;

        int id = item.GetInstanceID();
        fallen.Add(id);

        // Cancel any pending “success” for this item
        if (pending.TryGetValue(id, out var co))
        {
            StopCoroutine(co);
            pending.Remove(id);
        }
    }

    private IEnumerator SuccessAfterDelay(BalanceItem item, int id)
    {
        Debug.Log($"[ScaleZone] Waiting {successDelay}s for {item.data.itemId}");

        float t = 0f;

        // Wait successDelay seconds, but fail if item is destroyed or marked fallen
        while (t < successDelay)
        {
            if (item == null) { pending.Remove(id); yield break; }
            if (fallen.Contains(id))
            {
                Debug.Log($"[ScaleZone] ABORT (fell): {id}");
                
                pending.Remove(id); yield break; 
            }

            t += Time.deltaTime;
            yield return null;
        }

        // Still alive and not fallen -> success
        if (item != null && item.data != null && !fallen.Contains(id))
        {
            discovery.MarkDiscovered(item.data.itemId);
            Debug.Log($"[ScaleZone] SUCCESS: discovered {item.data.itemId}");

            // Optional: if you also want the in-world item to become “clean” now:
            var fx = item.GetComponentInChildren<ShaderEffectFader>();
            if (fx != null)
                fx.FadeTo(grayscaleTarget: 0f, noiseTarget: 0f);
        }

        pending.Remove(id);
    }
}
