using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, Inputs.IPlayerActions
{
    private Inputs inputs;

    void Awake()
    {
        try
        {
            inputs = new Inputs();
            inputs.Player.SetCallbacks(this);
            inputs.Player.Enable();
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error initializing InputManager: {e.Message}");
        }       
    }
    #region Input Events

    public event Action<Vector2> MoveInputEvent;
    public event Action<Vector2> LookInputEvent;

    public event Action JumpStartedInputEvent;
    public event Action JumpPerformedInputEvent;
    public event Action JumpCanceledInputEvent;

    public event Action SprintStartedInputEvent;
    public event Action SprintPerformedInputEvent;
    public event Action SprintCanceledInputEvent;

    #endregion

    #region InputCallbacks
    public void OnLook(InputAction.CallbackContext context)
    {
        LookInputEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveInputEvent?.Invoke(context.ReadValue<Vector2>());
    }
    #endregion

    void OnEnable()
    {
        if (inputs != null)
        {
            inputs.Player.Enable();
        }
    }
    void OnDestroy()
    {
        if (inputs != null) 
        {
            inputs.Player.Disable();
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.started) JumpStartedInputEvent?.Invoke();
        if (context.performed) JumpPerformedInputEvent?.Invoke();
        if (context.canceled) JumpCanceledInputEvent?.Invoke();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.started) SprintStartedInputEvent?.Invoke();
        if (context.performed) SprintPerformedInputEvent?.Invoke();
        if (context.canceled) SprintCanceledInputEvent?.Invoke();
    }
}
