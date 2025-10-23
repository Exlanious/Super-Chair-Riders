using Unity.VisualScripting;
using UnityEngine;

public class KickoffDetector : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private LayerMask validEnvironmentLayers;
    [SerializeField] private LayerMask validCombatLayers;
    [SerializeField] private bool debug = false;

    // Reference to parent's MovementScript
    private MovementScript movementScript;
    public string playerId;

    void Start()
    {
        movementScript = GetComponentInParent<MovementScript>();
        playerId = movementScript.GetPlayerId();
    }

    private bool kickoffPossible = false;
    private int overlapCount = 0;

    public bool GetKickoffPossible() => kickoffPossible;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & validEnvironmentLayers) != 0)
        {
            overlapCount++;
            kickoffPossible = true;

            if (debug)
            {
                //Debug.Log($"[KickoffDetector] Entered: {other.gameObject.name} (layer: {LayerMask.LayerToName(other.gameObject.layer)})");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & validEnvironmentLayers) != 0)
        {
            overlapCount--;
            kickoffPossible = overlapCount > 0;

            if (debug)
            {
                //Debug.Log($"[KickoffDetector] Exited: {other.gameObject.name} (layer: {LayerMask.LayerToName(other.gameObject.layer)})");
            }
        }
    }

    private void Update()
    {
        if (debug)
        {
            //Debug.Log($"[KickoffDetector] Kickoff Possible: {kickoffPossible} | Overlaps: {overlapCount}");
        }

        Collider2D[] overlaps = Physics2D.OverlapCircleAll(transform.position, 1f, validCombatLayers);
        // Also print amount of overlapping combat objects
        if (debug)
        {
            //Debug.Log($"[KickoffDetector] Overlapping Combat Objects Count: {overlaps.Length}");

        }

        foreach (var overlap in overlaps)
        {
            if (debug)
            {
                //Debug.Log($"[KickoffDetector] Overlapping Combat Object: {overlap.gameObject.name} (layer: {LayerMask.LayerToName(overlap.gameObject.layer)})");
            }

        }
    }

    // Transitions any players inside the hitbox into the hurt state.
    public void Apply_Damage()
    {
        Debug.Log("Damage was applied to all inside.");
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(transform.position, 1f, validCombatLayers);
        foreach (var overlap in overlaps)
        {
            Debug.Log("Attempted to run ReceiveDamage()");
            // gets component attatched to game object that is PlayerHurtbox.cs script
            var player_hurtbox_script = overlap.gameObject.GetComponent<PlayerHurtbox>();
            if (player_hurtbox_script != null && player_hurtbox_script.GetPlayerId() != playerId)
            {
                Debug.Log("Other enemy hitbox found, receiving dmg.");
                // A vector 2 that contains the global position of KickoffDetector
                player_hurtbox_script.ReceiveDamage(movementScript.GetKickDirection());
            }
            else
            {
                Debug.Log("No PlayerHurtbox script found or same playerId.");
            }
        }
    }
}
