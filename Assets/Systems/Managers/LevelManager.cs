using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    Transform player;
    string _spawnPoint;

    public void ChangeScene(string sceneName, string spawnPoint)
    {
        // Set spawn point to the one passed in by trigger.
        _spawnPoint = spawnPoint;
        // Subscribe to the sceneLoaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
        // Begin loading the scene.
        SceneManager.LoadScene(sceneName);

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

        // Move the player to the spawn point.
        player.position = spawn.position;
        player.rotation = spawn.rotation;

        // Remove the listener to avoid duplicate calls.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
