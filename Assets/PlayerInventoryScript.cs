using UnityEngine;

public class PlayerInventoryScript : MonoBehaviour
{
    public string[] inventory = new string[1]; // inventory with 1 slot
    public AbilityHolder abilityHolderObject;
    // Use IsHolding() to check whether the inventory has an item (single-slot inventory)
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ItemRemove();

    }

    // Update is called once per frame
    void Update()
    {


    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        GameObject obj = other.gameObject;
        CollectableItemScript itemScript = obj.GetComponent<CollectableItemScript>();
        if (itemScript != null)
        {
            string itemName = itemScript.getItemName();
            ItemPickup(itemName);
            Destroy(obj); // remove the collected item from the scene
        }
    }

    void ItemPickup(string itemName)
    {
        inventory[0] = itemName;
        // if the string is "plunger", "soda", or "sniper", swap to that ability
        // AWFUL CODE IM SORRY
        if (itemName == "plunger")
        {
            SwapToPlunger();
        }
        else if (itemName == "soda")
        {
            SwapToSoda();
        }
        else if (itemName == "sniper")
        {
            SwapToSniper();
        }
    }

    void ItemRemove()
    {
        inventory[0] = "";
        SwapToEmpty();
    }

    // Returns true if any inventory slot contains a non-empty item name.
    public bool IsHolding()
    {
        if (inventory == null || inventory.Length == 0) return false;
        return !string.IsNullOrEmpty(inventory[0]);
    }

    public void SwapToEmpty()
    {
        abilityHolderObject.ClearHeldAbility(); // disable all ability sprites
    }

    public void SwapToPlunger()
    {
        abilityHolderObject.SetCurrentHeldAbility(0);
    }

    public void SwapToSoda()
    {
        abilityHolderObject.SetCurrentHeldAbility(1);
    }

    public void SwapToSniper()
    {
        abilityHolderObject.SetCurrentHeldAbility(2);
    }



    // public void SwapToPlunger()
    // {
    //     AbilityHolder abilityHolder = GetComponent<AbilityHolder>();
    //     if (abilityHolder != null)
    //     {
    //         abilityHolder.SetCurrentHeldAbility(0);
    //         abilityHolder.UpdateHoldingSprite();
    //     }
    // }
}
