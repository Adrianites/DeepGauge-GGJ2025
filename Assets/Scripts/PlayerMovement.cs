using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 8f; 
    public float verticalSpeed = 6f; 
    public float buoyancyStrength = 3f; 
    public float buoyancyPeriod = 10f; 
    public float rotationSpeed = 15f; 
    public float maxLookAngle = 90f; 

    private Vector3 moveDirection;
    private Vector2 lookDirection;
    private Rigidbody rb;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction lookAction;

    private float pitch = 0f; 
    private float yaw = 0f; 
    private float buoyancyTime = 0f; 

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Movement"];  
        lookAction = playerInput.actions["Look"];  

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

        // Apply buoyancy force
        buoyancyTime += Time.fixedDeltaTime;
        float buoyancyForce = Mathf.Sin(buoyancyTime * (2 * Mathf.PI / buoyancyPeriod)) * buoyancyStrength;
        rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);

        // Read look input (Vector2)
        lookDirection = lookAction.ReadValue<Vector2>();

        // Update yaw and pitch based on look input
        yaw += lookDirection.x * rotationSpeed * Time.fixedDeltaTime;
        pitch -= lookDirection.y * rotationSpeed * Time.fixedDeltaTime;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        // Apply rotation to the player
        rb.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Items item = other.GetComponent<Items>();
        if (item != null)
        {
            GameManager.Instance.AddItemToInventory(item);
            Destroy(other.gameObject); // Destroy the item GameObject after collection
        }
    }
}