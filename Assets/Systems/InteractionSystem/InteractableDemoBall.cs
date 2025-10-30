using UnityEngine;

public class InteractableDemoBall : BaseInteractable
{
   public override void OnInteract()
    {
        Debug.Log("Using logic from interactable demo ball.");
        gameObject.SetActive(false);
    }
}
