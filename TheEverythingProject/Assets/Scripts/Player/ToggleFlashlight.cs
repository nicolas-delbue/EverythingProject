using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleFlashlight : MonoBehaviour
{
    private PlayerInput inputActions;
    private InputAction flashlightAction;

    [SerializeField]
    private Light flashlightLight;

    private bool flashlightEnabled;

    private void Awake()
    {
        //Inputs
        inputActions = this.GetComponent<PlayerInput>();
        flashlightAction = inputActions.actions["FlashLight"];
        flashlightEnabled = false;
    }
    private void Update()
    {
        flashlightLight.enabled = flashlightEnabled;
        
        if(flashlightAction.triggered)
        {
            flashlightEnabled = !flashlightEnabled;
        }
    }

}
