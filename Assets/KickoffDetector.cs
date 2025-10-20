using UnityEngine;

public class KickoffDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private LayerMask validLayers;
    [SerializeField] private bool debug = false;

    private bool kickoffPossible = false;
    private int overlapCount = 0;

    public bool GetKickoffPossible() => kickoffPossible;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & validLayers) != 0)
        {
            overlapCount++;
            kickoffPossible = true;

            if (debug)
            {
                Debug.Log($"[KickoffDetector] Entered: {other.gameObject.name} (layer: {LayerMask.LayerToName(other.gameObject.layer)})");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & validLayers) != 0)
        {
            overlapCount--;
            kickoffPossible = overlapCount > 0;

            if (debug)
            {
                Debug.Log($"[KickoffDetector] Exited: {other.gameObject.name} (layer: {LayerMask.LayerToName(other.gameObject.layer)})");
            }
        }
    }

    private void Update()
    {
        if (debug)
        {
            Debug.Log($"[KickoffDetector] Kickoff Possible: {kickoffPossible} | Overlaps: {overlapCount}");
        }
    }
}
