using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerAssigner : MonoBehaviour
{
    public PlayerInput[] players; // Assign in Inspector

    void Start()
    {
        var gamepads = Gamepad.all;

        for (int i = 0; i < players.Length && i < gamepads.Count; i++)
        {
            players[i].SwitchCurrentControlScheme("Gamepad", gamepads[i]);
            print($"Assigned {gamepads[i].name} to {players[i].gameObject.name}");
        }

    }
}
