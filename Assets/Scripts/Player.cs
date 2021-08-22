using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class Player : Damagable
{
    [SerializeField] Camera playerCam;
    [SerializeField] Transform playerVisuals;
    [SerializeField] Transform playerForward;
    [SerializeField] Light flashlight;

    [SerializeField] float baseMovementSpeed = 3f;
    [SerializeField] float sprintingMovementSpeed = 5f;
    [SerializeField] float crouchingMovementSpeed = 1f;
    [SerializeField] float lookSpeed = 1f;
    [SerializeField] float crouchHeight = 0.5f;
    [SerializeField] float crouchCheckScale = 0.99f;

    [SerializeField] float jumpForce = 10f;
    [SerializeField] float groundCheckRadius = 0.025f;
    [SerializeField] float groundCheckOffset = 0.005f;
    [SerializeField] float groundCheckScale = 0.9f;

    [SerializeField] LayerMask jumpIgnore;

    Rigidbody rb;
    CapsuleCollider capsuleCollider;

    Inputs input;
    bool controlsReady = false;

    Vector2 movement;
    float movementSpeed;

    bool shouldJump = false;
    bool isSprinting = false;
    bool isCrouching = false;
    bool shouldUncrouch = false;

    float baseHeight;

    float cameraX = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        rb = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        baseHeight = transform.localScale.y;

        input = new Inputs();

        input.Player.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += ctx => movement = Vector2.zero;

        input.Player.Sprint.performed += ctx => isSprinting = true;
        input.Player.Sprint.canceled += ctx => isSprinting = false;

        input.Player.Crouch.performed += ctx => Crouch(true);
        input.Player.Crouch.canceled += ctx => Crouch(false);

        input.Player.Jump.performed += ctx => shouldJump = true;

        input.Player.Flashlight.performed += ctx => ToggleFlashlight();

        controlsReady = true;
        SetControlsActive(true);
    }

    private void FixedUpdate()
    {
        Move();
        if (shouldJump) { Jump(); }
    }

    private void Update()
    {
        Look();

        if (shouldUncrouch) { UncrouchCheck(); }
    }

    void UncrouchCheck()
    {
        Vector3 basePos = new Vector3(transform.position.x, transform.position.y + (baseHeight - crouchHeight), transform.position.z);

        Vector3 startPos = new Vector3(transform.position.x, basePos.y - (baseHeight / 2), transform.position.z);
        Vector3 endPos = new Vector3(transform.position.x, basePos.y + (baseHeight / 2), transform.position.z);

        if (Physics.CheckCapsule(startPos, endPos, capsuleCollider.radius * crouchCheckScale, ~jumpIgnore, QueryTriggerInteraction.Ignore)) { return; }

        transform.localScale = new Vector3(transform.localScale.x, baseHeight, transform.localScale.z);
        transform.position += new Vector3(0, baseHeight - crouchHeight, 0);

        isCrouching = false;
        shouldUncrouch = false;
    }

    void Crouch(bool crouching)
    {
        if (crouching) {
            transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
            transform.position -= new Vector3(0, baseHeight - crouchHeight, 0);

            isCrouching = true;

            Damage(150);
        } else
        {
            shouldUncrouch = true;
        }
    }

    private void LateUpdate()
    {
        UpdatePlayerForward();
    }

    void Move()
    {
        movementSpeed = GetMovementSpeed();

        Vector3 movementVector = new Vector3(0, 0, 0);

        movementVector += playerForward.forward * movement.y;
        movementVector += playerForward.right * movement.x;

        movementVector *= movementSpeed;

        rb.velocity = new Vector3(movementVector.x, rb.velocity.y, movementVector.z);
    }

    void Look()
    {
        float mouseDeltaX = InputManager.mouseDelta.x * lookSpeed;
        float mouseDeltaY = InputManager.mouseDelta.y * lookSpeed;

        cameraX = Mathf.Clamp(cameraX - mouseDeltaY, -90f, 90f);

        playerCam.transform.localEulerAngles = new Vector3(cameraX, playerCam.transform.localEulerAngles.y + mouseDeltaX, 0);
        playerVisuals.localEulerAngles += new Vector3(0, mouseDeltaX, 0);
    }

    void Jump()
    {
        shouldJump = false;

        Vector3 sphereCenter = new Vector3(
            transform.position.x,
            transform.position.y - ((capsuleCollider.height * transform.localScale.y) / 2) - (groundCheckRadius / 2) + groundCheckOffset,
            transform.position.z
        );

        if (Physics.CheckSphere(sphereCenter, capsuleCollider.radius * groundCheckScale, ~jumpIgnore, QueryTriggerInteraction.Ignore))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    protected override void Kill()
    {
        Debug.Log("uh oh i died f");

        //base.Kill();
    }

    private void OnDrawGizmosSelected()
    {
        if (capsuleCollider == null) { capsuleCollider = GetComponent<CapsuleCollider>(); }

        Vector3 boxCenter = new Vector3(
            transform.position.x,
            transform.position.y - ((capsuleCollider.height * transform.localScale.y) / 2) - (groundCheckRadius / 2) + groundCheckOffset,
            transform.position.z
        );

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(boxCenter, capsuleCollider.radius * groundCheckScale);

        Vector3 basePos = new Vector3(transform.position.x, transform.position.y + (baseHeight - crouchHeight), transform.position.z);

        Vector3 startPos = new Vector3(transform.position.x, basePos.y - (baseHeight / 2), transform.position.z);
        Vector3 endPos = new Vector3(transform.position.x, basePos.y + (baseHeight / 2), transform.position.z);

        if (isCrouching)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(basePos, 0.1f);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(startPos, capsuleCollider.radius * crouchCheckScale);
            Gizmos.DrawWireSphere(endPos, capsuleCollider.radius * crouchCheckScale);
        }

    }

    void SetControlsActive(bool active)
    {
        if (!controlsReady) { return; }
        if (active) { input.Enable(); } else { input.Disable(); }
    }

    void UpdatePlayerForward()
    {
        playerForward.localEulerAngles = new Vector3(0, playerCam.transform.localEulerAngles.y, 0);
    }

    void ToggleFlashlight()
    {
        flashlight.gameObject.SetActive(!flashlight.gameObject.activeSelf);
    }

    float GetMovementSpeed()
    {
        if (isCrouching) return crouchingMovementSpeed;
        if (isSprinting) return sprintingMovementSpeed;

        return baseMovementSpeed;
    }

    private void OnEnable()
    {
        SetControlsActive(true);
    }

    private void OnDisable()
    {
        SetControlsActive(false);
    }
}
