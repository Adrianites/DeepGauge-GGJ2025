using UnityEngine;
using UnityEngine.InputSystem;

public class GroundPlayerMovement : MonoBehaviour
{
    public float speed = 8f;
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        lookAction = playerInput.actions["Look"];

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        moveDirection = moveAction.ReadValue<Vector3>();

        Vector3 transformedDirection = transform.TransformDirection(new Vector3(moveDirection.x, 0, moveDirection.z));

        rb.velocity = new Vector3(transformedDirection.x * speed, rb.velocity.y, transformedDirection.z * speed);

        lookDirection = lookAction.ReadValue<Vector2>();

        yaw += lookDirection.x * rotationSpeed * Time.fixedDeltaTime;
        pitch -= lookDirection.y * rotationSpeed * Time.fixedDeltaTime;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

        rb.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }
}