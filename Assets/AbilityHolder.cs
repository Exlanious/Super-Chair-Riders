using UnityEngine;

public class AbilityHolder : MonoBehaviour
{
    // reference for holding child that can be assigned in the editor, held in one array

    public GameObject plunger; // index 0
    public GameObject soda; // index 1
    public GameObject sniper; // index 2

    public GameObject[] abilities;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        abilities = new GameObject[] { plunger, soda, sniper };
        // set the ability to soda

    }

    // Update is called once per frame
    void Update()
    {

    }

    // method to disable all abilities, since inventory is empty
    public void ClearHeldAbility()
    {
        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i] != null)
            {
                abilities[i].SetActive(false);
            }
        }
    }

    public void SetCurrentHeldAbility(int index)
    {
        // get the sprite renderers of all abilities and disable them except the current one
        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i] != null)
            {
                abilities[i].SetActive(i == index);
            }
        }


    }
}
