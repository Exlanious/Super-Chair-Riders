using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KickHitbox : MonoBehaviour
{
    [Header("Kick Settings")]
    [SerializeField] private float kickForce = 15f;
    [SerializeField] private float kickDuration = 0.2f;
    [SerializeField] private float hitboxTravelDistance = 1f;
    [SerializeField] private float hitboxTravelSpeed = 20f;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask playerLayer;

    private Collider2D hitbox;
    private Rigidbody2D ownerRb;
    private Vector3 initialLocalPosition;
    private bool isActive = false;
    private Coroutine activeRoutine;

    public event Action<GameObject> OnPlayerHit;
    public event Action OnWallHit;

    private void Awake()
    {
        hitbox = GetComponent<Collider2D>();
        hitbox.enabled = false;

        Transform root = transform.root;
        ownerRb = root.GetComponent<Rigidbody2D>();
        if (ownerRb == null)
        {
            Debug.LogError("KickHitbox: No Rigidbody2D found on root object.");
        }

        initialLocalPosition = transform.localPosition;
    }

    public void Activate()
    {
        if (activeRoutine != null) StopCoroutine(activeRoutine);
        activeRoutine = StartCoroutine(MoveHitbox());
    }

    private System.Collections.IEnumerator MoveHitbox()
    {
        isActive = true;
        hitbox.enabled = true;

        Vector3 direction = transform.right;
        Vector3 targetLocalPos = initialLocalPosition + direction * hitboxTravelDistance;

        float elapsed = 0f;
        while (elapsed < kickDuration)
        {
            transform.localPosition = Vector3.MoveTowards(
                transform.localPosition,
                targetLocalPos,
                hitboxTravelSpeed * Time.deltaTime
            );
            elapsed += Time.deltaTime;
            yield return null;
        }

        ResetHitbox();
    }

    private void ResetHitbox()
    {
        transform.localPosition = initialLocalPosition;
        hitbox.enabled = false;
        isActive = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;

        GameObject otherObj = other.gameObject;

        if (((1 << otherObj.layer) & wallLayer) != 0)
        {
            Vector2 directionAwayFromWall = (ownerRb.position - (Vector2)other.transform.position).normalized;
            ownerRb.AddForce(directionAwayFromWall * kickForce, ForceMode2D.Impulse);

            Debug.Log($"[KickHitbox] Hit wall. Sliding away.");
            OnWallHit?.Invoke();
        }
        else if (((1 << otherObj.layer) & playerLayer) != 0 && otherObj != gameObject)
        {
            Rigidbody2D otherRb = other.GetComponent<Rigidbody2D>();
            if (otherRb != null)
            {
                Vector2 dir = (otherRb.position - ownerRb.position).normalized;
                ownerRb.AddForce(-dir * kickForce, ForceMode2D.Impulse);
                otherRb.AddForce(dir * kickForce, ForceMode2D.Impulse);

                Debug.Log($"[KickHitbox] Player hit: {otherObj.name} by {gameObject.name}");
                OnPlayerHit?.Invoke(otherObj);
            }
        }

        StopAllCoroutines();
        ResetHitbox();
    }
}
