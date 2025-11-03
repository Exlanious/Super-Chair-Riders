using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour
{
    public string[] inventory = new string[1];
    //  set to soda for testing

    public bool isEnabled = true;
    public string playerId = "Hero";
    public int health = 3;
    public int maxHealth = 3;

    public Rigidbody2D rb;
    public Animator Animator;

    public float maxSpeed = 20f;
    [Header("Kick Forces")]
    public float unchargedKickForce = 300f;
    public float chargedKickForce = 1000f;
    // Forces to use when a kickoff is possible (separate values for charged vs uncharged)
    public float unchargedKickOffForce = 450f;
    public float chargedKickOffForce = 1500f;

    // Multiplies all kick forces. Can be changed at runtime and reset to the original value.
    public float BoostedKickMultiplier = 1f;
    private float defaultBoostedKickMultiplier = 1f;

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

    // Soda boost (applies a constant acceleration in transform.up for a duration)
    // Acceleration is in world units (m/s^2). The code applies force = acceleration * mass
    public float sodaBoostAcceleration = 20f; // default acceleration applied while boosted
    public float sodaBoostDuration = 1f;      // default duration in seconds
    private float sodaBoostTimeRemaining = 0f;

    void Start()
    {
        // FOR TESTING SET INVENTORY TO SODA
        inventory[0] = "Soda";

        kickoffDetector = GetComponentInChildren<KickoffDetector>();
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
        // store original multiplier so Reset can restore it
        defaultBoostedKickMultiplier = BoostedKickMultiplier;

        // Ensure health is within the allowed range at start
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    // Apply the soda boost using default configured values.
    public void ApplySodaBoostDefault()
    {
        ApplySodaBoost(sodaBoostAcceleration, sodaBoostDuration);
    }

    /// <summary>
    /// Apply a soda boost that accelerates the player in transform.up by the given acceleration (m/s^2)
    /// for the given duration (seconds). This method is safe to call from other scripts.
    /// </summary>
    public void ApplySodaBoost(float acceleration, float duration)
    {
        sodaBoostAcceleration = acceleration;
        sodaBoostTimeRemaining = Mathf.Max(0f, duration);
    }

    void Update()
    {

        if (isEnabled) inputCheck();
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

        // Apply soda boost acceleration (if active). Use Time.fixedDeltaTime to account for fixed-step timing.
        if (sodaBoostTimeRemaining > 0f)
        {
            // Apply force to produce the desired acceleration: F = m * a
            if (rb != null)
            {
                rb.AddForce(transform.up * sodaBoostAcceleration * rb.mass, ForceMode2D.Force);
            }
            sodaBoostTimeRemaining -= Time.fixedDeltaTime;
        }

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
        // Choose base force depending on charged state and whether a kickoff is possible
        float forceToApply = isCharged ? chargedKickForce : unchargedKickForce;
        if (kickoffDetector != null && kickoffDetector.GetKickoffPossible())
        {
            forceToApply = isCharged ? chargedKickOffForce : unchargedKickOffForce;
        }

        // Apply global boosted multiplier
        forceToApply *= BoostedKickMultiplier;

        rb.AddForce(transform.up * forceToApply);
    }

    /// <summary>
    /// Set a runtime multiplier applied to all kicks.
    /// </summary>
    public void SetBoostedKickMultiplier(float multiplier)
    {
        BoostedKickMultiplier = multiplier;
    }

    /// <summary>
    /// Reset the boosted multiplier to the original value (captured at Start).
    /// </summary>
    public void ResetBoostedKickMultiplier()
    {
        BoostedKickMultiplier = defaultBoostedKickMultiplier;
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
        // Apply knockback
        rb.AddForce(AttackDirection * 500f, ForceMode2D.Impulse);

        // Decrease health (clamped at zero)
        health = Mathf.Max(0, health - 1);

        Animator.SetTrigger("StartHurt");
    }

    // Increase the player's max health by 1 and also increase current health by 1.
    public void IncreaseMaxHealth()
    {
        maxHealth += 1;
        health += 1;
        // keep health within bounds
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    public void OnKick(InputAction.CallbackContext ctx)
    {
        //Debug.Log("Kick input received");
        if (ctx.performed)
        {
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