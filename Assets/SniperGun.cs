using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

[RequireComponent(typeof(EdgeCollider2D), typeof(LineRenderer))]
public class SniperLaser_Edge : MonoBehaviour
{
    [Header("Laser Settings")]
    [SerializeField] private float laserLength = 20f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float duration = 0.1f;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask environmentLayer;

    [Header("References")]
    [SerializeField] private Transform firePoint;

    [Header("Debug")]
    [SerializeField] private bool debug = true;

    private EdgeCollider2D edgeCollider;
    private LineRenderer lineRenderer;
    private bool isFiring = false;

    private void Awake()
    {
        edgeCollider = GetComponent<EdgeCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();

        edgeCollider.enabled = false;
        lineRenderer.enabled = false;
        edgeCollider.isTrigger = true;

        lineRenderer.positionCount = 2;
        lineRenderer.widthMultiplier = 0.15f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.textureMode = LineTextureMode.Stretch;

        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
            new GradientColorKey(new Color(0f, 0.4f, 1f), 0f),   
            new GradientColorKey(new Color(0.6f, 0.9f, 1f), 1f) 
            },
            new GradientAlphaKey[] {
            new GradientAlphaKey(1f, 0f),   
            new GradientAlphaKey(0.2f, 1f)  
            }
        );
        lineRenderer.colorGradient = gradient;
    }


    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && !isFiring)
        {
            Fire();
        }
    }

    public void Fire()
    {
        StartCoroutine(FireLaser());
    }

    private IEnumerator FireLaser()
    {
        isFiring = true;

        Vector2 origin = firePoint.position;
        Vector2 direction = firePoint.right;
        Vector2 end = origin + direction * laserLength;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, laserLength, environmentLayer);
        if (hit.collider != null)
        {
            end = hit.point;
        }

        // Visual line
        lineRenderer.SetPosition(0, origin);
        lineRenderer.SetPosition(1, end);
        lineRenderer.enabled = true;

        // Collider
        Vector2[] points = new Vector2[2];
        points[0] = transform.InverseTransformPoint(origin);
        points[1] = transform.InverseTransformPoint(end);
        edgeCollider.points = points;
        edgeCollider.enabled = true;

        if (debug)
        {
            Debug.DrawLine(origin, end, Color.red, duration);
            Debug.Log($"[SniperLaser_Edge] Fired from {origin} to {end}");
        }

        yield return new WaitForSeconds(duration);

        lineRenderer.enabled = false;
        edgeCollider.enabled = false;
        isFiring = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isFiring) return;

        if (((1 << other.gameObject.layer) & playerLayer) == 0) return;

        if (other.CompareTag("Player") && other.gameObject != gameObject)
        {
            if (other.TryGetComponent(out MovementScript target))
            {
                target.health -= Mathf.RoundToInt(damage);
                if (debug)
                    Debug.Log($"[SniperLaser_Edge] Hit {other.name} ¡ú -{damage} HP");
            }
        }
    }
}
