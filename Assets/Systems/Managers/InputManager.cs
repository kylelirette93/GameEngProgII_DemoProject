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
    public event Action<InputAction.CallbackContext> JumpInputEvent;
    public event Action<InputAction.CallbackContext> SprintInputEvent;
    public event Action<InputAction.CallbackContext> CrouchInputEvent;
    public event Action<InputAction.CallbackContext> InteractInputEvent;

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
        JumpInputEvent?.Invoke(context);
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        SprintInputEvent?.Invoke(context);
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        CrouchInputEvent?.Invoke(context);
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        InteractInputEvent?.Invoke(context);
    }
}
