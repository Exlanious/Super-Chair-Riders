using System;
using UnityEngine;

public class CollectableItemScript : MonoBehaviour
{
    public String itemName;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartOscillate(0.5f, 1, 1); // start oscillating at 0.5 cycles per second (one cycle every 2s)

    }

    // Update is called once per frame
    void Update()
    {
        // If oscillation is active, update the localScale.x to a sine-based value between -1 and 1.
        if (isOscillating)
        {
            oscTime += Time.deltaTime;
            // Use separate frequencies for flip and scale -> multiply by 2pi for radians
            float thetaFlip = oscTime * flipFrequency * 2f * Mathf.PI;
            float thetaScale = oscTime * scaleFrequency * 2f * Mathf.PI;
            float val = Mathf.Sin(thetaFlip); // ranges -1..1 for flip behaviour

            // scale oscillation factor (1 +/- amplitude)
            float scaleFactor = 1f + scaleOscAmplitude * Mathf.Sin(thetaScale);

            Vector3 s = transform.localScale;
            // apply flip * baseScale * scaleFactor
            float baseX = Mathf.Abs(originalScale.x);
            float baseY = originalScale.y;
            float baseZ = originalScale.z;
            s.x = baseX * val * scaleFactor; // may be negative to flip
            s.y = baseY * scaleFactor;
            s.z = baseZ * scaleFactor;

            // enforce minimum absolute scale so object never becomes too small
            if (Mathf.Abs(s.x) < minScale) s.x = Mathf.Sign(s.x == 0f ? 1f : s.x) * minScale;
            if (Mathf.Abs(s.y) < minScale) s.y = Mathf.Sign(s.y == 0f ? 1f : s.y) * minScale;
            if (Mathf.Abs(s.z) < minScale) s.z = Mathf.Sign(s.z == 0f ? 1f : s.z) * minScale;

            transform.localScale = s;
        }
    }
    public String getItemName()
    {
        return itemName;
    }

    // --- Oscillation support: flip/scale X between -1 and 1 over time ---
    private bool isOscillating = false;
    private float flipFrequency = 1f; // cycles per second for flip
    private float scaleFrequency = 1f; // cycles per second for scale pulsing
    private float oscTime = 0f;
    private Vector3 originalScale;
    private float scaleOscAmplitude = 0f; // fractional amplitude for scale oscillation (e.g. 0.2 = ±20%)
    // Minimum absolute scale for any axis while oscillating (inspector adjustable)
    public float minScale = 0.5f;

    // Start oscillating the localScale.x between -1 and 1. flipFrequency controls flipping speed.
    // Optionally provide a scale amplitude (fractional) to make the object pulse in size while oscillating.
    // scaleFrequency controls how fast the pulsing happens (independent from flipping).
    // Example: scaleAmplitude = 0.2 will make size vary between 0.8x and 1.2x around the base.
    public void StartOscillate(float flipFreq = 1f, float scaleAmplitude = 0f, float scaleFreq = 1f)
    {
        if (!isOscillating)
        {
            originalScale = transform.localScale;
            oscTime = 0f;
            flipFrequency = Mathf.Max(0f, flipFreq);
            scaleFrequency = Mathf.Max(0f, scaleFreq);
            scaleOscAmplitude = Mathf.Max(0f, scaleAmplitude);
            isOscillating = true;
        }
    }

    // Stop oscillation and restore X scale to its absolute original (positive) value.
    public void StopOscillate()
    {
        isOscillating = false;
        // restore original full scale (ensure X is positive)
        Vector3 s = originalScale;
        s.x = Mathf.Abs(s.x);
        transform.localScale = s;
    }
}
