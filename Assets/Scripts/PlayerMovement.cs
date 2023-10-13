using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]

    private float animHorizontal;
    private float animVertical;

    [SerializeField] float animSmoothingSpeed = 2;

    [SerializeField] float animDst = 0.7f;
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
        
        animator.SetBool("isMoving",horizontalInput != 0 || verticalInput != 0);

        float multiplier;
        if (state == MovementState.sprinting || state == MovementState.crouching)
        {
            multiplier = 1;
        }
        else
        {
            multiplier = animDst;
        }
        float targetHorizontal = horizontalInput * multiplier;
        float targetVertical = verticalInput * multiplier;

        //stops jittering
        if (Mathf.Abs(animHorizontal - targetHorizontal) < 0.05f)
        {
            animHorizontal = targetHorizontal;
        }
        if (Mathf.Abs(animVertical - targetVertical) < 0.05f)
        {
            animVertical = targetVertical;
        }

        if (animHorizontal < targetHorizontal)
        {
            animHorizontal += Time.deltaTime * animSmoothingSpeed;
        }
        else if (animHorizontal > targetHorizontal)
        {
            animHorizontal -= Time.deltaTime * animSmoothingSpeed;
        }

        if (animVertical < targetVertical)
        {
            animVertical += Time.deltaTime * animSmoothingSpeed;
        }
        else if (animVertical > targetVertical)
        {
            animVertical -= Time.deltaTime * animSmoothingSpeed;
        }

        if(state == MovementState.climbing)
        {
            animator.SetBool("isAir", false);
            animator.SetBool("isClimbing", true);
            animator.SetFloat("vertical", Mathf.Ceil(animVertical)); 
        }
        else if (state == MovementState.air)
        {
            animator.SetBool("isAir", true);
        }
        else
        {
            if(state == MovementState.crouching)
            {
                animator.SetBool("isCrouching", true);
            }
            else
            {
                animator.SetBool("isCrouching", false);
            }
            animator.SetBool("isAir", false);
            animator.SetBool("isClimbing", false);
            
            if (ThirdPersonCamera.currentStyle == ThirdPersonCamera.CameraStyle.Combat)
            {
                
                animator.SetBool("isCombatCam", true);
                Debug.Log(animHorizontal.ToString() + "         " + animVertical.ToString());
                animator.SetFloat("horizontal", animVertical);
                animator.SetFloat("vertical", animHorizontal);
                
            }
            else
            {     
                animator.SetBool("isCombatCam", false);
                animator.SetFloat("vertical", Mathf.Max(Mathf.Abs(animVertical), Mathf.Abs(animHorizontal)));  
                
            }
        }
        
    }
        
    
    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        verticalInput = Input.GetAxisRaw("Vertical");

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

            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
        }

        // Mode - Walking
        else if (grounded && rb.velocity.magnitude > 0.1)
        {

            state = MovementState.walking;
            moveSpeed = walkSpeed;
        }

        else if (grounded && rb.velocity.magnitude <= 0.1)
        {

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
        animator.SetTrigger("Jump");
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
