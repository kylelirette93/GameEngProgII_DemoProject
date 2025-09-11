using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Manager references
    private InputManager inputManager;

    // Input
    public Vector2 moveInput;
    public Vector2 lookInput;

    private void Awake()
    {
        inputManager = GameManager.Instance.inputManager;
    }
    private void OnEnable()
    {
        inputManager.MoveInputEvent += SetMoveInput;
        inputManager.LookInputEvent += SetLookInput;

        inputManager.JumpStartedInputEvent += OnJumpStarted;
        inputManager.JumpPerformedInputEvent += OnJumpPerformed;
        inputManager.JumpCanceledInputEvent += OnJumpCanceled;

        inputManager.SprintStartedInputEvent += OnSprintStarted;
        inputManager.SprintPerformedInputEvent += OnSprintPerformed;
        inputManager.SprintCanceledInputEvent += OnSprintCanceled;
    }

    private void OnJumpCanceled()
    {
        Debug.Log("Jump Canceled");
    }

    private void OnJumpPerformed()
    {
        Debug.Log("Jump Performed");
    }

    private void OnJumpStarted()
    {
        Debug.Log("Jump Started");
    }

    private void OnSprintCanceled()
    {
        Debug.Log("Sprint Canceled");
    }

    private void OnSprintPerformed()
    {
        Debug.Log("Sprint Performed");
    }

    private void OnSprintStarted()
    {
        Debug.Log("Sprint Started");
    }
    private void SetLookInput(Vector2 vector)
    {
        lookInput = new Vector2(vector.x, vector.y);
    }

    private void SetMoveInput(Vector2 vector)
    {
        moveInput = new Vector2(vector.x, vector.y);
        Debug.Log("Move Input: " + moveInput);
    }

    private void OnDestroy()
    {
        inputManager.MoveInputEvent -= SetMoveInput;
        inputManager.LookInputEvent -= SetLookInput;

        inputManager.JumpStartedInputEvent -= OnJumpStarted;
        inputManager.JumpPerformedInputEvent -= OnJumpPerformed;
        inputManager.JumpCanceledInputEvent -= OnJumpCanceled;

        inputManager.SprintStartedInputEvent -= OnSprintStarted;
        inputManager.SprintPerformedInputEvent -= OnSprintPerformed;
        inputManager.SprintCanceledInputEvent -= OnSprintCanceled;
    }
}
