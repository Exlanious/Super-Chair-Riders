using System;
using UnityEngine;
/*
    Attach this object anything that should be clickable by the mouse to trigger behavior.
*/
public class Clickable : MonoBehaviour
{
    public Action OnClick;
    public bool clickable = true;

    public void TriggerClick()
    {
        if (clickable)
            OnClick?.Invoke();
    }
}
/*
    subscribe to the OnClick action in the script that executes the clickable code. 
    Ex. 
    clickable = GetComponent<Clickable>();
    clickable.OnClick += handleClick;
*/