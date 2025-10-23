using UnityEngine;

public class SceneChangeTrigger : MonoBehaviour
{
    public string sceneName;
    public string spawnPoint;

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.LevelManager.ChangeScene(sceneName, spawnPoint);
    }

    private void OnDrawGizmos()
    {
       // Draw a cube to match box collider size.
       BoxCollider boxCollider = GetComponent<BoxCollider>();
         if (boxCollider != null)
         {
              Gizmos.color = new Color(0, 1, 0, 0.5f);
              Gizmos.matrix = transform.localToWorldMatrix;
              Gizmos.DrawCube(boxCollider.center, boxCollider.size);
        }
    }
}
