using UnityEngine;

public class BedInteractable : Interactable
{
    public float ReduceAmount = 0;
    protected override void HandleInteraction(PlayerInteraction interactor)
    {
        EventSystem.current.TempDown(ReduceAmount);
    }
}
