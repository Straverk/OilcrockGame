using UnityEngine;
using Input;
using UnityEngine.InputSystem;
using static Input.PlayerInputActions;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class PlayerInputSystem : IPlayerActions
{
    public delegate void InputFloatHandler(float input);
    public delegate void InputVector2Handler(Vector2 input);


    private PlayerInputActions inputActions;

    public void Init()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if (inputActions == null)
        {
            inputActions = new();
            inputActions.Player.SetCallbacks(this);
        }

        inputActions.Player.Enable();
    }

    public void Enable() =>
        inputActions.Player.Enable();

    public void Disable() =>
        inputActions.Player.Disable();

    
    private InputVector2Handler moveHandler;
    public void SetMoveHandler(InputVector2Handler handler) =>
        moveHandler = handler;
    public void OnMove(InputAction.CallbackContext context) =>
        moveHandler(context.ReadValue<Vector2>());


    private InputVector2Handler lookHandler;
    public void SetLookHandler(InputVector2Handler handler) =>
        lookHandler = handler;
    public void OnLook(InputAction.CallbackContext context) =>
        lookHandler(context.ReadValue<Vector2>());


    private InputFloatHandler sprintHandler;
    public void SetSprintHandler(InputFloatHandler handler) =>
        sprintHandler = handler;
    public void OnSprint(InputAction.CallbackContext context) =>
        sprintHandler(context.ReadValue<float>());


    private InputFloatHandler crouchHandler;
    public void SetCrouchHandler(InputFloatHandler handler) =>
        crouchHandler = handler;
    public void OnCrouch(InputAction.CallbackContext context) =>
        crouchHandler(context.ReadValue<float>());


    private InputFloatHandler jumpHandler;
    public void SetJumpHandler(InputFloatHandler handler) =>
        jumpHandler = handler;
    public void OnJump(InputAction.CallbackContext context) =>
        jumpHandler(context.ReadValue<float>());

    private InputFloatHandler interactHandler;
    public void SetInteractHandler(InputFloatHandler handler) =>
        interactHandler = handler;
    public void OnInteract(InputAction.CallbackContext context)
    {
        interactHandler(context.ReadValue<float>());
    }

    public void OnPrimaryAction(InputAction.CallbackContext context)
    {
        //throw new System.NotImplementedException();
    }

    public void OnSecondaryAction(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        throw new System.NotImplementedException();
    }
}
