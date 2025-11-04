using UnityEngine;

public class CharacterSpritesScript : MonoBehaviour
{
    public PlayerInventoryScript playerInventory;

    // Simple sprite swapper: expose four named sprites and methods to display them.
    [Header("Named sprites")]
    public Sprite idleEmptySprite;
    public Sprite idleHoldSprite;

    public Sprite kickEmptySprite;
    public Sprite kickHoldSprite;

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

        // Try to set the initial sprite to the appropriate idle sprite (hold vs empty)
        // Use SetSprite so currentSprite is stored and LateUpdate will apply it.
        Sprite initial = (playerInventory != null && playerInventory.IsHolding()) ? idleHoldSprite : idleEmptySprite;
        SetSprite(initial);
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

    public void ShowIdle()
    {
        Sprite s = (playerInventory != null && playerInventory.IsHolding()) ? idleHoldSprite : idleEmptySprite;
        // debug message: player is holding / isnt
        Debug.Log("Player is " + (playerInventory.IsHolding() ? "holding" : "not holding") + " an item.");
        SetSprite(s);
    }

    public void ShowKick()
    {
        Sprite s = (playerInventory != null && playerInventory.IsHolding()) ? kickHoldSprite : kickEmptySprite;
        SetSprite(s);
    }

    // ShowHold behaves like ShowIdle for held/empty visuals (keeps consistency).
    public void ShowHold()
    {
        Sprite s = (playerInventory != null && playerInventory.IsHolding()) ? idleHoldSprite : idleEmptySprite;
        SetSprite(s);
    }

    public void ShowHurt() => SetSprite(hurtSprite);


    void Start()
    {

    }

    void Update()
    {

    }
}