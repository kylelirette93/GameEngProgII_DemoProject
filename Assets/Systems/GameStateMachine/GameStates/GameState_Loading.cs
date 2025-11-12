using UnityEngine;
using UnityEngine.InputSystem;

public class GameState_Loading : IState
{
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;
    PlayerController playerController => GameManager.Instance.PlayerController;

    UIManager uiManager => GameManager.Instance.UIManager;

    #region Singleton Instance
    private static readonly GameState_Loading instance = new GameState_Loading();

    public static GameState_Loading Instance = instance;

    #endregion
    public void EnterState()
    {
        Debug.Log("Entered game state loading.");
        uiManager.EnableLoadingMenu();
        Cursor.visible = false;
        Time.timeScale = 1f;
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
