using UnityEngine;

/*
This is the abstract base tool class. 
*/

public abstract class BaseTool : MonoBehaviour
{
    [Header("Base")]
    [SerializeField] protected bool onCooldown;
    [SerializeField] protected Transform attachedObject;

    void Awake()
    {
        Instantiate();
    }

    public abstract void Instantiate();

    public abstract void Activate();

}
