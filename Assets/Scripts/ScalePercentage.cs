using UnityEngine;

public class ScalePercentage : MonoBehaviour
{
    [System.Serializable]
    public enum Align
    {
        TOPLEFT, TOP, TOPRIGHT,
        LEFT, CENTER, RIGHT,
        BOTTOMLEFT, BOTTOM, BOTTOMRIGHT
    }

    [Header("Settings")]
    [SerializeField] private GameObject obj;
    [Range(0, 1)] public float percentage = 1;
    public bool scaleX = true;
    public bool scaleY = true;
    public Align alignment = Align.CENTER;

    private Vector3 defaultScale;
    private Vector3 defaultPosition;

    void Start()
    {
        if (obj != null)
        {
            defaultScale = obj.transform.localScale;
            defaultPosition = obj.transform.localPosition;
        }
    }

    void Update()
    {
        if (obj == null)
            return;

        // Scale calculation
        float newScaleX = scaleX ? defaultScale.x * percentage : defaultScale.x;
        float newScaleY = scaleY ? defaultScale.y * percentage : defaultScale.y;

        // Calculate offset based on alignment
        Vector3 scaleDelta = new Vector3(newScaleX - defaultScale.x, newScaleY - defaultScale.y, 0);
        Vector3 offset = Vector3.zero;

        switch (alignment)
        {
            case Align.TOPLEFT: offset = new Vector3(scaleDelta.x / 2, -scaleDelta.y / 2, 0); break;
            case Align.TOP: offset = new Vector3(0, -scaleDelta.y / 2, 0); break;
            case Align.TOPRIGHT: offset = new Vector3(-scaleDelta.x / 2, -scaleDelta.y / 2, 0); break;
            case Align.LEFT: offset = new Vector3(scaleDelta.x / 2, 0, 0); break;
            case Align.CENTER: offset = Vector3.zero; break;
            case Align.RIGHT: offset = new Vector3(-scaleDelta.x / 2, 0, 0); break;
            case Align.BOTTOMLEFT: offset = new Vector3(scaleDelta.x / 2, scaleDelta.y / 2, 0); break;
            case Align.BOTTOM: offset = new Vector3(0, scaleDelta.y / 2, 0); break;
            case Align.BOTTOMRIGHT: offset = new Vector3(-scaleDelta.x / 2, scaleDelta.y / 2, 0); break;
        }

        // Apply scale and position
        obj.transform.localScale = new Vector3(newScaleX, newScaleY, defaultScale.z);
        obj.transform.localPosition = defaultPosition + offset;
    }
}
