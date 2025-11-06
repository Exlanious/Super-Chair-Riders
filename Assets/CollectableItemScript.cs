using System;
using UnityEngine;

public class CollectableItemScript : MonoBehaviour
{
    public String itemName;
    [Header("Oscillation")]
    [Tooltip("If true the item will start oscillating automatically on Start.")]
    public bool autoStart = false;

    [Space]
    [Tooltip("Range for X localScale when flipping. Default -1..1 will flip horizontally.")]
    public Vector2 flipRange = new Vector2(-1f, 1f);
    [Tooltip("Flip frequency (cycles per second)")]
    public float flipFrequency = 1f;

    [Space]
    [Tooltip("Range for scale multiplier applied to original Y/Z (and magnitude of X if desired). Example 0.7..1.3)")]
    public Vector2 scaleRange = new Vector2(0.7f, 1.3f);
    [Tooltip("Scale pulse frequency (cycles per second)")]
    public float scaleFrequency = 1f;

    // internal state
    private bool isOscillating = false;
    private float oscTime = 0f;
    private Vector3 originalScale;

    void Start()
    {
        originalScale = transform.localScale;
        if (autoStart) StartOscillate();
    }

    void Update()
    {
        if (!isOscillating) return;

        oscTime += Time.deltaTime;

        // flip value: map sin -> 0..1 then to flipRange
        float flipT = (Mathf.Sin(oscTime * flipFrequency * 2f * Mathf.PI) + 1f) * 0.5f;
        float xVal = Mathf.Lerp(flipRange.x, flipRange.y, flipT);

        // scale multiplier: map sin -> 0..1 then to scaleRange
        float scaleT = (Mathf.Sin(oscTime * scaleFrequency * 2f * Mathf.PI) + 1f) * 0.5f;
        float scaleMul = Mathf.Lerp(scaleRange.x, scaleRange.y, scaleT);

        Vector3 s = originalScale;
        // Combine flip range and scale multiplier for X so both oscillations affect X.
        // final X = originalScale.x * flipValue * scaleMul
        s.x = originalScale.x * xVal * scaleMul;
        s.y = originalScale.y * scaleMul;
        s.z = originalScale.z * scaleMul;
        transform.localScale = s;
    }
    public String getItemName()
    {
        return itemName;
    }

    // Start oscillation using the editor parameters (flipRange/flipFrequency and scaleRange/scaleFrequency).
    public void StartOscillate()
    {
        if (!isOscillating)
        {
            originalScale = transform.localScale;
            oscTime = 0f;
            isOscillating = true;
        }
    }

    // Stop oscillation and restore the original localScale captured when oscillation started.
    public void StopOscillate()
    {
        if (!isOscillating) return;
        isOscillating = false;
        transform.localScale = originalScale;
    }
}
