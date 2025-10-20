using UnityEngine;

public class KickoffDetector : MonoBehaviour
{
    private bool kickoffPossible;

    public bool GetKickoffPossible() => kickoffPossible;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody != null)
            kickoffPossible = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.attachedRigidbody != null)
            kickoffPossible = false;
    }


    void Update()
    {
        Debug.Log("Kickoff Possible: " + kickoffPossible);

    }
}
