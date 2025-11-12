using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-100)]
public static class PerformBootload
{
    const string SceneName = "BootLoader";
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Execute()
    {
        if (SceneManager.GetActiveScene().name != SceneName)
        {
            // Check all currently loaded scenes to check if bootstrap is already loaded.
            for (int sceneIndex = 0; sceneIndex < SceneManager.sceneCount; sceneIndex++)
            {
                var candidateScene = SceneManager.GetSceneAt(sceneIndex);

                if (candidateScene.name == SceneName)
                {
                    return;
                }
            }
            Debug.Log("Loading bootloader scene" + SceneName);

            SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
        }
    }
}
public class BootLoader : MonoBehaviour
{
    public static BootLoader Instance { get; private set; } = null;

    private void Awake()
    {
        #region Singleton
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        #endregion
    }

    public void Test()
    {
        Debug.Log("Bootloader scene is active");
    }
}
