using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    [Header("Debug (read only)")]
    [SerializeField] private string lastActiveState;
    [SerializeField] private string currentActiveState;

    public IState currentState;
    private IState lastState;

    // Instantiate game states.
    private GameState_MainMenu gameState_MainMenu = GameState_MainMenu.Instance;
    private GameState_Gameplay gameState_Gameplay = GameState_Gameplay.Instance;

    private void Start()
    {
        currentState = gameState_Gameplay;
        currentState.EnterState();
        currentActiveState = currentState.ToString();
    }
    #region State Machine Update Calls
    private void Update()
    {
        currentState.UpdateState();
    }
    #endregion

    #region State Machine Fixed Update Calls
    private void FixedUpdate()
    {
        currentState.FixedUpdateState();
    }
    #endregion

    #region State Machine Late Update Calls
    private void LateUpdate()
    {
        currentState.LateUpdateState();
    }
    #endregion

    public void SwitchToState(IState newState)
    {
        lastState = currentState;
        lastActiveState = lastState.ToString();
        currentState.ExitState();
        currentState = newState;
        currentActiveState = currentState.ToString();
        currentState.EnterState();
    }
}
