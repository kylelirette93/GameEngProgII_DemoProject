using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum MovementState
    {
        Idle,
        Walking,
        Sprinting,
        Crouching,
        Jumping,
        Falling
    }

    public MovementState currentMovementState;
    public float characterVelocity;
    // Manager references
    private InputManager inputManager;

    // Input Vectors.
    Vector2 moveInput;
    Vector2 lookInput;

    // References.
    CharacterController controller;
    public Transform CameraRoot => cameraRoot;
    Transform cameraRoot;

    [Header("Movement Settings")]
    public float movementSpeed = 5f;
    float speedTransitionDuration = 0.25f;
    float crouchTransitionDuration = 0.50f;
    [SerializeField] private float currentMoveSpeed;
    private Vector3 velocity;

    [Header("Look Settings")]
    public float horizontalSensitivity = 1f;
    public float verticalSensitivity = 1f;
    public float upperLookLimit = 60f;
    public float lowerLookLimit = -60f;

    [Header("Gravity and Jump Settings")]
    [SerializeField] private bool isGrounded;
    [SerializeField] private float gravity = 30.0f;
    [SerializeField] private float jumpHeight = 2.0f;
    float jumpCooldown = 0.2f;
    float jumpCooldownTimer = 0f;
    float groundCheckRadius = 0.1f;
    bool jumpRequested = false;

    [Header("Crouch Settings")]
    private float standingHeight;
    private Vector3 standingCenter;
    private float standingCamY;
    private bool isObstructed = false;
    private bool crouchInput = false;
    [SerializeField] private float crouchingHeight = 1.0f;
    [SerializeField] private Vector3 crouchingCenter = new Vector3(0, 0.5f, 0);
    [SerializeField] private float crouchingCamY = 0.75f;
    int playerLayerMask;

    private float targetHeight;
    private Vector3 targetCenter;
    private float targetCamY; // Target Y position for camera root during crouch transition

    public Transform spawnPosition;

    Vector3 standingPosition;
    private float crouchMoveSpeed = 2.0f;
    private float sprintMoveSpeed = 8.0f;
    float walkMoveSpeed = 5.0f;

    private bool sprintInput = false;

    [Header("Look and Move Toggles")]
    public bool lookEnabled = true;
    public bool moveEnabled = true;

    [SerializeField] bool jumpEnabled = true;
    [SerializeField] bool sprintEnabled = true;
    [SerializeField] bool crouchEnabled = true;

    private void Awake()
    {
        playerLayerMask = ~LayerMask.GetMask("Player");

        #region Initialization Default values
        currentMovementState = MovementState.Idle;
        inputManager = GameManager.Instance.InputManager;
        controller = GetComponent<CharacterController>();
        cameraRoot = GameObject.Find("CameraRoot").transform;

        // Store initial standing values.
        standingHeight = controller.height;
        standingCenter = controller.center;
        standingCamY = cameraRoot.localPosition.y;

        targetHeight = standingHeight;
        targetCenter = standingCenter;
        targetCamY = cameraRoot.localPosition.y;

        // Set default state of bools.
        crouchInput = false;
        sprintInput = false;

        #endregion
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
        if (context.started)
        {
            if (jumpEnabled && isGrounded && jumpCooldownTimer <= 0f)
            {
                Debug.Log("Jump started");
                // Calculate initial upward velocity needed to reach desired jump height.
                jumpRequested = true;

                jumpCooldownTimer = 0.1f;
            }
        }
       
    }

    public void HandlePlayerMovement()
    {
        if (moveEnabled == false) return;

        // Determine movement state.
        DetermineMovementState();

        // Perform ground check.
        GroundedCheck();

        // Handle crouch transition.
        HandleCrouchTransition();

        // Apply Movement.
        ApplyMovement();
    }

    private void DetermineMovementState()
    {
        // determine current movement state based on inputs and conditions.

        // if player is not grounded, they are either jumping or falling.
        if (!isGrounded)
        {
            if (velocity.y > 0.1f)
            {
                currentMovementState = MovementState.Jumping;
            }
            else
            {
                currentMovementState = MovementState.Falling;
            }
        }
        else if (isGrounded)
        {
            if (crouchInput || isObstructed)
            {
                currentMovementState = MovementState.Crouching;
            }
            // Sprint check.
            else if (sprintInput && currentMovementState != MovementState.Crouching)
            {
                currentMovementState = MovementState.Sprinting;
            }
            else if (moveInput.magnitude > 0.1f && !sprintInput && !crouchInput)
            {
                currentMovementState = MovementState.Walking;
            }
            else if (moveInput.magnitude <= 0.1f)
            {
                currentMovementState = MovementState.Idle;
            }          
        }
    }

    private void ApplyMovement()
    {
        // Step1: get input direction.
        Vector3 moveInputDirection = new Vector3(moveInput.x, 0, moveInput.y);
        Vector3 worldMoveDirection = transform.TransformDirection(moveInputDirection);

        // Step2: determine movement speed.
        float targetSpeed = movementSpeed;

       switch (currentMovementState)
        {
            case MovementState.Crouching:
                movementSpeed = crouchMoveSpeed;
                break;
            case MovementState.Sprinting:
                movementSpeed = sprintMoveSpeed;
                break;
            case MovementState.Walking:
                movementSpeed = walkMoveSpeed;
                break;
            default:
                movementSpeed = walkMoveSpeed;
                break;
        }

        // Step3: smoothly interpolate current speed towards target speed.
        float lerpSpeed = 1f - Mathf.Pow(0.01f, Time.deltaTime / speedTransitionDuration);
        currentMoveSpeed = Mathf.Lerp(currentMoveSpeed, targetSpeed, lerpSpeed);

        // Step4: 

        Vector3 horizontalMovement = worldMoveDirection * currentMoveSpeed;

        // Step 5: Handle jump and gravity here.
        ApplyJumpAndGravity();
        Vector3 movement = horizontalMovement;
        movement.y = velocity.y;
        controller.Move(movement * Time.deltaTime);
    }

    public void HandlePlayerLook()
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

    private void ApplyJumpAndGravity()
    {
        if (jumpEnabled == true)
        {
            if (jumpRequested)
            {
                velocity.y = Mathf.Sqrt(2f * jumpHeight * gravity);
                jumpRequested = false;
                jumpCooldownTimer = jumpCooldown;
            }
        }

        // Apply gravity based on player's state.
        if (isGrounded && velocity.y < 0)
        {
            // If grounded and moving downwards due to accumulated gravity from previous frames
            // snap velocity to small negative value. This keeps the player firmly on the ground
            // Without allowing gravity to build up indefinitely. Preventing bouncing or incorrect
            // ground detection issues.
            velocity.y = -1f;
        }
        else
        {
            velocity.y -= gravity * Time.deltaTime;
        }
         if (jumpCooldownTimer > 0)
            {
                jumpCooldownTimer -= Time.deltaTime;
            }
    }

    private void HandleCrouchTransition()
    {
        bool shouldCrouch = crouchInput == true;

        // if airborne and was crouching, maintain crouch state (prevents standing up from crouch while walking off a ledge)
        bool wasAlreadyCrouching = controller.height < (standingHeight - 0.05f);

        if (isGrounded == false && wasAlreadyCrouching)
        {
            shouldCrouch = true; // Maintain crouch state if airborne (walking off ledge while crouching)
        }

        if (shouldCrouch)
        {
            targetHeight = crouchingHeight;
            targetCenter = crouchingCenter;
            targetCamY = crouchingCamY;
            isObstructed = false; // No obstruction when intentionally crouching
        }
        else
        {
            // float maxAllowedHeight = GetMaxAllowedHeight();

            float maxAllowedHeight = GetMaxAllowedHeight();

            if (maxAllowedHeight >= standingHeight - 0.05f)
            {
                // No obstruction, allow immediate transition to standing
                targetHeight = standingHeight;
                targetCenter = standingCenter;
                targetCamY = standingCamY;
                isObstructed = false;
            }
            else
            {
                // Obstruction detected, limit height and center
                targetHeight = Mathf.Min(standingHeight, maxAllowedHeight);
                float standRatio = Mathf.Clamp01((targetHeight - crouchingHeight) / (standingHeight - crouchingHeight));
                targetCenter = Vector3.Lerp(crouchingCenter, standingCenter, standRatio);
                targetCamY = Mathf.Lerp(crouchingCamY, standingCamY, standRatio);
                isObstructed = true;
            }
        }

        // Calculate lerp speed based on desired duration
        // This formula ensures the transition approximately reaches 99% of the target in 'crouchTransitionDuration' seconds.
        float lerpSpeed = 1f - Mathf.Pow(0.01f, Time.deltaTime / crouchTransitionDuration);

        // Smoothly transition to targets
        controller.height = Mathf.Lerp(controller.height, targetHeight, lerpSpeed);
        controller.center = Vector3.Lerp(controller.center, targetCenter, lerpSpeed);

        Vector3 currentCamPos = cameraRoot.localPosition;
        cameraRoot.localPosition = new Vector3(currentCamPos.x, Mathf.Lerp(currentCamPos.y, targetCamY, lerpSpeed), currentCamPos.z);
    }
    public void MovePlayerToSpawnPoint(Transform spawnPoint)
    {
        // Move the player to the spawn point position.
        controller.enabled = false; 
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
        controller.enabled = true;
    }
    private void HandleCrouch(InputAction.CallbackContext context)
    {
        if (crouchEnabled == false) return;

        if (context.started)
        {
            crouchInput = !crouchInput;
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

    private void GroundedCheck()
    {
        isGrounded = controller.isGrounded;
    }

    private float GetMaxAllowedHeight()
    {
        RaycastHit hit;
        // Cast a ray upwards from the players position to check for obstructions.
        // If we hit something, calculated the max allowed height based on hit distance.
        float maxCheckDistance = standingHeight + 0.15f;
        if (Physics.Raycast(transform.position, Vector3.up, out hit, maxCheckDistance, playerLayerMask))
        {
            // If we hit something, calculate max heihgt player can stand.

            // Subtract small buffer to prevent clipping.
            float maxHeight = hit.distance - 0.1f;

            maxHeight = Mathf.Max(maxHeight, crouchingHeight);

            Debug.Log("Obstruction detected. Max allowed height: " + maxHeight);
            return maxHeight;
        }

        // If no obstruction found, can stand at full height.
        return standingHeight;
    }

    public void MovePlayerToSpawnpoint(Transform spawnPosition)
    {
        controller.enabled = false;
        transform.position = spawnPosition.position;
        transform.rotation = spawnPosition.rotation;
        transform.eulerAngles = spawnPosition.eulerAngles;
        cameraRoot.localEulerAngles = spawnPosition.localEulerAngles;
        controller.enabled = true;
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
