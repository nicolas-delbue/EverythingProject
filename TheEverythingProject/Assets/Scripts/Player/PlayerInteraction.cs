using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInteraction : MonoBehaviour
{
    private PlayerInput inputActions;
    private InputAction interactAction;

    [SerializeField]
    Transform PlayerCamera;
    [SerializeField]
    float interactionRange;
    [SerializeField]
    LayerMask InteractMask;

    Interactable interactable;
    private float interactionTime;     //Use for UI
    private float interactionProgress; //Use for UI
    private bool interactDetected;     //Use for UI
    private bool singleInteract = false;

    private void Awake()
    {
        //Inputs
        inputActions = this.GetComponent<PlayerInput>();
        interactAction = inputActions.actions["Interact"];
    }
    private void Update()
    {
        InteractionCheck();
    }
    void InteractionCheck()
    {
        //Somewhere in here needs to be a call to UI for interaction ui to appear
        if (Physics.Raycast(PlayerCamera.position, PlayerCamera.forward, out RaycastHit rayHit, interactionRange, InteractMask))
        {
            interactable = rayHit.collider.gameObject.GetComponent<Interactable>();
            if (interactable.canInteract)
            {
                interactDetected = true;
                //Debug.Log("Show Interactable is: " + interactable.InteractName);
                //Send Off Interactable/Info
                EventSystem.current.InteractionDetected(interactDetected, interactable.InteractName, interactable.InteractType, interactable.InteractTime);
            }
            else
            {
                interactDetected = false;
                EventSystem.current.InteractionDetected(interactDetected, interactable.InteractName, interactable.InteractType, interactable.InteractTime);
            }
        }
        else
        {
            interactDetected = false;
            EventSystem.current.InteractionDetected(interactDetected, "NA", "NA", 1f);
            interactionTime = 0;
        }

        if (interactAction.inProgress && interactDetected)
        {
            interactionTime += Time.deltaTime;
            interactionProgress = (interactionTime / interactable.InteractTime);

            if (interactionProgress >= 1)
            {
                if (!singleInteract)
                {
                    interactable.Interact(this);
                    singleInteract = true;
                }
            }
            EventSystem.current.InteractionInteracting(interactionProgress);
        }
        else if (!interactDetected)
        {
            //Debug.Log("No interactable found");
            interactable = null;
            interactionTime = 0;
        }
        else if (!interactAction.inProgress)
        {
            interactionTime = 0;
            singleInteract = false;
        }
        else
        {
            interactionTime = 0;
        }
    }
}
