using UnityEngine;

public class MouseManager : MonoBehaviour
{
    public static Vector2 mousePos;
    public static bool inputEnabled = true; // Toggle this to freeze mouse input

    void Update()
    {
        if (!inputEnabled)
            return; // Freeze all mouse interactions

        // Get mouse world position
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Handle left mouse button click
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null)
            {
                Clickable clickable = hit.collider.GetComponent<Clickable>();
                if (clickable != null)
                {
                    Debug.Log("Clicked on: " + hit.collider.gameObject.name);
                    clickable.TriggerClick();
                }
            }
        }
    }
}
