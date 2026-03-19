using UnityEngine;

public class TestInteract : Interactable
{
    protected override void HandleInteraction(PlayerInteraction interactor)
    {
        Debug.Log("Hello");
    }
}
