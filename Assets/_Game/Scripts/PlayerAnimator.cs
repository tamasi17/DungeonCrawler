using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerAnimator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public float walkSpeed = 5f;
    [SerializeField] public float sprintSpeed = 8f;

    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Vector2 lastMoveDirection = Vector2.down; // Default facing direction

    private PlayerStamina staminaSystem;

    private bool isLocked = false;

    private bool isSprinting;
    private bool isDead = false;
    private float stopTimer;
    [SerializeField] private float stopDelay = 0.15f; // Small buffer time

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        staminaSystem = GetComponent<PlayerStamina>();

        // Start the global timer when the player spawns
        if (GameSession.Instance != null)
        {
            GameSession.Instance.StartLevel();
        }

    }

    void Update()
    {
        if (isDead) return;
        if (Time.timeScale == 0f) return;

        if (isLocked)
        {
            // 1. Kill the input variable so FixedUpdate doesn't use old data
            moveInput = Vector2.zero;

            // 2. Kill the animation so we don't look like we are walking in place
            UpdateAnimator(false);

            return;
        }

        // 1. Read Input
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        bool wantsToSprint = Input.GetKey(KeyCode.LeftShift);

        // We only sprint if:
        // 1. Shift is held
        // 2. We are actually moving (optional, but saves stamina while standing still)
        // 3. The Stamina System exists AND says "OK"
        if (wantsToSprint && moveInput.magnitude > 0 && staminaSystem != null && staminaSystem.CanSprint)
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        moveInput = new Vector2(x, y).normalized;

        // 2. Logic: Remember the last direction
        // If we stop moving, we want to keep facing the last direction (not snap to 0,0)
        if (moveInput.magnitude > 0)
        {
            lastMoveDirection = moveInput;
            stopTimer = stopDelay; // Refill the buffer
        }
        else
        {
            // If no input, drain the timer
            stopTimer -= Time.deltaTime;
        }

        bool isMoving = stopTimer > 0;

        // 3. Update Animator Parameters
        UpdateAnimator(isMoving);
    }

    void FixedUpdate()
    {
        if (isDead) return;

        if (isLocked)
        {
            // Force physics to stop immediately
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // 4. Move the Physics Body
        float currentSpeed = isSprinting ? sprintSpeed : walkSpeed;
        rb.linearVelocity = moveInput * currentSpeed;
    }

    private void UpdateAnimator(bool IsMoving)
    {
        // "InputX" and "InputY" drive the Blend Tree
        // We always feed it the LAST known direction so the Idle frame is correct
        animator.SetFloat("InputX", lastMoveDirection.x);
        animator.SetFloat("InputY", lastMoveDirection.y);

        // "IsMoving" handles the transition from Idle -> Walk
        animator.SetBool("IsMoving", IsMoving);

        // "IsSprinting" handles the transition from Walk -> Sprint
        animator.SetBool("IsSprinting", isSprinting);
    }

    public void UpgradeSpeed(float amount)
    {
        sprintSpeed += amount;

        walkSpeed += amount * 0.5f; 

        Debug.Log("SPEED UPGRADED! New Sprint Speed: " + sprintSpeed);
    }

    public void LockMovement(float duration)
    {
        StartCoroutine(LockRoutine(duration));
    }

    private IEnumerator LockRoutine(float duration)
    {
        isLocked = true;

        // Optional: Play an "Item Get" animation here if you have one
        // GetComponent<Animator>().SetTrigger("Pickup");

        yield return new WaitForSeconds(duration);

        isLocked = false;
    }

    // Call this method from your Health/Damage script
    public void TriggerDeath()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero; // Stop moving instantly
        animator.SetTrigger("Die");
    }
}