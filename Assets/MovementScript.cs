using UnityEngine;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour
{
    public string playerName = "Hero";

    public int health = 3;

    public Rigidbody2D rb;
    public Animator Animator;

    public float maxSpeed = 20f;
    public float unchargedKickForce = 300f;
    public float chargedKickForce = 1000f;
    public float kickOffMultiplier = 1.5f;

    private bool isCharged = false;

    private KickoffDetector kickoffDetector;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        kickoffDetector = GetComponentInChildren<KickoffDetector>();
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
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

    void LateUpdate()
    {

    }

    void FixedUpdate()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
    }


    public void PhysicsKick()
    {
        var kickOffForce = (isCharged ? chargedKickForce : unchargedKickForce);
        if (kickoffDetector.GetKickoffPossible())
            kickOffForce *= kickOffMultiplier;
        rb.AddForce(transform.up * kickOffForce);
        print(isCharged ? "Charged Kick!" : "Uncharged Kick!");
        print("Kickoff Executed: " + kickoffDetector.GetKickoffPossible());
    }

    public float GetSpeed()
    {
        return rb.linearVelocity.magnitude;
    }

    public void SetCharged()
    {
        isCharged = true;
    }

    public void SetUncharged()
    {
        isCharged = false;
    }

    public bool GetKickoffPossible()
    {
        return kickoffDetector.GetKickoffPossible();
    }

}
