using UnityEngine;
using UnityEngine.InputSystem;

public class RotationScript : MonoBehaviour
{

    public Camera mainCam;
    public bool isEnabled = true;
    private Vector2 movementInput;
    public float rotationSpeed = 20f;

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
        if (movementInput.sqrMagnitude > 0.01f)
        {
            // Set stickAngle to angle of left stick input
            float stickAngle = Mathf.Atan2(movementInput.y, movementInput.x) * Mathf.Rad2Deg + 270f;

            // Lerp at an adjustable speed towards the target angle
            float currentZ = transform.rotation.eulerAngles.z;
            float newZ = Mathf.LerpAngle(currentZ, stickAngle, rotationSpeed * Time.deltaTime);

            transform.rotation = Quaternion.Euler(0f, 0f, newZ);
        }



    }
    public void OnMove(InputAction.CallbackContext ctx)
    {
        // if input action is left stick


        movementInput = ctx.ReadValue<Vector2>();
    }


}
