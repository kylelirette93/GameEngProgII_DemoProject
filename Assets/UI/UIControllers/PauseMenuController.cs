using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuController : MonoBehaviour
{
    UIDocument pauseMenuDoc => GetComponent<UIDocument>();
    Button resumeButton;
    Button mainMenuButton;
    Button quitButton;

    GameManager gameManager => GameManager.Instance;
    UIManager uiManager => GameManager.Instance.UIManager;

    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;
    LevelManager levelManager => GameManager.Instance.LevelManager;
    private Button[] menuButtons;
    private int focusedIndex = 0;

    #region Setup Button references and listeners.

    private void OnEnable()
    {
        resumeButton = pauseMenuDoc.rootVisualElement.Q<Button>("Resume");
        mainMenuButton = pauseMenuDoc.rootVisualElement.Q<Button>("MainMenu");
        quitButton = pauseMenuDoc.rootVisualElement.Q<Button>("Quit");

        resumeButton.clicked += OnResumeButtonClicked;
        mainMenuButton.clicked += OnMainMenuClicked;
        quitButton.clicked += OnQuitButtonClicked;
    }

    private void OnResumeButtonClicked()
    {
       gameStateManager.Resume();
    }

    private void OnQuitButtonClicked()
    {
        gameStateManager.Quit();
    }

    private void OnMainMenuClicked()
    {
        gameStateManager.MainMenu();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameStateManager.Resume();
        }
    }
    #endregion
}
