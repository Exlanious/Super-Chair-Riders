using UnityEngine;

public class CharacterSpritesScript : MonoBehaviour
{
    // Simple sprite swapper: expose four named sprites and methods to display them.
    [Header("Named sprites")]
    public Sprite idleSprite;
    public Sprite kickSprite;
    public Sprite holdSprite;
    public Sprite hurtSprite;

    // SpriteRenderer to display sprites. Assign in the inspector to override automatic lookup.
    public SpriteRenderer spriteRenderer;

    // Currently selected sprite (stored when SetSprite is called). Applied to the SpriteRenderer in LateUpdate.
    public Sprite currentSprite;

    void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer == null)
        {
            // no sprite renderer assigned/found — silently continue
        }

        // Try to set the initial sprite to the idle sprite (if assigned)
        // Use SetSprite so currentSprite is stored and LateUpdate will apply it.
        SetSprite(idleSprite);
    }



    // internal helper: store the sprite in currentSprite. LateUpdate will apply it to the renderer.
    private void SetSprite(Sprite s)
    {
        // allow null to clear the sprite
        currentSprite = s;
    }

    // Apply the stored sprite to the SpriteRenderer each frame (LateUpdate ensures other updates finished).
    private void LateUpdate()
    {
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) return;

        if (spriteRenderer.sprite != currentSprite)
        {
            spriteRenderer.sprite = currentSprite;
        }
    }

    // Public methods the user requested: swap to the named sprites

    public void ShowIdle() => SetSprite(idleSprite);
    public void ShowKick() => SetSprite(kickSprite);
    public void ShowHold() => SetSprite(holdSprite);
    public void ShowHurt() => SetSprite(hurtSprite);

    // (No index-based access required for this simplified script.)

    // Editor-time changes should reflect on the SpriteRenderer for quick iteration
    // private void OnValidate()
    // {
    //     if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
    //     if (spriteRenderer == null) return;

    //     if (idleSprite != null)
    //     {
    //         spriteRenderer.sprite = idleSprite;
    //     }
    // }

    // Keep empty Start/Update in case the project expects them; no behaviour needed here
    void Start()
    {

    }

    void Update()
    {

    }
}
