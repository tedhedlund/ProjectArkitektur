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
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private float crouchYscale;
    [SerializeField] private Transform sphereCheck;

    private CharacterController player;
    private LayerMask groundLayer;

    private Vector3 standCheck;
    private Vector3 direction;
    private Vector3 inputHorizontal;
    private Vector3 inputVertical;

    private float verticalForce;
    private float sphereCheckRadius;
    private float standingYscale;
    private float standingHeight;
    private float gravity;
    public float sprintTimer;
    public float moveSpeed;

    private bool onGround;
    private bool canSprint = true;

    private enum MoveStatus { standing, crouching, sprinting }
    private MoveStatus moveStatus = MoveStatus.standing;


    void Start()
    {
        standCheck = GameObject.Find("StandCheck").transform.position;
        groundLayer = LayerMask.GetMask("Ground");
        player = GetComponent<CharacterController>();
        gravity = -9.82f;
        sphereCheckRadius = 0.4f;
        standingYscale = transform.localScale.y;
        standingHeight = transform.position.y;
    }

    
    void Update()
    {
        CharacterMove();
    }

    private void CharacterMove()
    {
        // Set bool if player is/is not standing on the ground layer.
        onGround = Physics.CheckSphere(sphereCheck.position, sphereCheckRadius, groundLayer);

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

    private void ToggleCrouch()
    {
        if (moveStatus == MoveStatus.crouching && transform.localScale.y > crouchYscale)
        {
            transform.localScale -= new Vector3(0, crouchSpeed, 0);
        }
        else if(moveStatus == MoveStatus.standing && transform.localScale.y < standingYscale)
        {
            transform.localScale += new Vector3(0, standUpSpeed, 0);
        }
    }

    private void ValidateStand()
    {
        if (moveStatus == MoveStatus.crouching)
        {
            RaycastHit standHit;
            if (Physics.Raycast(transform.position, Vector3.up, out standHit))
            {
                if (standHit.distance > standingHeight + 0.1f)
                {
                    moveStatus = MoveStatus.standing;
                }                                
            }
            else moveStatus = MoveStatus.standing;
        }
        else moveStatus = MoveStatus.crouching;
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
            if (moveStatus == MoveStatus.crouching)
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
        if (Input.GetKey(KeyCode.LeftShift) && canSprint)
        {
            moveSpeed = sprintSpeed;
            sprintTimer += Time.deltaTime;
            if (sprintTimer >= maxSprintTime)
            {
                canSprint = false;
            }
        }
        else if (moveStatus == MoveStatus.crouching)
        {
            moveSpeed = crouchWalkSpeed;
            canSprint = false;
        }
        else
        {
            moveSpeed = walkSpeed;
            if (sprintTimer > 0) sprintTimer -= Time.deltaTime;
        }

        // Cooldown timer to reset the ability to sprint.
        if (!canSprint)
        {
            sprintTimer -= Time.deltaTime;
            if (sprintTimer < maxSprintTime - sprintCoolDownTime && moveStatus != MoveStatus.crouching)
            {
                canSprint = true;
                sprintTimer = 0;
            }         
        }


        // Move player horizontaly with direction vector.
        // Clamp the magnitude of the direction vector to max 1
        // in order to avoid the player moving faster diagonally.
        player.Move(Vector3.ClampMagnitude(direction, 1f) * moveSpeed * Time.deltaTime);

    }
}
