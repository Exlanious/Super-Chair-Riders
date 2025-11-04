using UnityEngine;

public class CharacterSpritesHelper : MonoBehaviour
{
    // This class allows the four methods to be called. This script is attatched to player, and CharacterSpritesScript is attatched to the child object "Sprite2D".
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    // Optional: assign in inspector or the helper will try to find the child named "Sprite2D" at Awake
    public CharacterSpritesScript spriteScript;

    void Awake()
    {
        if (spriteScript == null)
        {
            Transform child = transform.Find("Sprite2D");
            if (child != null)
            {
                spriteScript = child.GetComponent<CharacterSpritesScript>();
            }
        }

        if (spriteScript == null)
        {
        }
    }

    // Public pass-through methods. These can be called from other scripts, animation events, or UI buttons.
    public void ShowIdle()
    {
        if (spriteScript != null) spriteScript.ShowIdle();
    }

    public void ShowKick()
    {
        if (spriteScript != null) spriteScript.ShowKick();

    }
    public void ShowHold()
    {
        if (spriteScript != null) spriteScript.ShowHold();

    }

    public void ShowHurt()
    {
        if (spriteScript != null) spriteScript.ShowHurt();

    }

    // Keep Update in case you want per-frame behaviour later
    void Start()
    {

    }

    void Update()
    {

    }
}
