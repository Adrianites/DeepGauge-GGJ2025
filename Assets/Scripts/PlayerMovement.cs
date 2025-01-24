using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 8f; 
    public float verticalSpeed = 6f; 
    public float buoyancy = 15f; 
    public float waterLevel = 0f; 
    public float rotationSpeed = 15f; 
    public float maxLookAngle = 90f; 
    public float buoyancyPeriod = 10f; 
    public float buoyancyStrength = 3f; 

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
        moveDirection = moveAction.ReadValue<Vector3>();

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


        Vector3 transformedDirection = transform.TransformDirection(new Vector3(moveDirection.x, 0, moveDirection.z));

        rb.velocity = new Vector3(transformedDirection.x * speed, moveDirection.y * verticalSpeed, transformedDirection.z * speed);

        if (transform.position.y < waterLevel)
        {
            buoyancyTime += Time.fixedDeltaTime;
            float buoyancyForce = Mathf.Sin(buoyancyTime * (2 * Mathf.PI / buoyancyPeriod)) * buoyancy * buoyancyStrength;
            rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);
        }
        else
        {
            buoyancyTime = 0f;
        }

        lookDirection = lookAction.ReadValue<Vector2>();

        yaw += lookDirection.x * rotationSpeed * Time.fixedDeltaTime;
        pitch -= lookDirection.y * rotationSpeed * Time.fixedDeltaTime;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        rb.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}