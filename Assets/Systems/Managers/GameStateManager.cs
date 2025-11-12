using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : MonoBehaviour
{
    [Header("Debug (read only)")]
    [SerializeField] private string lastActiveState;
    [SerializeField] private string currentActiveState;

    public IState currentState;
    private IState lastState;

    // Instantiate game states.
    private GameState_Bootload gameState_Bootload = GameState_Bootload.Instance;
    private GameState_MainMenu gameState_MainMenu = GameState_MainMenu.Instance;
    private GameState_Gameplay gameState_Gameplay = GameState_Gameplay.Instance;
    private GameState_Paused gameState_Paused = GameState_Paused.Instance;
    private GameState_GameOver gameState_GameOver = GameState_GameOver.Instance;
    private GameState_Loading gameState_Loading = GameState_Loading.Instance;

    private void Start()
    {
        currentState = gameState_Bootload;
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

    #region Button Call Methods

    public void GameOver()
    {
        SwitchToState(gameState_GameOver);
    }
    public void PlayGame()
    {
        GameManager.Instance.LevelManager.ChangeScene("Level01", "SpawnPoint");
        SwitchToState(gameState_Gameplay);
    }
    public void Pause()
    {        
        if (currentState == gameState_Gameplay) 
            SwitchToState(gameState_Paused);
    }

    public void Resume()
    {
        if (currentState == gameState_Paused) 
            SwitchToState(gameState_Gameplay);
    }

    public void MainMenu()
    {
        GameManager.Instance.LevelManager.ChangeScene("MainMenu");
        SwitchToState(gameState_MainMenu);
    }

    public void Quit()
    {
        Application.Quit();
    }
    #endregion
}
