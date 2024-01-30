using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction lookAction;

    [SerializeField] float speed = 5;
    [SerializeField] float jumpForce = 5;
    [SerializeField] float dashSpeed = 10; // New variable for dash speed
    [SerializeField] float dashDuration = 0.5f; // New variable for dash duration
    public float groundDistance = 0.5f;
    Rigidbody rigidBody;

    bool isDashing = false; // New variable to track whether the player is dashing

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        lookAction = playerInput.actions.FindAction("Look");
        rigidBody = GetComponent<Rigidbody>();
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, groundDistance);
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        RotatePlayer();

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rigidBody.velocity = Vector3.up * jumpForce;
        }

        // Check for dash input
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDashing)
        {
            StartCoroutine(Dash());
        }
    }

    void MovePlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();

        // Check if dashing
        float currentSpeed = isDashing ? dashSpeed : speed;

        transform.position += new Vector3(direction.x, 0, direction.y) * currentSpeed * Time.deltaTime;
    }

    void RotatePlayer()
    {
        Vector2 lookDelta = lookAction.ReadValue<Vector2>();
        float mouseX = lookDelta.x;

        // Rotate the player around the Y-axis based on mouse input
        transform.Rotate(Vector3.up, mouseX);
    }

    IEnumerator Dash()
    {
        isDashing = true;

        // Apply dash speed for a certain duration
        float startTime = Time.time;
        while (Time.time < startTime + dashDuration)
        {
            MovePlayer(); // Move with dash speed
            yield return null;
        }

        isDashing = false;
    }
}
