using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject GameplayPanel;
    public GameObject PauseMenuPanel;
    public GameObject GameOverPanel;

    private void Awake()
    {
        DisableAllMenuUI();
    }
    public void DisableAllMenuUI()
    {
        MainMenuPanel.SetActive(false);
        GameplayPanel.SetActive(false);
        PauseMenuPanel.SetActive(false);
        GameOverPanel.SetActive(false);
    }

    public void EnableGameOverMenu()
    {
        DisableAllMenuUI();
        GameOverPanel.SetActive(true);
    }

    public void EnableMainMenu()
    {
        DisableAllMenuUI();
        MainMenuPanel.SetActive(true);
    }

    public void EnableGameplayUI()
    {
        DisableAllMenuUI();
        GameplayPanel.SetActive(true);
    }
    public void EnablePauseMenu()
    {
        DisableAllMenuUI();
        PauseMenuPanel.SetActive(true);
    }

    public void EnableCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void DisableCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }


}
