using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    //Header Style
    //Accessable 
    //Serialized 
    //Private 

    [Header("Basic Movement")]
    //Accessable Basic Movement
    public float MaxBaseMoveSpeed;
    public float BaseSpeedMod;
    public float MaxSprintMoveSpeed;
    public float SpeedModIncriment;
    //Private Basic Movement
    private Vector3 moveDirection;
    private float SpeedMod = 0;
    bool isSprinting = false;

    [Header("Camera")]
    //Accessable Camera Variables
    public float sensitivity;
    public float MaxLookAngle;
    public float MinLookAngle;
    //Private Camera Variables
    private Vector3 lookDirection;
    private float xRot;

    [Header("Jumping")]
    //Accessable Jump Variables
    public float JumpStrength;
    public LayerMask GroundMask;
    //Private Jump Variables
    private bool hasJumped = false;
    private bool IsGrounded = false;
    [SerializeField]
    private Transform GroundCheck;
    [SerializeField]
    private float raycastDist;

    /*[Header("Crouch and Slide")]
    [Header("Extra")]*/

    [Header("Unity Attributes")]
    //Accessable Attributes
    //Serialized Attributes
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private GameObject playerCamera;
    //Private Attributes
    private PlayerInput inputActions;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction crouchAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    

    private void Awake()
    {
        //Inputs
        inputActions = this.GetComponent<PlayerInput>();
        moveAction = inputActions.actions["Move"];
        lookAction = inputActions.actions["Look"];
        crouchAction = inputActions.actions["Crouch"];
        jumpAction = inputActions.actions["Jump"];
        sprintAction = inputActions.actions["Sprint"];

        Cursor.lockState = CursorLockMode.Locked;
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
        MovementUpdate();
        movePlayer(moveDirection);
        //Look
        CameraUpdate();
        //Sprint Check
        SprintCheck();
        //Jump Check
        hasJumped = jumpAction.triggered;
        GroundedCheck();
        Jump();
        //Crouch Check

        //Slide Check


        //More Checks
        Debug.Log(rb.linearVelocity.magnitude);
        Debug.Log(rb.maxLinearVelocity);
    }
    private void FixedUpdate()
    {
        //Basic Movement
        
        //Jump
    }
    private void MovementUpdate()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        moveDirection = transform.forward * move.y + transform.right * move.x;
    }
    private void movePlayer(Vector3 dir)
    {
        if(isSprinting)
        {
            rb.maxLinearVelocity = MaxBaseMoveSpeed + MaxSprintMoveSpeed;
        }
        else
        {
            rb.maxLinearVelocity = MaxBaseMoveSpeed;
        }

        float TotalMoveSpeedMod = BaseSpeedMod + SpeedMod;
        dir *= TotalMoveSpeedMod * Time.fixedDeltaTime;

        var movementPlane = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.y);
        if (movementPlane.magnitude < rb.maxLinearVelocity)
        {
            rb.AddForce(dir, ForceMode.Force);
        }
    }
    private void CameraUpdate()
    {
        Vector2 look = lookAction.ReadValue<Vector2>();
        xRot -= look.y * sensitivity * Time.deltaTime;
        xRot = Mathf.Clamp(xRot, MinLookAngle, MaxLookAngle);
        gameObject.transform.Rotate(0, look.x * sensitivity * Time.deltaTime, 0);
        playerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }
    private void SprintCheck()
    {
        isSprinting = sprintAction.inProgress;
        if (isSprinting)
        {
            if (SpeedMod >= MaxSprintMoveSpeed)
            {
                SpeedMod = MaxSprintMoveSpeed;
            }
            else
            {
                SpeedMod += SpeedModIncriment;
            }
        }
        else
        {
            SpeedMod = 0;
        }
    }
    private void GroundedCheck()
    {
        RaycastHit hit;
        if(Physics.Raycast(GroundCheck.position, Vector3.down, out hit, raycastDist, GroundMask))
            IsGrounded = true;
        else
            IsGrounded = false;
    }
    private void Jump()
    {
        if (IsGrounded && hasJumped)
        {
            rb.AddForce(Vector3.up * JumpStrength, ForceMode.Impulse);
        }
    }
}
