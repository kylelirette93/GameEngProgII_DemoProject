using UnityEngine;

// Initialize first
[DefaultExecutionOrder(-100)]

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Manager References")]
    public InputManager inputManager;

    void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion

        // Assign a reference ot input manager if it doesnt exist.
        if (inputManager == null) inputManager = GetComponentInChildren<InputManager>();
    }
}
