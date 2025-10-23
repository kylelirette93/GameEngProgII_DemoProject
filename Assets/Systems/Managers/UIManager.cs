using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    [Header("UI Menu Objects")]
    [SerializeField] private UIDocument mainMenuUI;
    [SerializeField] private UIDocument gameplayUI;
    [SerializeField] private UIDocument pauseUI;
    [SerializeField] private UIDocument gameoverUI;

    private void Awake()
    {
        mainMenuUI = FindUIDocument("MainMenu");
        gameplayUI = FindUIDocument("Gameplay");
        pauseUI = FindUIDocument("PauseMenu");
        gameoverUI = FindUIDocument("Gameover");

        if (mainMenuUI != null) mainMenuUI.gameObject.SetActive(true);
        if (gameplayUI != null) gameplayUI.gameObject.SetActive(true);
        if (pauseUI != null) pauseUI.gameObject.SetActive(true);
        if (gameoverUI != null) gameoverUI.gameObject.SetActive(true);
        DisableAllMenuUI();
    }
    public void DisableAllMenuUI()
    {
        mainMenuUI.rootVisualElement.style.display = DisplayStyle.None;
        gameplayUI.rootVisualElement.style.display = DisplayStyle.None;
        pauseUI.rootVisualElement.style.display = DisplayStyle.None;
        gameoverUI.rootVisualElement.style.display = DisplayStyle.None;
    }

    public void EnableGameOverMenu()
    {
        DisableAllMenuUI();
        gameoverUI.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    public void EnableMainMenu()
    {
        DisableAllMenuUI();
        mainMenuUI.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    public void EnableGameplayUI()
    {
        DisableAllMenuUI();
        gameplayUI.rootVisualElement.style.display = DisplayStyle.Flex;
    }
    public void EnablePauseMenu()
    {
        DisableAllMenuUI();
        pauseUI.rootVisualElement.style.display = DisplayStyle.Flex;
    }

    public void EnableCursor()
    {
        /*Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;*/
    }
    public void DisableCursor()
    {
        /*Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;*/
    }

    private UIDocument FindUIDocument(string name)
    {
        var documents = Object.FindObjectsByType<UIDocument>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (var doc in documents)
        {
            if (doc.name == name)
            {
                return doc;
            }
        }
        Debug.LogWarning($"UIDocument '{name}' not found in scene.");
        return null;
    }
}
