using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    Transform player;
    string _spawnPoint;
    GameStateManager gameStateManager => GameManager.Instance.GameStateManager;
    UIManager uiManager => GameManager.Instance.UIManager;

    public void ChangeScene(string sceneName, string spawnPoint)
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        Debug.Log(sceneIndex);
       
        StartCoroutine(LoadSceneAsync(sceneIndex));
        // Set spawn point to the one passed in by trigger.
        _spawnPoint = spawnPoint;
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        gameStateManager.SwitchToState(GameState_Loading.Instance);

        Debug.Log("LoadSceneAsync started for scene ID: " + sceneId);

        // Wait one frame to ensure UI is properly initialized
        yield return null;

        SceneManager.sceneLoaded += OnSceneLoaded;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId);

        // Prevent scene activation until we're ready
        asyncLoad.allowSceneActivation = false;

        float artificialProgress = 0f;
        float minUpdateInterval = 0.005f; // Time between updates in seconds
        float maxUpdateInterval = 0.5f; // Time between updates in seconds
        float minProgressIncrement = 0.005f; // Minimum progress increase per update
        float maxProgressIncrement = 0.05f; // Maximum progress increase per update
        float progressCompletedDelayDuration = 1.0f; // Delay after reaching 100% before completing

        while (!asyncLoad.isDone)
        {
            // Progress goes from 0 to 0.9
            float realProgress = asyncLoad.progress;

            // Gradually increase artificial progress
            artificialProgress = Mathf.MoveTowards(
                artificialProgress,
                realProgress,
                Random.Range(minProgressIncrement, maxProgressIncrement)
            );

            if (realProgress >= 0.9f && artificialProgress >= 0.9f)
            {
                // Set progress to 100% before the hold
                artificialProgress = 1.0f;
                uiManager.loadingController.UpdateProgressBar(artificialProgress);

                Debug.Log("Loading completed, holding for display...");

                // Hold at 100% for desired duration
                yield return new WaitForSeconds(progressCompletedDelayDuration);

                Debug.Log("Hold complete, activating scene...");

                // Now allow the scene to activate
                asyncLoad.allowSceneActivation = true;
            }
            else
            {
                // Normalize progress to 0-1 range
                artificialProgress = Mathf.Clamp01(artificialProgress / 0.9f);
            }

            uiManager.loadingController.UpdateProgressBar(artificialProgress);

            // Wait for the specified interval before next update
            yield return new WaitForSeconds(Random.Range(minUpdateInterval, maxUpdateInterval));
        }
    }


    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Find the player object.
        player = GameObject.Find("Player").transform;

        if (SceneManager.GetActiveScene().buildIndex == 1) // Main menu.
        {
            gameStateManager.SwitchToState(GameState_MainMenu.Instance);

        }
        // Find the spawn point object based on the name passed in
        else 
        {
            Transform spawn = GameObject.Find("SpawnPoint").transform;
            PlayerController pc = player.GetComponent<PlayerController>();
            pc.MovePlayerToSpawnPoint(spawn);
            gameStateManager.SwitchToState(GameState_Gameplay.Instance);
        }

        // Remove the listener to avoid duplicate calls.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
