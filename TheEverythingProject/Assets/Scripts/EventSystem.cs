using System;
using UnityEngine;

public class EventSystem : MonoBehaviour
{
    public static EventSystem current;
    public void Awake()
    {
        if (current != null)
        {
            Debug.LogWarning("Two instances of EvenControllerHub in Scene");
        }
        current = this;
    }

    public event Action<float> onRefillWater;
    public void RefillWater(float RefillAmount)
    {
        if(onRefillWater != null)
        {
            onRefillWater(RefillAmount);
        }
    }
    public event Action<float> onTempDown;
    public void TempDown(float reduceAmount)
    {
        if(onTempDown != null)
        {
            onTempDown(reduceAmount);
        }
    }
    public event Action<bool, string, string, float> onInteractionDetected;
    public void InteractionDetected(bool detect, string name, string type, float time)
    {
        if (onInteractionDetected != null)
        {
            onInteractionDetected(detect, name, type, time);
        }
    }
    public event Action<float> onInteractionInteracting;
    public void InteractionInteracting(float time)
    {
        if (onInteractionInteracting != null)
        {
            onInteractionInteracting(time);
        }
    }
}
