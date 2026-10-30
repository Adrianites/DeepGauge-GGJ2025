using UnityEngine;
using UnityEngine.InputSystem;

public class GroundPlayerMovement : MonoBehaviour
{
    public float speed = 8f;
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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
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

        lookDirection = lookAction.ReadValue<Vector2>();

        yaw += lookDirection.x * mouseSensitivity;
        pitch -= lookDirection.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);
    }

    private void LateUpdate()
    {
        cameraTransform.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void FixedUpdate()
    {
        moveDirection = moveAction.ReadValue<Vector3>();

        Quaternion yawRotation = Quaternion.Euler(0f, yaw, 0f);
        rb.MoveRotation(yawRotation);

        Vector3 transformedDirection = yawRotation * new Vector3(moveDirection.x, 0, moveDirection.z);

        rb.linearVelocity = new Vector3(transformedDirection.x * speed, rb.linearVelocity.y, transformedDirection.z * speed);
    }
}
