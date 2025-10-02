using UnityEngine;
using UnityEngine.InputSystem;

public class GameState_Paused : IState
{
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;

    UIManager uIManager => GameManager.Instance.UIManager;

    #region Singleton Instance
    // A single, readonly instance of the atate class is created.
    // The 'readonly' keyword ensures this instance cannot be modified after initialization.
    private static readonly GameState_Paused instance = new GameState_Paused();

    // Provides global access to the singleton instance of this state.
    // Uses an expression-bodied property to return the static _instance variable.
    public static GameState_Paused Instance = instance;
    #endregion
    public void EnterState()
    {
        Time.timeScale = 0f;
        Debug.Log("Entered Main Menu State");
        uIManager.EnablePauseMenu();
        uIManager.EnableCursor();
    }

    public void FixedUpdateState()
    {

    }
    public void UpdateState()
    {
        Debug.Log("Running main menu update state.");
        if (Keyboard.current[Key.P].wasPressedThisFrame)
        {
            gameStateManager.Resume();
        }
    }

    public void LateUpdateState()
    {

    }
    public void ExitState()
    {

    }

}
