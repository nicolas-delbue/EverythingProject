using UnityEngine;
using UnityEngine.InputSystem;

public class Hydration : MonoBehaviour
{
    [Header("Hydration Tracking")]
    public bool IsDehydrated;
    [SerializeField]
    private Wallrun wallrun;
    [SerializeField]
    private float DecreaseIncriment;
    public float JumpModifier;
    public float SprintModifier;
    public float WallrunModifier;
    [SerializeField]
    private float MaxHydration;
    private float currentHydration;
    public float CurrentHydration => currentHydration;

    [Header("WaterManagement")]
    [SerializeField]
    private float ContainerMaxWater;
    private float currentContainerWater;
    public float CurrentContainerWater => currentContainerWater;


    private PlayerInput inputActions;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction drinkAction;

    public static Hydration Context;

    private void Awake()
    {
        Context = this;

        inputActions = this.GetComponent<PlayerInput>();
        jumpAction = inputActions.actions["Jump"];
        sprintAction = inputActions.actions["Sprint"];
        drinkAction = inputActions.actions["Drink"];

        currentHydration = MaxHydration;
        currentContainerWater = ContainerMaxWater;
    }
    private void Start()
    {
        //Subscribe To Water Refill
        EventSystem.current.onRefillWater += RefillContainer;
    }
    private void OnDestroy()
    {
        EventSystem.current.onRefillWater -= RefillContainer;
    }
    private void RefillContainer(float RefillAmount)
    {
        currentContainerWater += RefillAmount;
        if(currentContainerWater > ContainerMaxWater)
        {
            currentContainerWater = ContainerMaxWater;
        }
        currentContainerWater = Mathf.Round(currentContainerWater * 100)/100;
    }
    private void Update()
    {
        //Hydration
        CheckJump();
        CheckSprint();
        CheckWallrun();
        CheckHydration();

        //Debug.Log("Current Hydration = "+ currentHydration);
        //Debug.Log("Current Container = "+ currentContainerWater);

        //Rehydration
        DrinkWaterFromContainer();
    }
    private void CheckJump()
    {
        if (jumpAction.triggered && PlayerControl.Context._IsGrounded)
        {
            currentHydration -= DecreaseIncriment*JumpModifier;
        }
    }
    private void CheckSprint()
    {
        if(sprintAction.inProgress)
        {
            currentHydration -= DecreaseIncriment*SprintModifier*Time.deltaTime;
        }
    }
    private void CheckWallrun()
    {
        if(Wallrun.Context.IsWallRunning)
        {
            currentHydration -= DecreaseIncriment*WallrunModifier*Time.deltaTime;
        }
    }
    private void CheckHydration()
    {
        currentHydration = (Mathf.Round(currentHydration * 100)) / 100;
        if(currentHydration <= 0)
        {
            IsDehydrated = true;
            currentHydration = 0;
        }
        else
        {
            IsDehydrated = false;
        }
    }
    private void DrinkWaterFromContainer()
    {
        if(drinkAction.triggered)
        {
            float amountToDrink = MaxHydration - currentHydration;
            float containerLeft = currentContainerWater - amountToDrink;

            if(containerLeft < 0)
            {
                currentHydration += currentContainerWater;
                currentContainerWater = 0;
            }
            else
            {
                currentHydration += amountToDrink;
                currentContainerWater -= amountToDrink;
            }
        }
    }
    public float GetCurrentHydration()
    {
        return currentHydration;
    }
    public float GetCurrentContainer()
    {
        return currentContainerWater;
    }
}
