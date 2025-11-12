using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    Transform player;
    string _spawnPoint;

    public void ChangeScene(string sceneName, string spawnPoint)
    {
        LoadSceneAsync(SceneManager.GetSceneByName(sceneName).buildIndex);
        // Set spawn point to the one passed in by trigger.
        _spawnPoint = spawnPoint;
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Begin loading the scene.
        SceneManager.LoadScene(sceneName);
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        GameManager.Instance.GameStateManager.SwitchToState(GameState_Loading.Instance);

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId);

        while (asyncLoad.isDone == false)
        {
            float progressValue = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            GameManager.Instance.UIManager.loadingController.UpdateProgressBar(progressValue);
            yield return null;
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

        // Find the spawn point object based on the name passed in
        Transform spawn = GameObject.Find(_spawnPoint).transform;

        PlayerController pc = player.GetComponent<PlayerController>();
        pc.MovePlayerToSpawnPoint(spawn);

        // Remove the listener to avoid duplicate calls.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
