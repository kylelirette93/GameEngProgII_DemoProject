using UnityEngine;
using UnityEngine.InputSystem;

public class GameState_GameOver : IState
{
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;

    UIManager uIManager => GameManager.Instance.UIManager;

    #region Singleton Instance
    // A single, readonly instance of the atate class is created.
    // The 'readonly' keyword ensures this instance cannot be modified after initialization.
    private static readonly GameState_GameOver instance = new GameState_GameOver();

    // Provides global access to the singleton instance of this state.
    // Uses an expression-bodied property to return the static _instance variable.
    public static GameState_GameOver Instance = instance;
    #endregion
    public void EnterState()
    {
        Time.timeScale = 0f;
        Debug.Log("Entered Game Over State");
        uIManager.EnableGameOverMenu();
        uIManager.EnableCursor();
    }

    public void FixedUpdateState()
    {

    }
    public void UpdateState()
    {
      
    }

    public void LateUpdateState()
    {

    }
    public void ExitState()
    {

    }

}
