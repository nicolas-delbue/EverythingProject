using UnityEngine;
using UnityEngine.InputSystem;

public class Wallrun : MonoBehaviour
{
    [SerializeField]
    Transform orientation;

    [Header("Wall Run Components")]
    [SerializeField]
    float wallDistance = 0.5f;
    [SerializeField]
    float minJumpHeight = 1.5f;
    [SerializeField]
    private float wallRunGravity;
    public float WallJumpForce;

    bool wallLeft = false;
    bool wallRight = false;
    private RaycastHit leftHit;
    private RaycastHit rightHit;

    private PlayerInput inputActions;
    private InputAction jumpAction;


    [SerializeField]
    private Rigidbody rb;

    private void Awake()
    {
        //Inputs
        inputActions = this.GetComponent<PlayerInput>();
        jumpAction = inputActions.actions["Jump"];
    }

    bool CanWallRun()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight);
    }
    void CheckWall()
    {
        wallLeft = Physics.Raycast(transform.position, -orientation.right, out leftHit, wallDistance);
        wallRight = Physics.Raycast(transform.position, orientation.right, out rightHit, wallDistance);
    }
    private void Update()
    {
        CheckWall();
        if(CanWallRun())
        {
            if(wallLeft)
            {
                StartWallRun();
                Debug.Log("On your left");
            }
            else if(wallRight)
            {
                StartWallRun();
                Debug.Log("On your right");
            }
            else
            {
                StopWallRun();
            }
        }
        else
        {
            StopWallRun();
        }
    }
    //Updates: Maintain movement?
    private void StartWallRun()
    {
        rb.useGravity = false;

        rb.AddForce(Vector3.down * wallRunGravity, ForceMode.Force);

        if(jumpAction.triggered)
        {
            if(wallLeft)
            {
                Vector3 wallRunJumpDir = transform.up + leftHit.normal;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(wallRunJumpDir.normalized*WallJumpForce*100, ForceMode.Force);
            }
            else if(wallRight)
            {
                Vector3 wallRunJumpDir = transform.up + rightHit.normal;
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
                rb.AddForce(wallRunJumpDir.normalized * WallJumpForce*100, ForceMode.Force);
            }
            else
            {
                Debug.Log("What wall are you on????");
            }
        }
    }
    private void StopWallRun()
    {
        rb.useGravity = true;
    }
}
