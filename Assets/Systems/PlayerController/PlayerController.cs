using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Manager references
    private InputManager inputManager;

    // Input Vectors.
    Vector2 moveInput;
    Vector2 lookInput;

    // References.
    CharacterController controller;
    Transform cameraRoot;

    [Header("Movement Settings")]
    public float movementSpeed = 5f;
    float speedTransitionDuration = 0.25f;
    [SerializeField] private float currentMoveSpeed;

    private float crouchMoveSpeed = 2.0f;
    private float sprintMoveSpeed = 5f;
    float walkMoveSpeed = 2.0f;

    private bool sprintInput = false;
    private bool crouchInput = false;

    [Header("Look Settings")]
    public float horizontalSensitivity = 1f;
    public float verticalSensitivity = 1f;
    public float upperLookLimit = 60f;
    public float lowerLookLimit = -60f;

    [Header("Look and Move Toggles")]
    public bool lookEnabled = true;
    public bool moveEnabled = true;

    [SerializeField] bool jumpEnabled = true;
    [SerializeField] bool sprintEnabled = true;


    private void Awake()
    {
        inputManager = GameManager.Instance.inputManager;
        controller = GetComponent<CharacterController>();
        cameraRoot = GameObject.Find("CameraRoot").transform;
        
    }
    private void OnEnable()
    {
        inputManager.MoveInputEvent += GetMovementInput;
        inputManager.LookInputEvent += GetLookInput;

        inputManager.JumpInputEvent += HandleJump;

        inputManager.CrouchInputEvent += HandleCrouch;
        inputManager.SprintInputEvent += HandleSprint;
    }
    private void GetMovementInput(Vector2 vector)
    {
        moveInput = new Vector2(vector.x, vector.y);
        Debug.Log("Move Input: " + moveInput);
    }
 

    private void GetLookInput(Vector2 vector)
    {
        lookInput = new Vector2(vector.x, vector.y);
    }

    private void HandleJump(InputAction.CallbackContext context) 
    {
        if (jumpEnabled && context.started)
        {
            Debug.Log("Jump started");
        }
        if (context.performed)
        {
            Debug.Log("I'm jumping!");
        }
    }

    private void Update()
    {
        HandlePlayerMovement();
    }

    private void LateUpdate()
    {
        HandlePlayerLook();
    }

    private void HandlePlayerMovement()
    {
        if (moveEnabled == false) return;

        // Step1: get input direction.
        Vector3 moveInputDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 worldMoveDirection = transform.TransformDirection(moveInputDirection);

        // Step2: determine movement speed.
        float targetSpeed = movementSpeed;

        if (sprintInput == true)
        {
            targetSpeed = sprintMoveSpeed;
        }
        else
        {
            targetSpeed = walkMoveSpeed;
        }

        // Step3: smoothly interpolate current speed towards target speed.
        float lerpSpeed = 1f - Mathf.Pow(0.01f, Time.deltaTime / speedTransitionDuration);
        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, targetSpeed, lerpSpeed);

        // Step4: 

        Vector3 horizontalMovement = worldMoveDirection * currentMoveSpeed;
        controller.Move(horizontalMovement * Time.deltaTime);
    }

    private void HandlePlayerLook()
    {
        if (lookEnabled == false) return;

        float horizontalLook = lookInput.x * horizontalSensitivity;
        float verticalLook = lookInput.y * verticalSensitivity;

        transform.Rotate(Vector3.up * horizontalLook);
        Vector3 currentAngles = cameraRoot.localEulerAngles;
        float newRotationX = currentAngles.x - verticalLook;
        newRotationX = (newRotationX > 180) ? newRotationX - 360 : newRotationX;
        newRotationX = Mathf.Clamp(newRotationX, lowerLookLimit, upperLookLimit);

        cameraRoot.localEulerAngles = new Vector3(newRotationX, 0, 0);
    }
    private void HandleCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Begin crouching..");
        }
        if (context.canceled)
        {
            Debug.Log("No longer crouching.");
        }
    }

    private void HandleSprint(InputAction.CallbackContext context)
    {
        if (!sprintEnabled) return;

        if (context.started)
        {
            sprintInput = true;
        }
 
        if (context.canceled)
        {
            sprintInput = false;
        }
    }

    private void OnDestroy()
    {
        inputManager.MoveInputEvent -= GetMovementInput;
        inputManager.LookInputEvent -= GetLookInput;

        inputManager.JumpInputEvent -= HandleJump;

        inputManager.SprintInputEvent -= HandleSprint;
        inputManager.CrouchInputEvent -= HandleCrouch;
    }
}
