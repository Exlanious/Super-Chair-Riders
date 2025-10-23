using System.Collections.Generic;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private string itemTag = "Item";
    [SerializeField] private bool debugLog = true;

    // Boolean states for unique items
    [SerializeField] private bool hasSniper;
    [SerializeField] private bool hasSoda;
    [SerializeField] private bool hasPlunger;

    public bool HasSniper => hasSniper;
    public bool HasSoda => hasSoda;
    public bool HasPlunger => hasPlunger;
    // References for applying effects
    [SerializeField] private MovementScript movement;
    [SerializeField] private int healthBoost = 1;
    [SerializeField] private float speedBoost = 5f;
    [SerializeField] private float speedDuration = 5f;


    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;

        if (((1 << obj.layer) & itemLayer) == 0) return;
        if (!obj.CompareTag(itemTag)) return;

        string itemName = obj.name;

        switch (itemName)
        {
            case "Sniper":
                hasSniper = true;
                break;

            case "Soda":
                hasSoda = true;
                break;

            case "Plunger":
                hasPlunger = true;
                break;

            case "Health":
                if (movement != null)
                {
                    movement.health += healthBoost;
                    if (debugLog) Debug.Log("[ItemCollector] Health increased!");
                }
                break;

            case "Speed":
                if (movement != null)
                {
                    StartCoroutine(TempSpeedBoost());
                    if (debugLog) Debug.Log("[ItemCollector] Speed boost applied!");
                }
                break;

            default:
                if (debugLog) Debug.Log($"[ItemCollector] Unknown item: {itemName}");
                break;
        }

        if (debugLog) Debug.Log($"[ItemCollector] Picked up: {itemName}");
        Destroy(obj);
    }

    private System.Collections.IEnumerator TempSpeedBoost()
    {
        float originalSpeed = movement.maxSpeed;
        movement.maxSpeed += speedBoost;
        yield return new WaitForSeconds(speedDuration);
        movement.maxSpeed = originalSpeed;
    }
}
