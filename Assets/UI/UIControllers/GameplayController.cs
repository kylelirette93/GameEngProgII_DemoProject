using UnityEngine;
using UnityEngine.UIElements;

public class GameplayController : MonoBehaviour
{
    UIDocument mainMenuDoc => GetComponent<UIDocument>();

    GameManager gameManager => GameManager.Instance;
    UIManager uiManager => GameManager.Instance.UIManager;

    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;
    LevelManager levelManager => GameManager.Instance.LevelManager;
    private Button[] menuButtons;
    private int focusedIndex = 0;

    #region Setup Button references and listeners.

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameStateManager.Pause();
        }
    }
    #endregion
}
