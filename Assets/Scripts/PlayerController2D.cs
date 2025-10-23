using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 500f;

    [Header("Kick Settings")]
    [SerializeField] private KickHitbox kickHitbox;
    [SerializeField] private Transform headDirection;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float kickChargeStart;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ReadInput();
        RotateHead();
        HandleKickInput();
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }

    private void ReadInput()
    {
        moveInput = Vector2.zero;

        if (Keyboard.current.wKey.isPressed) moveInput.y += 1;
        if (Keyboard.current.sKey.isPressed) moveInput.y -= 1;
        if (Keyboard.current.dKey.isPressed) moveInput.x += 1;
        if (Keyboard.current.aKey.isPressed) moveInput.x -= 1;

        // Optional: normalize diagonal speed
        moveInput = moveInput.normalized;
    }

    private void HandleKickInput()
    {
        var key = Keyboard.current.leftCtrlKey;

        if (key.wasPressedThisFrame)
        {
            kickChargeStart = Time.time;
        }
        else if (key.wasReleasedThisFrame)
        {
            float charge = Mathf.Clamp01(Time.time - kickChargeStart);
            kickHitbox.Activate(); 
        }
    }

    private void RotateHead()
    {
        if (moveInput == Vector2.zero) return;

        float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        headDirection.rotation = Quaternion.RotateTowards(
            headDirection.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}
