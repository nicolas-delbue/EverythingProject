using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Basic Movement")]
    public float BaseMoveSpeed;
    public float MaxSprintMoveSpeed;
    private Vector3 moveDirection;

    /*[Header("Jumping")]
    [Header("Crouch and Slide")]
    [Header("Extra")]*/

    [Header("Unity Attributes")]
    private PlayerInput inputActions;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction crouchAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private Rigidbody rigidbody;

    private void Awake()
    {
        //Inputs
        inputActions = this.GetComponent<PlayerInput>();
        moveAction = inputActions.actions["Move"];
        lookAction = inputActions.actions["Look"];
        crouchAction = inputActions.actions["Crouch"];
        jumpAction = inputActions.actions["Jump"];
        sprintAction = inputActions.actions["Sprint"];

        //RigidBody
        rigidbody = this.GetComponent<Rigidbody>();
    }
    void Start()
    {
        //Input Events

        //Other Events

        //Base Variables
    }
    void Update()
    {
        //Basic Movement
        Vector2 move = moveAction.ReadValue<Vector2>();
        moveDirection = new Vector3(move.x,0,move.y).normalized;

        //Sprint Check

        //Jump Check
        //inputActions.Player.Jump.triggered //Bool when you trigger the button
        //Crouch Check

        //Slide Check


        //More Checks
    }
    private void FixedUpdate()
    {
        //Basic Movement
        movePlayer(moveDirection);
    }
    private void movePlayer(Vector3 dir)
    {
        rigidbody.linearVelocity = dir * BaseMoveSpeed * Time.fixedDeltaTime;
    }
}
