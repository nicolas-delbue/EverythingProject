using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    //Header Style
    //Accessable 
    //Serialized 
    //Private 

    [Header("Basic Movement")]
    //Accessable Basic Movement Variables
    public float WalkMoveSpeed;
    public float SprintMoveSpeed;
    public float MovementMultiplier;
    public float PlayerDrag;
    //Serialized Basic Movement Variables
    [SerializeField]
    private float SpeedIncriment;
    //Private Basic Movement Variables
    private Vector3 moveDirection;
    private float moveSpeed;
    private float SprintMod;
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
    //Serialized Jump Variables
    [SerializeField]
    private Transform GroundCheck;
    [SerializeField]
    private float raycastDist;
    //Private Jump Variables
    private bool hasJumped = false;
    private bool IsGrounded = false;


    [Header("Crouch and Slide")]
    //Accessable Crouch Varaibles
    public float CrouchMoveSpeed;
    public float CrouchCheckRadius;
    //Serialized Crouch Variables
    [SerializeField]
    private Collider StandCollider;
    [SerializeField]
    private Collider CrouchCollider;
    [SerializeField]
    private Transform HeadCheck;
    //Private Crouch Variables
    private bool IsCrouching = false;

    //[Header("Extra")]

    [Header("Unity Attributes")]
    //Accessable Attributes
    //Serialized Attributes
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private GameObject playerCamera;
    [SerializeField]
    private Vector3 standingCameraPosition;
    [SerializeField]
    private Vector3 crouchingCameraPosition;
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
        ControlDrag();
        //Look
        CameraUpdate();
        //Sprint Check
        SprintCheck();
        //Jump Check
        GroundedCheck();
        Jump();
        //Crouch Check
        CrouchCheck();
        //Slide Check


        //More Checks
        Debug.Log(rb.linearVelocity.magnitude);
    }
    private void FixedUpdate()
    {
        movePlayer();
    }
    private void MovementUpdate()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        moveDirection = transform.forward * move.y + transform.right * move.x;
    }
    private void movePlayer()
    {
        if (isSprinting)
        {
            moveSpeed = SprintMod;
        }
        else if (!isSprinting && IsCrouching)
        {
            moveSpeed = CrouchMoveSpeed;
        }
        else
        {
            moveSpeed = WalkMoveSpeed;
        }
        //else if (isSprinting && IsCrouching)
        //{
        //    //Slide?
        //}


        //Movespeed and move multiplier
        rb.AddForce(moveDirection.normalized * moveSpeed * MovementMultiplier, ForceMode.Acceleration);
    }
    private void ControlDrag()
    {
        rb.linearDamping = 6f;
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
            if (SprintMod >= SprintMoveSpeed)
            {
                SprintMod = SprintMoveSpeed;
            }
            else
            {
                SprintMod += SpeedIncriment;
            }
        }
        else
        {
            SprintMod = WalkMoveSpeed;
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
        hasJumped = jumpAction.triggered;
        if (IsGrounded && hasJumped)
        {
            rb.AddForce(Vector3.up * JumpStrength, ForceMode.Impulse);
        }
    }
    private void CrouchCheck()
    {
        IsCrouching = crouchAction.inProgress;

        if (Physics.CheckSphere(HeadCheck.position, CrouchCheckRadius, GroundMask) && !IsCrouching)
            IsCrouching = true;

        if (IsCrouching)
        {
            StandCollider.enabled = false;
            CrouchCollider.enabled = true;
            //Move Camera
            playerCamera.transform.localPosition = crouchingCameraPosition;
        }
        else
        {
            StandCollider.enabled = true;
            CrouchCollider.enabled = false;
            //Move Camera
            playerCamera.transform.localPosition = standingCameraPosition;
        }
    }
}
