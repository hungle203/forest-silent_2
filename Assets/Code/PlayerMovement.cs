using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float jumpHeight = 1.5f;
    public float gravity = -20f;

    [Header("Look")]
    public Transform cameraHolder;
    public float mouseSensitivity = 0.15f;
    public float maxLookAngle = 80f;

    [Header("Head Bob")]
public float walkBobSpeed = 14f;
public float walkBobAmount = 0.05f;

public float sprintBobSpeed = 18f;
public float sprintBobAmount = 0.08f;

private float defaultYPos;
private float bobTimer;

    private CharacterController controller;
    private Vector3 velocity;
    private float xRotation;

    void Start()
    {
        defaultYPos = cameraHolder.localPosition.y;
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Move();
        Look();
        HeadBob();
    }

   void HeadBob()
{
    bool isMoving =
        (Keyboard.current.wKey.isPressed ||
         Keyboard.current.aKey.isPressed ||
         Keyboard.current.sKey.isPressed ||
         Keyboard.current.dKey.isPressed)
        && controller.isGrounded;

    if (isMoving)
    {
        bool sprinting = Keyboard.current.leftShiftKey.isPressed;

        float speed = sprinting ? sprintBobSpeed : walkBobSpeed;
        float amount = sprinting ? sprintBobAmount : walkBobAmount;

        bobTimer += Time.deltaTime * speed;

        Vector3 pos = cameraHolder.localPosition;
        pos.y = defaultYPos + Mathf.Sin(bobTimer) * amount;

        cameraHolder.localPosition = pos;
    }
    else
    {
        bobTimer = 0;

        Vector3 pos = cameraHolder.localPosition;
        pos.y = Mathf.Lerp(pos.y, defaultYPos, Time.deltaTime * 10f);

        cameraHolder.localPosition = pos;
    }
}
    void Move()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed) input.y += 1;
            if (Keyboard.current.sKey.isPressed) input.y -= 1;
            if (Keyboard.current.aKey.isPressed) input.x -= 1;
            if (Keyboard.current.dKey.isPressed) input.x += 1;
        }

        Vector3 move = (transform.forward * input.y + transform.right * input.x).normalized;

        float speed = Keyboard.current.leftShiftKey.isPressed
            ? sprintSpeed
            : walkSpeed;

        controller.Move(move * speed * Time.deltaTime);

        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -2f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Look()
    {
        if (Mouse.current == null)
            return;

        Vector2 mouseDelta = Mouse.current.delta.ReadValue();

        float mouseX = mouseDelta.x * mouseSensitivity;
        float mouseY = mouseDelta.y * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -maxLookAngle, maxLookAngle);

        cameraHolder.localRotation = Quaternion.Euler(xRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);
    }
}