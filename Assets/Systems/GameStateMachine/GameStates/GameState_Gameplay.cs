

using UnityEngine;
using UnityEngine.InputSystem;

public class GameState_Gameplay : IState
{
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;
    PlayerController playerController => GameManager.Instance.PlayerController;

    #region Singleton Instance
    private static readonly GameState_Gameplay instance = new GameState_Gameplay();

    public static GameState_Gameplay Instance = instance;

    #endregion
    public void EnterState()
    {
        Debug.Log("Entered Gameplay State");
    }

    public void FixedUpdateState()
    {

    }
    public void UpdateState()
    {
        playerController.HandlePlayerMovement();
        Debug.Log("Running gameplay update state.");
        if (Keyboard.current[Key.P].wasPressedThisFrame)
        {
            gameStateManager.SwitchToState(GameState_MainMenu.Instance);
        }
    }

    public void LateUpdateState()
    {
        playerController.HandlePlayerLook();
    }
    public void ExitState()
    {
        Debug.Log("Exiting Gameplay State");
    }

}
