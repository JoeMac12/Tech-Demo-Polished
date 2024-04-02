using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Movement : MonoBehaviour
{
    [SerializeField] Transform playerCamera;
    [SerializeField][Range(0.0f, 0.5f)] float mouseSmoothTime = 0.03f;
    [SerializeField] bool cursorLock = true;
    [SerializeField] float mouseSensitivity = 3.5f;
    [SerializeField] float Speed = 6.0f;
    [SerializeField][Range(0.0f, 0.5f)] float moveSmoothTime = 0.3f;
    [SerializeField] float gravity = -30f;
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask ground;

    [SerializeField] TextMeshProUGUI staminaText;

    [SerializeField] float normalFOV = 75f;
    [SerializeField] float sprintFOV = 90f;
    [SerializeField] float fovTransitionSpeed = 5f;

    [SerializeField] float baseStaminaRegenRate = 1.5f;
    [SerializeField] float stoppedStaminaRegenRate = 3.0f;
    [SerializeField] float staminaRegenDelay = 1.0f;

    private float staminaRegenTimer = 0f;

    [SerializeField] float sprintSpeed = 10f;
    [SerializeField] float staminaDrainRate = 0.2f;
    [SerializeField] float maxStamina = 100f;
    float currentStamina;

    public float jumpHeight = 6f;
    float velocityY;
    bool isGrounded;

    float cameraCap;
    Vector2 currentMouseDelta;
    Vector2 currentMouseDeltaVelocity;

    CharacterController controller;
    Vector2 currentDir;
    Vector2 currentDirVelocity;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;

        if (cursorLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = true;
        }
    }

    void Update()
    {
        UpdateMouse();
        UpdateMove();
        UpdateStamina();
    }

    void UpdateMouse()
    {
        Vector2 targetMouseDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        currentMouseDelta = Vector2.SmoothDamp(currentMouseDelta, targetMouseDelta, ref currentMouseDeltaVelocity, mouseSmoothTime);

        cameraCap -= currentMouseDelta.y * mouseSensitivity;

        cameraCap = Mathf.Clamp(cameraCap, -90.0f, 90.0f);

        playerCamera.localEulerAngles = Vector3.right * cameraCap;

        transform.Rotate(Vector3.up * currentMouseDelta.x * mouseSensitivity);
    }

    void UpdateMove()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, ground);

        Vector2 targetDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        targetDir.Normalize();

        currentDir = Vector2.SmoothDamp(currentDir, targetDir, ref currentDirVelocity, moveSmoothTime);

        velocityY += gravity * 2f * Time.deltaTime;

        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0;

        float targetSpeed = isSprinting ? sprintSpeed : Speed;
        Vector3 targetVelocity = (transform.forward * currentDir.y + transform.right * currentDir.x) * targetSpeed + Vector3.up * velocityY;
        controller.Move(targetVelocity * Time.deltaTime);

        float targetFOV = isSprinting ? sprintFOV : normalFOV;
        playerCamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(playerCamera.GetComponent<Camera>().fieldOfView, targetFOV, fovTransitionSpeed * Time.deltaTime);

        if (isSprinting)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocityY = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if(isGrounded! && controller.velocity.y < -1f)
        {
            velocityY = -8f;
        }
    }

    void UpdateStamina() {
        bool isSprinting = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0;
        bool isMoving = Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0 || Mathf.Abs(Input.GetAxisRaw("Vertical")) > 0;

        if (isSprinting) {
            staminaRegenTimer = 0f;
        } else {
            staminaRegenTimer += Time.deltaTime;
        }

        if (staminaRegenTimer >= staminaRegenDelay) {
            float regenRate = isMoving ? baseStaminaRegenRate : stoppedStaminaRegenRate;
            currentStamina += regenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }

        if (staminaText != null) {
            staminaText.text = "Stamina: " + Mathf.RoundToInt(currentStamina).ToString();
        }
    }
}
