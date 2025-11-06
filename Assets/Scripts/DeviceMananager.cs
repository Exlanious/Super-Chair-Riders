using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class DeviceMananager : MonoBehaviour
{
    public bool checkConnectionStatus = false;
    public int controllerCount = 2;

    public bool debugMode = false;

    void Start()
    {
        CheckConnectedGamepads();
    }

    void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    public void CheckConnectedGamepads()
    {
        // Get a list of all gamepads currently connected
        var gamepads = Gamepad.all;

        if (debugMode)
            Debug.Log("Total connected gamepads: " + gamepads.Count);

        // Iterate through all connected gamepads to see details
        /*
        foreach (var gamepad in gamepads)
        {
            Debug.Log("Gamepad name: " + gamepad.name + " Device Id: " + gamepad.deviceId);
        }
        */
    }

    public bool InputReady()
    {
        return (checkConnectionStatus ? Gamepad.all.Count == controllerCount : true);
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        switch (change)
        {
            case InputDeviceChange.Added:
                if (debugMode)
                    Debug.Log("New device added: " + device);
                break;
            case InputDeviceChange.Removed:
                if (debugMode)
                    Debug.Log("Device removed: " + device);
                break;
            case InputDeviceChange.Reconnected:
                if (debugMode)
                    Debug.Log("Device Reconnected: " + device);
                break;
            case InputDeviceChange.Disconnected:
                if (debugMode)
                    Debug.Log("Device disconnected: " + device);
                break;
        }
    }
}

/*TODO: 
Figure out the controller scheme and exactly how it is considered ready. Do we need to also include the player on this? 
This script right now only tells when a controller is connected and nothing else. 

*/