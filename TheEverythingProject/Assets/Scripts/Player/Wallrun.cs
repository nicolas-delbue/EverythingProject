using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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
    [SerializeField]
    private float wallGrav;
    public float WallJumpForce;

    bool wallLeft = false;
    bool wallRight = false;
    private RaycastHit leftHit;
    private RaycastHit rightHit;

    private PlayerInput inputActions;
    private InputAction jumpAction;

    public bool IsWallRunning = false;

    [SerializeField]
    private bool runOnce = false;
    private IEnumerator coroutine;

    private Vector3 wallRunDir;
    public Vector3 WallDir => wallRunDir;

    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Hydration hydration;

    private void Awake()
    {
        //Inputs
        inputActions = this.GetComponent<PlayerInput>();
        jumpAction = inputActions.actions["Jump"];
        IsWallRunning = false;
        runOnce = false;
        wallGrav = wallRunGravity;
        coroutine = Gravity(2.0f);
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
        if (CanWallRun() && !hydration.IsDehydrated && rb.linearVelocity.z != 0)
        {
            if(wallLeft)
            {
                StartWallRun();
                wallRunDir = new Vector3(-leftHit.normal.z, 0, leftHit.normal.x);
            }
            else if(wallRight)
            {
                StartWallRun();
                wallRunDir = new Vector3(rightHit.normal.z, 0, -rightHit.normal.x); ;
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

        //if (!runOnce)
        //{
        //    runOnce = true;
        //    StartCoroutine(coroutine);
        //}

        rb.AddForce(Vector3.down * wallGrav, ForceMode.Force);

        IsWallRunning = true;

        if (jumpAction.triggered)
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
        IsWallRunning = false;
        //StopCoroutine(coroutine);
        //runOnce = false;
    }
    private IEnumerator Gravity(float time)
    {
        wallGrav = 0;
        yield return new WaitForSeconds(time);
        wallGrav = wallRunGravity;
    }
}
