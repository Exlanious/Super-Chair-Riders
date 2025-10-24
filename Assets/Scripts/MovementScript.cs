using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour
{
    public bool enabled = true;
    public string playerId = "Hero";
    public int health = 3;

    public Rigidbody2D rb;
    public Animator Animator;

    public float maxSpeed = 20f;
    public float unchargedKickForce = 300f;
    public float chargedKickForce = 1000f;
    public float kickOffMultiplier = 1.5f;

    public float defaultDamping = 0.3f;
    public float chargedDamping = 1.0f;

    private bool isCharged = false;
    private KickoffDetector kickoffDetector;

    [Header("Drift Settings")]
    public float driftForce = 100f;
    public float requiredDriftSpeed = 3f;
    public float driftBuildUpRate = 3f;     // How quickly drift strength builds (higher = faster)
    public float driftDecayRate = 5f;       // How quickly it fades when released
    public float maxDriftMultiplier = 1f;   // Maximum drift strength multiplier

    private float currentDriftMultiplier = 0f;

    void Start()
    {
        kickoffDetector = GetComponentInChildren<KickoffDetector>();
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    void Update()
    {

        if (enabled) inputCheck();
    }

    void inputCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetUncharged();
            Animator.ResetTrigger("ReleaseCharge");
            Animator.SetTrigger("StartCharge");
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Animator.ResetTrigger("StartCharge");
            Animator.SetTrigger("ReleaseCharge");
        }
    }

    void defaultDamp()
    {
        rb.linearDamping = defaultDamping;
    }

    void chargedDamp()
    {
        rb.linearDamping = chargedDamping;
    }

    void FixedUpdate()
    {
        // Cap speed
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;

        // Skip if nearly stationary
        if (rb.linearVelocity.sqrMagnitude < requiredDriftSpeed * requiredDriftSpeed)
        {
            currentDriftMultiplier = Mathf.MoveTowards(currentDriftMultiplier, 0f, driftDecayRate * Time.fixedDeltaTime);
            return;
        }

        // Get direction
        Vector2 velDir = rb.linearVelocity.normalized;
        Vector2 velRight = new Vector2(velDir.y, -velDir.x);
        Vector2 velLeft = new Vector2(-velDir.y, velDir.x);

        bool driftLeft = Input.GetKey(KeyCode.A);
        bool driftRight = Input.GetKey(KeyCode.D);

        if (driftLeft || driftRight)
        {
            // Gradually build up drift multiplier
            currentDriftMultiplier = Mathf.MoveTowards(currentDriftMultiplier, maxDriftMultiplier, driftBuildUpRate * Time.fixedDeltaTime);

            Vector2 driftDir = driftLeft ? velLeft : velRight;
            rb.AddForce(driftDir * driftForce * currentDriftMultiplier * Time.fixedDeltaTime, ForceMode2D.Force);
        }
        else
        {
            // Decay drift strength when not holding drift keys
            currentDriftMultiplier = Mathf.MoveTowards(currentDriftMultiplier, 0f, driftDecayRate * Time.fixedDeltaTime);
        }
    }

    public void PhysicsKick()
    {
        var kickOffForce = (isCharged ? chargedKickForce : unchargedKickForce);
        if (kickoffDetector.GetKickoffPossible())
            kickOffForce *= kickOffMultiplier;
        rb.AddForce(transform.up * kickOffForce);
    }

    public float GetSpeed() => rb.linearVelocity.magnitude;

    public void SetCharged() => isCharged = true;
    public void SetUncharged() => isCharged = false;
    public bool GetKickoffPossible() => kickoffDetector.GetKickoffPossible();

    public void KickApplyDamage()
    {
        kickoffDetector.Apply_Damage();
    }

    public string GetPlayerId()
    {
        return playerId;
    }

    public Vector2 GetKickDirection()
    {
        return -transform.up;
    }

    public void TakeDamage(Vector2 AttackDirection)
    {
        Debug.Log("MovementScript: Taking damage!");
        // Apply knockback
        rb.AddForce(AttackDirection * 500f, ForceMode2D.Impulse);
        health -= 1;
        Debug.Log("Health remaining: " + health);
        Animator.SetTrigger("StartHurt");
    }

    public void OnKick(InputAction.CallbackContext ctx)
    {
        Debug.Log("Kick input received");
        if (ctx.performed)
        {
            // Button pressed (like GetKeyDown)
            SetUncharged();
            Animator.ResetTrigger("ReleaseCharge");
            Animator.SetTrigger("StartCharge");
        }

        if (ctx.canceled)
        {
            // Button released (like GetKeyUp)
            Animator.ResetTrigger("StartCharge");
            Animator.SetTrigger("ReleaseCharge");
        }
    }
}