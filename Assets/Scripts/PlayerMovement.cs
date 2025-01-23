using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 10f; // Movement speed
    public float verticalSpeed = 6f; // Vertical movement speed
    public float buoyancy = 10f; // Buoyancy force
    public float waterLevel = 0f; // Y position of the water surface
    public float rotationSpeed = 15f; // Rotation speed
    public float maxLookAngle = 80f; // Maximum angle the player can look up or down

    private Vector3 moveDirection;
    private Vector2 lookDirection;
    private Rigidbody rb;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lookAction;

    private float pitch = 0f; // Up and down rotation
    private float yaw = 0f; // Left and right rotation

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Movement"];  // Read from "3D Vector"
        lookAction = playerInput.actions["Look"];  // Read from "2D Vector"

        // Lock the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        // Read movement input (Vector3)
        moveDirection = moveAction.ReadValue<Vector3>();

        // Check for vertical movement (up and down)
        if (Keyboard.current.spaceKey.isPressed)
        {
            moveDirection.y = 1;
        }
        else if (Keyboard.current.leftShiftKey.isPressed)
        {
            moveDirection.y = -1;
        }
        else
        {
            moveDirection.y = 0;
        }

        // Transform movement direction to be relative to the player's rotation
        Vector3 transformedDirection = transform.TransformDirection(new Vector3(moveDirection.x, 0, moveDirection.z));

        // Apply movement with Rigidbody
        rb.velocity = new Vector3(transformedDirection.x * speed, moveDirection.y * verticalSpeed, transformedDirection.z * speed);

        // Apply buoyancy force if below water level
        if (transform.position.y < waterLevel)
        {
            rb.AddForce(Vector3.up * buoyancy, ForceMode.Acceleration);
        }

        // Read look input (Vector2)
        lookDirection = lookAction.ReadValue<Vector2>();

        // Update yaw and pitch based on look input
        yaw += lookDirection.x * rotationSpeed * Time.fixedDeltaTime;
        pitch -= lookDirection.y * rotationSpeed * Time.fixedDeltaTime;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        // Apply rotation to the player
        rb.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}