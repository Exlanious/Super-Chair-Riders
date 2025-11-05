using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFocus : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private List<Transform> centerInstances;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 20f;
    [SerializeField] private float zoomSpeed = 5f;
    [SerializeField] private float padding = 2f;

    [Tooltip("Top-left world border (x, y)")]
    [SerializeField] private Vector2 TLBorder;

    [Tooltip("Bottom-right world border (x, y)")]
    [SerializeField] private Vector2 BRBorder;

    [Header("References")]
    [SerializeField] private Camera cam;

    [Header("Runtime (debug)")]
    public Vector2 center;
    public Bounds bounds;

    private void Awake()
    {
        if (cam == null)
            cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (centerInstances == null || centerInstances.Count == 0)
            return;

        // Compute bounds that include all targets
        bounds = GetBounds(centerInstances);

        // Calculate center point
        center = bounds.center;

        // Compute the required orthographic size (zoom)
        float targetZoom = GetRequiredZoom(bounds);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, Time.deltaTime * zoomSpeed);

        // Clamp camera position so it doesn’t go outside the world borders
        Vector3 clampedPos = ClampCameraToBorders(center, cam.orthographicSize);
        cam.transform.position = new Vector3(clampedPos.x, clampedPos.y, cam.transform.position.z);
    }

    private Bounds GetBounds(List<Transform> targets)
    {
        var b = new Bounds(targets[0].position, Vector3.zero);
        foreach (var t in targets)
            b.Encapsulate(t.position);
        return b;
    }

    private float GetRequiredZoom(Bounds b)
    {
        float verticalSize = b.size.y / 2f + padding;
        float horizontalSize = (b.size.x / 2f) / cam.aspect + padding;
        float requiredSize = Mathf.Max(verticalSize, horizontalSize);
        return Mathf.Clamp(requiredSize, minZoom, maxZoom);
    }

    private Vector3 ClampCameraToBorders(Vector2 targetCenter, float orthoSize)
    {
        float camHeight = orthoSize;
        float camWidth = orthoSize * cam.aspect;

        float minX = TLBorder.x + camWidth;
        float maxX = BRBorder.x - camWidth;
        float minY = BRBorder.y + camHeight;
        float maxY = TLBorder.y - camHeight;

        float clampedX = Mathf.Clamp(targetCenter.x, minX, maxX);
        float clampedY = Mathf.Clamp(targetCenter.y, minY, maxY);

        return new Vector3(clampedX, clampedY, 0);
    }
}
