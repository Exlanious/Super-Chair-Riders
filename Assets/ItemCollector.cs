using System.Collections;
using UnityEngine;

public class ItemCollector : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private string itemTag = "Item";
    [SerializeField] private bool debugLog = true;

    [Header("Item Prefabs (Drag from Inspector)")]
    [SerializeField] private GameObject sniperPrefab;
    [SerializeField] private GameObject sodaPrefab;
    [SerializeField] private GameObject plungerPrefab;

    // Item state tracking
    [SerializeField] private bool hasSniper;
    [SerializeField] private bool hasSoda;
    [SerializeField] private bool hasPlunger;

    public bool HasSniper => hasSniper;
    public bool HasSoda => hasSoda;
    public bool HasPlunger => hasPlunger;

    [Header("Effect Settings")]
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

    private IEnumerator TempSpeedBoost()
    {
        float originalSpeed = movement.maxSpeed;
        movement.maxSpeed += speedBoost;
        yield return new WaitForSeconds(speedDuration);
        movement.maxSpeed = originalSpeed;
    }

    // Drop held item when kicked
    public void DropCurrentItem()
    {
        Vector2 dropPosition = transform.position;
        GameObject itemToDrop = null;

        if (hasSniper)
        {
            itemToDrop = sniperPrefab;
            hasSniper = false;
        }
        else if (hasSoda)
        {
            itemToDrop = sodaPrefab;
            hasSoda = false;
        }
        else if (hasPlunger)
        {
            itemToDrop = plungerPrefab;
            hasPlunger = false;
        }

        if (itemToDrop != null)
        {
            GameObject dropped = Instantiate(itemToDrop, dropPosition, Quaternion.identity);

            // Add bounce force
            Rigidbody2D rb = dropped.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 randomDir = Random.insideUnitCircle.normalized;
                rb.AddForce(randomDir * 3f, ForceMode2D.Impulse);
            }

            // Temporarily disable pickup
            Collider2D col = dropped.GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
                StartCoroutine(ReenablePickup(col, 1f));
            }

            if (debugLog) Debug.Log($"[ItemCollector] Dropped: {itemToDrop.name}");
        }
    }

    private IEnumerator ReenablePickup(Collider2D collider, float delay)
    {
        yield return new WaitForSeconds(delay);
        collider.enabled = true;
    }
}
