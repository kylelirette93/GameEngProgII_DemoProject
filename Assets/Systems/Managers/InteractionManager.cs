using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    [Header("Interaction Settings")]
    private LayerMask interactableLayer;
    [SerializeField] private float interactionDistance = 3f;
    
    // Interface reference used internally.
    private IInteractable currentFocusedInteractable;

    private Transform cameraRoot;

    private InputManager inputManager => GameManager.Instance.InputManager;
    private UIManager uiManager => GameManager.Instance.UIManager;

    private void Start()
    {
        interactableLayer = LayerMask.GetMask("Interactable");
        cameraRoot = GameManager.Instance.PlayerController.CameraRoot;
    }

    private void Update()
    {
        HandleInteractionDetection();
    }

    private void HandleInteractionDetection()
    {
        if (Physics.Raycast(cameraRoot.position, cameraRoot.forward, out RaycastHit hitInfo, interactionDistance, interactableLayer))
        {
            Debug.Log(hitInfo.transform.name);
            // Get the interactable component from the hit object.
            IInteractable hitInteractable = hitInfo.transform.GetComponent<IInteractable>();

            if (hitInteractable != null)
            {
                if (hitInteractable != currentFocusedInteractable)
                {
                    if (currentFocusedInteractable != null)
                    {
                        // Clear focus from previous interactable.
                        currentFocusedInteractable.SetFocus(false);
                    }
                    // Set new focus.
                    currentFocusedInteractable = hitInteractable;

                    currentFocusedInteractable.SetFocus(true);

                    // Use reference to show interaction prompt.
                    if (hitInteractable is BaseInteractable baseInteractable)
                    {
                        string promptText = baseInteractable.GetInteractionPrompt();
                        uiManager.ShowInteractionPrompt(promptText);
                    }
                }
            }
        }
        else if (currentFocusedInteractable != null)
        {
            // If no hit, clear focus from current interactable.
            currentFocusedInteractable?.SetFocus(false);
            // Clear reference.
            currentFocusedInteractable = null;
        }
    }

    private void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (currentFocusedInteractable != null)
            {
                currentFocusedInteractable?.OnInteract();
            }
        }
    }
    private void OnEnable()
    {
        inputManager.InteractInputEvent += OnInteractInput;
    }
    private void OnDisable()
    {
        inputManager.InteractInputEvent -= OnInteractInput;
    }
}
