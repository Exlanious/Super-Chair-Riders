using UnityEngine;

public class PlayerHurtbox : MonoBehaviour
{
    // Reference to parent's MovementScript
    private MovementScript movementScript;
    public string playerId;

    void Start()
    {
        movementScript = GetComponentInParent<MovementScript>();
        playerId = movementScript.GetPlayerId();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Method
    public void ReceiveDamage(Vector2 AttackDirection)
    {
        Debug.Log("Me: Hurtbox received damage!");
        // In direction
        Debug.Log("Attack Direction: " + AttackDirection);
        movementScript.TakeDamage(AttackDirection);
    }
    public string GetPlayerId()
    {
        return playerId;
    }
}
