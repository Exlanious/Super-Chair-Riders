using UnityEngine;

public class RotationScript : MonoBehaviour
{
    public Camera mainCam;
    public bool enabled = true;

    void Start()
    {

    }

    void Update()
    {
        if (enabled)
        {
            rotate();
        }

    }

    public void rotate()
    {
        Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 270f;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

    }

}
