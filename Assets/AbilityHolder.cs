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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetCurrentHeldAbility(int index)
    {
        // get the sprite renderers of all abilities and disable them except the current one
        for (int i = 0; i < abilities.Length; i++)
        {
            SpriteRenderer sr = abilities[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                if (i == index)
                {
                    sr.enabled = true;
                }
                else
                {
                    sr.enabled = false;
                }
            }
        }


    }
}
