using UnityEngine;

public class WaterFountain : Interactable
{
    public float MaxRefillAmount;
    private float currentAmount;

    public override void Initialize()
    {
        currentAmount = MaxRefillAmount;
    }
    protected override void HandleInteraction(PlayerInteraction interactor)
    {
        EventSystem.current.RefillWater(currentAmount);
        currentAmount = 0;
    }
    public override void Update()
    {
        if (currentAmount < MaxRefillAmount)
        {
            currentAmount += 1 * Time.deltaTime;
        }
    }
}
