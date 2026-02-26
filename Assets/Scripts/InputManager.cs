using UnityEngine;
using UnityEngine.InputSystem;

public static class InputManager
{
    public static bool IsFeedingEnabled { get; private set; } = false;

    public static void HandleGlobalInput()
    {
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            IsFeedingEnabled = !IsFeedingEnabled;
            Debug.Log($"InputManager: Feeding Mode Toggled - Now {(IsFeedingEnabled ? "ON" : "OFF")}");
        }
    }
}
