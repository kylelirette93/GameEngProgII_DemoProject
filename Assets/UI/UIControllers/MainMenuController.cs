using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    UIDocument mainMenuDoc => GetComponent<UIDocument>();
    Button playButton;
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
        playButton = mainMenuDoc.rootVisualElement.Q<Button>("Play");
        quitButton = mainMenuDoc.rootVisualElement.Q<Button>("Quit");

        playButton.clicked += OnPlayButtonClicked;
        quitButton.clicked += OnQuitButtonClicked;
    }

    private void OnPlayButtonClicked()
    {
        gameStateManager.PlayGame();
    }

    private void OnQuitButtonClicked()
    {
        Application.Quit();
    }

    
    #endregion
}
