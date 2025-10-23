using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))] //this is needed for the test tool. Change if not needed. 
public class PlayerToolBehaviour : MonoBehaviour
{
    [Header("Settings")]
    //this should be a child of the player. 
    public BaseTool activeToolPrefab;

    [Header("Runtime")]
    private BaseTool activeTool;

    void Start()
    {
        activeTool = Instantiate(activeToolPrefab, transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            activeTool.Activate();
    }
}
