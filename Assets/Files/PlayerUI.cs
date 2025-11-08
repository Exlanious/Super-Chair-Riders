using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public List<Image> hearts = new List<Image>();
    public List<Image> spds = new List<Image>();
    public MovementScript playerMov;

    void Start()
    {
        foreach (Image heart in hearts)
        {
            heart.enabled = false;
        }
        foreach (Image spd in spds)
        {
            spd.enabled = false;
        }
    }

    void Update()
    {
        if (playerMov.health >= hearts.Count)
        {
            Debug.LogError("PLayer health exceeds max health allowed by UI");
        }
        else
        {
            for (int i = 0; i < playerMov.health; i++)
            {
                hearts[i].enabled = true;
            }
            for (int j = playerMov.health; j < hearts.Count; j++)
            {
                hearts[j].enabled = false;
            }
        }
        /* HOW DOES THE SPD MODIFIER WORK?
                if (playerMov.GetSpeed() >= spds.Count)
                {
                    Debug.LogError("PLayer health exceeds max health allowed by UI");
                }
                else
                {
                    for (int i = 0; i < playerMov.GetSpeed(); i++)
                    {
                        spds[i].enabled = true;
                    }
                    for (int j = playerMov.GetSpeed(); j < spds.Count; j++)
                    {
                        spds[j].enabled = false;
                    }
                }
        */
    }
}
