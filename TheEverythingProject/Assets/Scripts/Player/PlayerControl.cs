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
    
    //Serialized Basic Movement Variables
    [SerializeField]
    private float SpeedIncriment;
    [SerializeField]
    private float GroundMovementMultiplier;
    [SerializeField]
    private float AirMovementMultiplier;
    //Private Basic Movement Variables
    private Vector3 moveDirection;
    private float moveSpeed;
    private float SprintMod;
    private bool isSprinting = false;

    //Slope
    RaycastHit SlopeHit;
    Vector3 SlopeMoveDir;

    [Header("Camera")]
    //Accessable Camera Variables
    public float sensitivity;
    public float MaxLookAngle;
    public float MinLookAngle;
    //Private Camera Variables
    private Vector3 lookDirection;
    private float xRot;
    private float yRot;

    [Header("Jumping")]
    //Accessable Jump Variables
    public float JumpStrength;
    public Transform GroundCheckPos;
    public LayerMask GroundMask;
    //Serialized Jump Variables
    [SerializeField]
    private float playerHeight;
    [SerializeField]
    private float jumpCheckRad;
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
    //Private Crouch Variables
    private bool IsCrouching = false;

    //[Header("Extra")]

    [Header("Unity Attributes")]
    //Accessable Attributes
    public float GroundDrag;
    public float AirDrag;
    //Serialized Attributes
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private GameObject playerCamera;
    [SerializeField]
    private Transform orientation;
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
        //Debug.Log(rb.linearVelocity.magnitude);
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void MovementUpdate()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        moveDirection = orientation.forward * move.y + orientation.right * move.x;
        SlopeMoveDir = Vector3.ProjectOnPlane(moveDirection, SlopeHit.normal);
    }
    private void MovePlayer()
    {
        //else if (isSprinting && IsCrouching)
        //{
        //    //Slide?
        //}

        //Movespeed and move multiplier
        if (IsGrounded && !OnSlope())
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * GroundMovementMultiplier, ForceMode.Acceleration);
        }
        else if(IsGrounded && OnSlope())
        {
            rb.AddForce(SlopeMoveDir.normalized * moveSpeed * GroundMovementMultiplier, ForceMode.Acceleration);
        }
        else if (!IsGrounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * AirMovementMultiplier, ForceMode.Acceleration);
        }
        
    }
    private void ControlDrag()
    {
        if(!IsGrounded)
        {
            rb.linearDamping = AirDrag;
        }
        else
        {
            rb.linearDamping = GroundDrag;
        }
    }
    private void CameraUpdate()
    {
        Vector2 look = lookAction.ReadValue<Vector2>();

        yRot += look.x * sensitivity * Time.deltaTime;
        xRot -= look.y * sensitivity * Time.deltaTime;

        xRot = Mathf.Clamp(xRot, MinLookAngle, MaxLookAngle);

        playerCamera.transform.rotation = Quaternion.Euler(xRot, yRot, 0);
        orientation.transform.rotation = Quaternion.Euler(0f, yRot, 0f);

        //rotate the model

        //gameObject.transform.Rotate(0f, look.x * sensitivity * Time.deltaTime, 0f);
        //playerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
    }
    private void SprintCheck()
    {
        isSprinting = sprintAction.inProgress;
        if (isSprinting && IsGrounded)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, SprintMoveSpeed, SpeedIncriment * Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, WalkMoveSpeed, SpeedIncriment * Time.deltaTime);
        }
    }
    private void GroundedCheck()
    {
        if (Physics.CheckSphere(GroundCheckPos.position, jumpCheckRad, GroundMask))
            IsGrounded = true;
        else
            IsGrounded = false;
    }
    private void Jump()
    {
        hasJumped = jumpAction.triggered;
        if (IsGrounded && hasJumped)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x,0,rb.linearVelocity.z);
            rb.AddForce(Vector3.up * JumpStrength, ForceMode.Impulse);
        }
    }
    private void CrouchCheck()
    {
        IsCrouching = crouchAction.inProgress;

        if (Physics.CheckSphere(transform.position, CrouchCheckRadius, GroundMask) && !IsCrouching)
            IsCrouching = true;

        if (IsCrouching)
        {
            StandCollider.enabled = false;
            CrouchCollider.enabled = true;
            moveSpeed = CrouchMoveSpeed;
            //Move Camera
            //playerCamera.transform.localPosition = crouchingCameraPosition;
        }
        else
        {
            StandCollider.enabled = true;
            CrouchCollider.enabled = false;
            moveSpeed = WalkMoveSpeed;
            //Move Camera
            //playerCamera.transform.localPosition = standingCameraPosition;
        }
    }
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out SlopeHit, playerHeight / 2 + 0.5f))
        {
            if (SlopeHit.normal != Vector3.up)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }
    public bool GetCrouched()
    {
        return IsCrouching;
    }
}
