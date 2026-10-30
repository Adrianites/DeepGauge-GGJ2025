using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 8f;
    public float verticalSpeed = 6f;
    public float buoyancyStrength = 3f;
    public float buoyancyPeriod = 10f;
    public float mouseSensitivity = 0.15f;
    public float maxLookAngle = 90f;
    public Transform cameraTransform;

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

        if (cameraTransform == null)
        {
            cameraTransform = GetComponentInChildren<Camera>().transform;
        }

        rb.freezeRotation = true;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        yaw = transform.eulerAngles.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }

        // Read look input (Vector2)
        lookDirection = lookAction.ReadValue<Vector2>();

        // Update yaw and pitch based on look input
        yaw += lookDirection.x * mouseSensitivity;
        pitch -= lookDirection.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
    }

    private void LateUpdate()
    {
        // Apply rotation to the player
        cameraTransform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void FixedUpdate()
    {
        // Read movement input (Vector3)
        moveDirection = moveAction.ReadValue<Vector3>();

        Quaternion yawRotation = Quaternion.Euler(0f, yaw, 0f);
        rb.MoveRotation(yawRotation);

        // Transform movement direction to be relative to the player's rotation
        Vector3 transformedDirection = yawRotation * new Vector3(moveDirection.x, 0, moveDirection.z);

        // Apply movement with Rigidbody
        rb.linearVelocity = new Vector3(transformedDirection.x * speed, moveDirection.y * verticalSpeed, transformedDirection.z * speed);

        // Apply buoyancy force
        buoyancyTime += Time.fixedDeltaTime;
        float buoyancyForce = Mathf.Sin(buoyancyTime * (2 * Mathf.PI / buoyancyPeriod)) * buoyancyStrength;
        rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);
    }
}
