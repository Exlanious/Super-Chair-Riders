using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFocus : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<Transform> centerInstances; // Objects the camera should keep in view
    [SerializeField] private float minZoom = 5f;              // Minimum orthographic size (zoom-in limit)
    [SerializeField] private float maxZoom = 20f;             // Maximum orthographic size (zoom-out limit)
    [SerializeField] private float zoomSpeed = 5f;            // Speed at which zoom adjusts to target size

    [Header("Directional Padding")]
    [SerializeField] private float paddingTop = 2f;           
    [SerializeField] private float paddingBottom = 2f;       
    [SerializeField] private float paddingLeft = 2f;          
    [SerializeField] private float paddingRight = 2f;         

    [Header("World Borders")]
    [Tooltip("Top-left world border (x, y)")]
    [SerializeField] private Vector2 TLBorder;                // The top-left world boundary (map edge)
    [Tooltip("Bottom-right world border (x, y)")]
    [SerializeField] private Vector2 BRBorder;                // The bottom-right world boundary (map edge)

    [Header("References")]
    [SerializeField] private Camera cam;                     

    [Header("Runtime (debug)")]
    public Vector2 center;                                   // Current calculated center position
    public Bounds bounds;                                    // Current bounding box of all tracked objects

    private void Awake()
    {
        if (cam == null)
            cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (centerInstances == null || centerInstances.Count == 0)
            return;

        // Compute the bounding box that contains all tracked objects
        bounds = GetBounds(centerInstances);

        // Calculate center position of all tracked objects
        center = bounds.center;

        // Compute target orthographic size (zoom level) based on object spread + padding
        float targetZoom = GetRequiredZoom(bounds);

        // Smoothly interpolate current zoom toward target zoom
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

        // Clamp camera position so that visible area stays within map borders
        Vector3 clampedPos = ClampCameraToBorders(center, cam.orthographicSize);

        // Update camera position (preserving Z depth)
        cam.transform.position = new Vector3(clampedPos.x, clampedPos.y, cam.transform.position.z);
    }

    // Calculates a bounding box that includes all tracked transforms.
    private Bounds GetBounds(List<Transform> targets)
    {
        var b = new Bounds(targets[0].position, Vector3.zero);
        foreach (var t in targets)
            b.Encapsulate(t.position);
        return b;
    }


    // Determines the orthographic size (zoom) required to fit all tracked objects on screen,
    // considering per-direction padding and camera aspect ratio.
    private float GetRequiredZoom(Bounds b)
    {
        // Add per-direction padding
        float verticalSize = (b.size.y / 2f) + Mathf.Max(paddingTop, paddingBottom);
        float horizontalSize = (b.size.x / 2f) / cam.aspect + Mathf.Max(paddingLeft, paddingRight);

        // Take the larger dimension as the required zoom size
        float requiredSize = Mathf.Max(verticalSize, horizontalSize);

        // Clamp to allowed zoom range
        return Mathf.Clamp(requiredSize, minZoom, maxZoom);
    }


    // Keeps the camera view within the defined map borders.
    // Prevents camera from showing outside world limits.
    private Vector3 ClampCameraToBorders(Vector2 targetCenter, float orthoSize)
    {
        float camHeight = orthoSize;
        float camWidth = orthoSize * cam.aspect;

        // Calculate the camera’s current edges in world space
        float leftEdge = targetCenter.x - camWidth;
        float rightEdge = targetCenter.x + camWidth;
        float topEdge = targetCenter.y + camHeight;
        float bottomEdge = targetCenter.y - camHeight;

        // Initialize clamped position with target center
        float clampedX = targetCenter.x;
        float clampedY = targetCenter.y;

        // Adjust X if the camera would show beyond left or right map edges
        if (leftEdge < TLBorder.x)
            clampedX += (TLBorder.x - leftEdge);
        else if (rightEdge > BRBorder.x)
            clampedX -= (rightEdge - BRBorder.x);

        // Adjust Y if the camera would show beyond top or bottom map edges
        if (topEdge > TLBorder.y)
            clampedY -= (topEdge - TLBorder.y);
        else if (bottomEdge < BRBorder.y)
            clampedY += (BRBorder.y - bottomEdge);

        return new Vector3(clampedX, clampedY, 0);
    }
}
