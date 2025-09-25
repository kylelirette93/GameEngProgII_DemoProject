using UnityEngine;
using UnityEngine.InputSystem;

public class GameState_MainMenu : IState
{
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;

    #region Singleton Instance
    // A single, readonly instance of the atate class is created.
    // The 'readonly' keyword ensures this instance cannot be modified after initialization.
    private static readonly GameState_MainMenu instance = new GameState_MainMenu();

    // Provides global access to the singleton instance of this state.
    // Uses an expression-bodied property to return the static _instance variable.
    public static GameState_MainMenu Instance = instance;
    #endregion
    public void EnterState()
    {
        Debug.Log("Entered Main Menu State");
    }

    public void FixedUpdateState()
    {

    }
    public void UpdateState()
    {
        Debug.Log("Running main menu update state.");
        if (Keyboard.current[Key.P].wasPressedThisFrame)
        {
            gameStateManager.SwitchToState(GameState_Gameplay.Instance);
        }
    }

    public void LateUpdateState()
    {

    }
    public void ExitState()
    {

    }

}
