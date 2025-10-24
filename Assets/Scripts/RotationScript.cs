using UnityEngine;
using UnityEngine.InputSystem;

public class RotationScript : MonoBehaviour
{

    public Camera mainCam;
    public bool isEnabled = true;
    private Vector2 movementInput;

    void Start()
    {

    }

    void Update()
    {
        if (isEnabled)
        {
            rotate();
        }



    }

    public void rotate()
    {
        // Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        // Vector3 direction = mousePos - transform.position;
        // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 270f;
        // transform.rotation = Quaternion.Euler(0f, 0f, angle);
        // Use left stick input for rotation if available
        if (movementInput.sqrMagnitude > 0.01f)
        {
            float stickAngle = Mathf.Atan2(movementInput.y, movementInput.x) * Mathf.Rad2Deg + 270f;
            transform.rotation = Quaternion.Euler(0f, 0f, stickAngle);
        }
    }
    public void OnMove(InputAction.CallbackContext ctx)
    {
        // if input action is left stick


        movementInput = ctx.ReadValue<Vector2>();
    }


}
