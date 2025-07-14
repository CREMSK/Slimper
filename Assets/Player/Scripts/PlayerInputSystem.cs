using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSystem : IPlayerInput
{
    public event Action onJumpStart;
    public event Action onJumpEnd;

    private InputSystem_Actions inputActions_;

    public PlayerInputSystem()
    {
        // initializing the new input system
        inputActions_ = new InputSystem_Actions();
        inputActions_.Player.Enable();

        // subscribing to input events
        inputActions_.Player.ChargeJump.started += startJump;
        inputActions_.Player.ChargeJump.canceled += endJump;
    }

    ~PlayerInputSystem()
    {
        // unsubscribing to input events
        inputActions_.Player.ChargeJump.started -= startJump;
        inputActions_.Player.ChargeJump.canceled -= endJump;
    }

    public Vector2 GetMouseWorldPos()
    {
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        Vector2 mouseWorldPosition = (Vector2)Camera.main.ScreenToWorldPoint(mousePosition);

        return mouseWorldPosition;
    }

    private void startJump(InputAction.CallbackContext context) {
        onJumpStart?.Invoke();
    }
    
    private void endJump(InputAction.CallbackContext context) {
        onJumpEnd?.Invoke();
    }
}
