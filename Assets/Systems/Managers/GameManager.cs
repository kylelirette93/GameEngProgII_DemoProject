using UnityEngine;

// Initialize first
[DefaultExecutionOrder(-100)]

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Manager References (Auto-Assigned)")]
    [SerializeField] private InputManager inputManager;
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private UIManager uiManager;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private InteractionManager interactionManager;

    // public read-only accessors for other scripts to use the managers
    public InputManager InputManager => inputManager;
    public GameStateManager GameStateManager => gameStateManager;
    public PlayerController PlayerController => playerController;

    public LevelManager LevelManager => levelManager;

    public UIManager UIManager => uiManager;
    public InteractionManager InteractionManager => interactionManager;

    void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        // Auto-assign manager references if not set in inspector
        inputManager ??= GetComponentInChildren<InputManager>();
        gameStateManager ??= GetComponentInChildren<GameStateManager>();
        playerController ??= GetComponentInChildren<PlayerController>();
        uiManager ??= GetComponentInChildren<UIManager>();
        levelManager ??= GetComponentInChildren<LevelManager>();
        interactionManager ??= GetComponentInChildren<InteractionManager>();
    }
}
