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

    [Header("Footstep")]
    public float walkStepInterval = 0.5f;
    public float sprintStepInterval = 0.32f;

    private float footstepTimer;

    [Header("Breathing")]
    public float breathingStartDelay = 0.5f;

    private float breathingTimer;
    private bool breathing;

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
        HandleFootsteps();
        HandleBreathing();
    }

    // =====================================================
    // FOOTSTEP
    // =====================================================

    void HandleFootsteps()
    {
        if (Keyboard.current == null)
            return;

        bool moving =
            (Keyboard.current.wKey.isPressed ||
             Keyboard.current.aKey.isPressed ||
             Keyboard.current.sKey.isPressed ||
             Keyboard.current.dKey.isPressed);

        bool grounded = controller.isGrounded;

        if (!moving || !grounded)
        {
            footstepTimer = 0f;
            return;
        }

        bool sprinting =
            Keyboard.current.leftShiftKey.isPressed;

        float stepInterval =
            sprinting
            ? sprintStepInterval
            : walkStepInterval;

        footstepTimer -= Time.deltaTime;

        if (footstepTimer <= 0f)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayFootstep();
            }

            footstepTimer = stepInterval;
        }
    }

    // =====================================================
    // BREATHING
    // =====================================================

    void HandleBreathing()
    {
        if (Keyboard.current == null)
            return;

        bool moving =
            (Keyboard.current.wKey.isPressed ||
             Keyboard.current.aKey.isPressed ||
             Keyboard.current.sKey.isPressed ||
             Keyboard.current.dKey.isPressed);

        bool sprinting =
            Keyboard.current.leftShiftKey.isPressed;

        bool shouldBreathe =
            moving &&
            sprinting &&
            controller.isGrounded;

        if (shouldBreathe)
        {
            if (!breathing)
            {
                breathingTimer += Time.deltaTime;

                if (breathingTimer >= breathingStartDelay)
                {
                    StartBreathing();
                }
            }
        }
        else
        {
            breathingTimer = 0f;

            if (breathing)
            {
                StopBreathing();
            }
        }
    }

    void StartBreathing()
    {
        breathing = true;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBreathing();
        }
    }

    void StopBreathing()
    {
        breathing = false;

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopBreathing();
        }
    }

    // =====================================================
    // HEAD BOB
    // =====================================================

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
            bool sprinting =
                Keyboard.current.leftShiftKey.isPressed;

            float speed =
                sprinting
                ? sprintBobSpeed
                : walkBobSpeed;

            float amount =
                sprinting
                ? sprintBobAmount
                : walkBobAmount;

            bobTimer += Time.deltaTime * speed;

            Vector3 pos = cameraHolder.localPosition;

            pos.y =
                defaultYPos +
                Mathf.Sin(bobTimer) * amount;

            cameraHolder.localPosition = pos;
        }
        else
        {
            bobTimer = 0;

            Vector3 pos =
                cameraHolder.localPosition;

            pos.y = Mathf.Lerp(
                pos.y,
                defaultYPos,
                Time.deltaTime * 10f
            );

            cameraHolder.localPosition = pos;
        }
    }

    // =====================================================
    // MOVE
    // =====================================================

    void Move()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)
                input.y += 1;

            if (Keyboard.current.sKey.isPressed)
                input.y -= 1;

            if (Keyboard.current.aKey.isPressed)
                input.x -= 1;

            if (Keyboard.current.dKey.isPressed)
                input.x += 1;
        }

        Vector3 move =
            (transform.forward * input.y +
             transform.right * input.x).normalized;

        float speed =
            Keyboard.current.leftShiftKey.isPressed
            ? sprintSpeed
            : walkSpeed;

        controller.Move(
            move * speed * Time.deltaTime
        );

        if (controller.isGrounded)
        {
            if (velocity.y < 0)
                velocity.y = -2f;

            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                velocity.y =
                    Mathf.Sqrt(
                        jumpHeight * -2f * gravity
                    );
            }
        }

        velocity.y +=
            gravity * Time.deltaTime;

        controller.Move(
            velocity * Time.deltaTime
        );
    }

    // =====================================================
    // LOOK
    // =====================================================

    void Look()
    {
        if (Mouse.current == null)
            return;

        Vector2 mouseDelta =
            Mouse.current.delta.ReadValue();

        float mouseX =
            mouseDelta.x * mouseSensitivity;

        float mouseY =
            mouseDelta.y * mouseSensitivity;

        xRotation -= mouseY;

        xRotation =
            Mathf.Clamp(
                xRotation,
                -maxLookAngle,
                maxLookAngle
            );

        cameraHolder.localRotation =
            Quaternion.Euler(
                xRotation,
                0,
                0
            );

        transform.Rotate(
            Vector3.up * mouseX
        );
    }

    // =====================================================
    // CLEANUP
    // =====================================================

    void OnDisable()
    {
        StopBreathing();
    }
}