using UnityEngine;

public class MovementScript : MonoBehaviour
{
    public string playerName = "Hero";

    public int health = 3;

    public Rigidbody2D rb;
    public Animator Animator;

    public float maxSpeed = 20f;
    public float kickForce = 1000f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
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
        rb.AddForce(transform.up * kickForce);
    }
}
