using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    [Header("Character movement settings")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jumpSpeed;

    private Transform cameraTransfrom;
    private LayerMask groundLayer;
    private CharacterController player;

    private Vector3 verticalVelocity;
    private Vector3 sphereCheckPos;
    private Vector3 direction;
    private Vector3 inputWASD;

    private float sphereCheckOffset;
    private float sphereCheckRadius;
    private float gravity;

    private bool onGround;

    void Start()
    {
        cameraTransfrom = GameObject.Find("Main Camera").transform;
        groundLayer = LayerMask.GetMask("Ground");
        player = GetComponent<CharacterController>();
        // Radius of the player
        sphereCheckRadius = transform.localScale.x / 2;

        gravity = -9.82f;
    }

    
    void Update()
    {
        CharacterMove();
    }

    private void CharacterMove()
    {
        // Find bottom of the character model.
        sphereCheckOffset = transform.position.y - transform.localScale.y;

        // Set the position needed to check if player is standing on the ground.
        // Set bool if player is/is not standing on the ground layer.
        sphereCheckPos = new Vector3(transform.position.x, sphereCheckOffset, transform.position.z);
        onGround = Physics.CheckSphere(sphereCheckPos, sphereCheckRadius, groundLayer);

        // If player is on ground, stop velocity from building up.
        // Else, keep adding gravity force
        if (onGround && verticalVelocity.y < 0)
        {
            verticalVelocity.y = -2f;
        }
        else
        {
            verticalVelocity.y += 2 * gravity * Mathf.Pow(Time.deltaTime, 2);
        }

        // Get input from WASD.
        inputWASD = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        // Move player FORWARD with new direction gathered from input-WASD
        direction = transform.TransformDirection(inputWASD);
        player.Move(direction * movementSpeed * Time.deltaTime);

        if (onGround && Input.GetButtonDown("Jump"))
        {
            verticalVelocity.y = jumpSpeed * Time.deltaTime;
        }

        // Move player Up/Down with velocity that is only affected by gravity
        player.Move(verticalVelocity);

    }
}
