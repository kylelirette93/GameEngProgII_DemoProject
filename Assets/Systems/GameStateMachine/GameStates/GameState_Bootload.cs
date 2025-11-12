using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameState_Bootload : IState
{
    GameManager gameManager => GameManager.Instance;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;
    PlayerController playerController => GameManager.Instance.PlayerController;

    UIManager uiManager => GameManager.Instance.UIManager;

    #region Singleton Instance
    private static readonly GameState_Bootload instance = new GameState_Bootload();

    public static GameState_Bootload Instance = instance;

    #endregion
    public void EnterState()
    {
        Cursor.visible = false;
        Time.timeScale = 0f;
        Debug.Log("Entered Bootload State");

        // Detect current scene type and set game state accordingly.
        if (SceneManager.sceneCount == 1 && SceneManager.GetActiveScene().name == "BootLoader")
        {
            GameManager.Instance.GameStateManager.SwitchToState(GameState_MainMenu.Instance);
            GameManager.Instance.LevelManager.ChangeScene("MainMenu", "SpawnPoint");
        }
        else if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            GameManager.Instance.GameStateManager.SwitchToState(GameState_MainMenu.Instance);
        }
        else
        {
            GameManager.Instance.GameStateManager.SwitchToState(GameState_Gameplay.Instance);
        }   
    }

    public void FixedUpdateState()
    {

    }
    
    public void ExitState()
    {
        Debug.Log("Exiting Bootload State");
    }

    public void UpdateState()
    {
        // bootload update
    }

    public void LateUpdateState()
    {
        // bootload late update
    }
}
