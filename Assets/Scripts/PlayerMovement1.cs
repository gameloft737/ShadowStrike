using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    [Header("Movement")]

    [SerializeField] Animator animator;
    public float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float climbSpeed;

    public float groundDrag;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;


    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;
    
    [Header("References")]
    public Transform orientation;
    public Climbing climbingScript;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    public Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        crouching,
        climbing,
        air,
        idle
    }

    public WalkingDirection direction;
    public enum WalkingDirection
    {
        forward,
        left,
        right,
        back
    }

    public bool climbing;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        // ground check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();
        SetAnimStates();

        // handle drag
        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void SetAnimStates()
    {
        if (ThirdPersonCamera.currentStyle == ThirdPersonCamera.CameraStyle.Combat)
        {
            if (direction == WalkingDirection.right)
            {
                animator.SetBool("isRight", true);
                animator.SetBool("isLeft", false);
            }
            else if (direction == WalkingDirection.left)
            {
                animator.SetBool("isRight", false);
                animator.SetBool("isLeft", true);
            }
            else
            {
                animator.SetBool("isRight", false);
                animator.SetBool("isLeft", false);
            }
            if (direction == WalkingDirection.back)
            {
                animator.SetBool("isBack", true);
            }
            else
            {
                animator.SetBool("isBack", false);
            }
        }
        else
        {
            animator.SetBool("isRight", false);
            animator.SetBool("isLeft", false);
        }
        //moving
        if (state == MovementState.idle)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            animator.SetBool("isCrouching", false);
        }
        else if (state== MovementState.walking)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("isCrouching", false);
        }
        else if (state== MovementState.sprinting && rb.velocity.magnitude > 0.1)
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", true);
            animator.SetBool("isCrouching", false);
        }
        else if (state == MovementState.crouching)
        {
            Debug.Log("crouhing");
            if(rb.velocity.magnitude < 0.1)
                animator.SetBool("isWalking", false);
            else
                animator.SetBool("isWalking", true);
            animator.SetBool("isRunning", false);
            animator.SetBool("isCrouching", true);
        }
    }
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput > 0.1)
        {
            direction = WalkingDirection.right;
        }
        else if (horizontalInput < -0.1)
        {
            direction = WalkingDirection.left;
        }
        else if (horizontalInput == 0)
        {
            if(verticalInput < -0.1)
            {
                direction = WalkingDirection.back;
            }
            else
            {
                direction = WalkingDirection.forward;
            }
        }
        // when to jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            if (grounded)
            {
                 transform.position = new Vector3(transform.position.x, transform.position.y - crouchYScale, transform.position.z);
            }
            else
            {
                rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
            }
            
            
            

        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            
            
        }
    }

    private void StateHandler()
    {

        //climbing mode
        if (climbing)
        {
            state = MovementState.climbing;
            moveSpeed = climbSpeed;
        }
        // Mode - Crouching
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
        }

        // Mode - Sprinting
        else if(grounded && Input.GetKey(sprintKey) && rb.velocity.magnitude > 0.1)
        {
            Debug.Log("sprit");
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded && rb.velocity.magnitude > 0.1)
        {
            Debug.Log("walk");
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        else if (grounded && rb.velocity.magnitude <= 0.1)
        {
            Debug.Log("idle");
            state = MovementState.idle;
        }
        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        if(climbingScript.exitingWall) return;

        // calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // on slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }

        // on ground
        else if(grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // in air
        else if(!grounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);

        // turn gravity off while on slope
        rb.useGravity = !OnSlope();
    }

    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}