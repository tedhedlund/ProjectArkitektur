using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [Header("Character movement settings")]
    [SerializeField] private float crouchWalkSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float maxSprintTime;
    [SerializeField] private float sprintCoolDownTime;
    [SerializeField] private float standUpSpeed;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float groundStandOffset;
    [SerializeField] private float groundCrouchOffset;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private float crouchHeight;
    [SerializeField] private Transform sphereCheck;
    [SerializeField] private AudioManager audioManager;

    [Header("Player sound settings")]
    [SerializeField] private float crouchStepTime;
    [SerializeField] private float walkingStepTime;
    [SerializeField] private float sprintStepTime;

    private CharacterController player;
    private LayerMask groundLayer;

    private Vector3 direction;
    private Vector3 inputHorizontal;
    private Vector3 inputVertical;

    private float verticalForce;
    private float sphereCheckRadius;
    private float groundedOffset;
    private float gravity;
    private float standHeight;
    private float steptimer;
    private float currentStepTime;

    [Header("Misc")]
    public float sprintTimer;
    public float moveSpeed;

    private bool onGround;
    private bool canSprint = true;

    public bool ads = false;
    public bool sprinting = false;

    public enum CrouchStatus { standing, crouching }
    public CrouchStatus crouchStatus = CrouchStatus.standing;

    public enum MoveStatus { idle, walking, sprinting, leftStrafe, rightStrafe }
    public MoveStatus moveStatus = MoveStatus.idle;


    void Start()
    {
        groundLayer = LayerMask.GetMask("Ground");
        player = GetComponent<CharacterController>();
        gravity = -9.82f;
        sphereCheckRadius = 0.4f;
        groundCrouchOffset = -1.049f;
        groundStandOffset = -1.699f;
        standHeight = player.height;
    }

    
    void Update()
    {
        CharacterMove();
    }

    private void CharacterMove()
    {
        // Set bool if player is/is not standing on the ground layer.
        groundedOffset = crouchStatus == CrouchStatus.standing ? groundStandOffset : groundCrouchOffset;
        sphereCheck.position = new Vector3(transform.position.x, transform.position.y + groundedOffset, transform.position.z);
        onGround = Physics.CheckSphere(sphereCheck.position, sphereCheckRadius, groundLayer);

        HandleADS();
        HandleMove();
        HandleCrouch();
        HandleJump();
    }

    private void HandleCrouch()
    {
        if (onGround && Input.GetKeyDown(KeyCode.LeftControl))
        {
            ValidateStand();
        }
        ToggleCrouch();
    }

    private void HandleADS()
    {
        if (Input.GetMouseButtonDown(1))
        {
            ads = !ads;
        }
    }

    private void ToggleCrouch()
    {
        if (crouchStatus == CrouchStatus.crouching && player.height > crouchHeight)
        {
            player.height = Mathf.Lerp(player.height, crouchHeight, crouchSpeed);
        }
        else if(crouchStatus == CrouchStatus.standing && player.height < standHeight)
        {
            player.height = Mathf.Lerp(player.height, standHeight, standUpSpeed);
        }
    }

    private void ValidateStand()
    {
        if (crouchStatus == CrouchStatus.crouching)
        {
            RaycastHit standHit;
            if (Physics.Raycast(transform.position, Vector3.up, out standHit))
            {
                if (standHit.distance > standHeight + 0.1f)
                {
                    crouchStatus = CrouchStatus.standing;
                }                                
            }
            else crouchStatus = CrouchStatus.standing;
        }
        else crouchStatus = CrouchStatus.crouching;
    }

    private void HandleJump()
    {
        // If player is on ground, stop velocity from building up.
        // Else, keep adding gravity force.
        if (verticalForce < 0 && onGround)
        {
            verticalForce = -2f;
        }

        if (onGround && Input.GetKeyDown(KeyCode.Space))
        {
            // If player wants to jump but is crouch, then stand up instead
            if (crouchStatus == CrouchStatus.crouching)
            {
                ValidateStand();
                ToggleCrouch();
            }
            // Else jump
            else verticalForce = jumpSpeed;
        }

        // Apply gravity force to the force accumulator.
        verticalForce += jumpMultiplier * gravity * Time.deltaTime;

        // Move player up and down with the vertical force vector.
        player.Move(new Vector3(0f, verticalForce, 0f) * Time.deltaTime);
    }

    private void HandleMove()
    {
        // Get input from AD and WS.
        // Transfrom them from global space to local space with right and forward vectors of the CC.
        inputHorizontal = Input.GetAxis("Horizontal") * transform.right;
        inputVertical = Input.GetAxis("Vertical") * transform.forward;

        // Create new direction vector from the given transformed input vectors.
        direction = inputHorizontal + inputVertical;

        // Start sprinting.
        // If player holds shift, sprint until exhausted by maxSprintTime.
        if (Input.GetKey(KeyCode.LeftShift) && canSprint && moveStatus != MoveStatus.idle)
        {
            moveSpeed = sprintSpeed;
            sprintTimer += Time.deltaTime;
            ads = false;
            moveStatus = MoveStatus.sprinting;

            if (sprintTimer >= maxSprintTime)
            {
                canSprint = false;
                sprinting = false;
            }
        }
        else if (crouchStatus == CrouchStatus.crouching || ads)
        {
            // Check if player is crouched and moving or if crouch and idle
            if (inputHorizontal != Vector3.zero || inputVertical != Vector3.zero)
            {
                moveStatus = MoveStatus.walking;
            }
            else moveStatus = MoveStatus.idle;

            moveSpeed = crouchWalkSpeed;
            canSprint = false;
            sprinting = false;
        }
        else
        {
            // Check if player is idle or moving
            if (inputHorizontal == Vector3.zero && inputVertical == Vector3.zero)
            {
                moveStatus = MoveStatus.idle;
            }
            else if(inputHorizontal.x > 0 && inputVertical == Vector3.zero)
            {
                // Ej implementerad ännu
                //moveStatus = MoveStatus.rightStrafe;
            }
            else if(inputHorizontal.x < 0 && inputVertical == Vector3.zero)
            {
                // Ej implementerad ännu
                //moveStatus = MoveStatus.leftStrafe;
            }
            else moveStatus = MoveStatus.walking;


            moveSpeed = walkSpeed;
            sprinting = false;
            if (sprintTimer > 0) sprintTimer -= Time.deltaTime;
        }

        // Cooldown timer to reset the ability to sprint.
        if (!canSprint)
        {
            sprintTimer -= Time.deltaTime;
            if (sprintTimer < maxSprintTime - sprintCoolDownTime && crouchStatus != CrouchStatus.crouching)
            {
                canSprint = true;
                sprintTimer = 0;
            }         
        }

        // Move player horizontaly with direction vector.
        // Clamp the magnitude of the direction vector to max 1
        // in order to avoid the player moving faster diagonally.
        player.Move(Vector3.ClampMagnitude(direction, 1f) * moveSpeed * Time.deltaTime);

        HandlePlayerSound();
    }

    private void HandlePlayerSound()
    {
        switch (moveStatus)
        {
            case MoveStatus.walking:
                // If walking crouched
                if (crouchStatus == CrouchStatus.crouching)
                {
                    currentStepTime = crouchStepTime;
                }
                // If walking standing up
                else currentStepTime = walkingStepTime;
                break;

            case MoveStatus.sprinting:
                currentStepTime = sprintStepTime;
                break;
            default:
                break;
        }

        steptimer += Time.deltaTime;
        // Dont play walking sound if jumping/in air
        if (onGround && moveStatus != MoveStatus.idle)
        {
            
            if (steptimer >= currentStepTime)
            {
                audioManager.walkingStep.Play();
                steptimer = 0;
            }
        }
    }
}
