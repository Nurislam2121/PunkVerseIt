using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float rotationSpeed = 10f;
    public Transform cameraTransform;

    [Header("Jump")]
    public float jumpForce = 5f;
    public float jumpCooldown = 0.4f;
    public float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;

    [Header("Stamina")]
    public float maxStamina = 5f;
    public float staminaRecoveryRate = 1f;
    public float staminaDrainRate = 1.5f;

    [Header("Spawn")]
    public GameObject spawnPrefab;
    public Vector3 spawnOffset = new Vector3(0, 1f, 0);

    private bool isGrounded;
    private bool canJump = true;
    private float currentStamina;
    private bool isRunning;
    private Vector3 spawnPoint;

    private Rigidbody rb;
    private PlatformTrigger currentTrigger;
    private Animator animator;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        currentStamina = maxStamina;
        spawnPoint = transform.position;

        animator = GetComponent<Animator>(); // получаем Animator
    }

    private void Update()
    {
        CheckGround();

        if (Input.GetKeyDown(KeyCode.Space))
            Jump();

        if (Input.GetKeyDown(KeyCode.Q))
            SpawnObject();

        if (currentTrigger != null && Input.GetKeyDown(KeyCode.E))
            currentTrigger.ActivatePlatform();
    }

    private void FixedUpdate()
    {
        Move();
        HandleStamina();
        ApplyBetterFall();
    }

    void SpawnObject()
    {
        if (spawnPrefab == null) return;
        Instantiate(spawnPrefab, transform.position + spawnOffset, Quaternion.identity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlatformTrigger trigger))
            currentTrigger = trigger;

        if (other.CompareTag("CheckPoint"))
            spawnPoint = other.transform.position;

        if (other.CompareTag("Kill"))
            Respawn();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlatformTrigger trigger) && currentTrigger == trigger)
            currentTrigger = null;
    }

    void Respawn()
    {
        rb.velocity = Vector3.zero;
        transform.position = spawnPoint;
    }

    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 inputDirection = new Vector3(horizontal, 0f, vertical).normalized;

        isRunning = Input.GetKey(KeyCode.LeftShift) && currentStamina > 0f;
        float moveSpeed = isRunning ? runSpeed : walkSpeed;

        if (inputDirection.magnitude >= 0.1f)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;

            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            Vector3 moveDirection = camForward * vertical + camRight * horizontal;

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                rotationSpeed * Time.fixedDeltaTime
            );

            rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);

            // 🔥 Обновляем параметры Animator
            animator.SetFloat("Speed", moveSpeed);
            animator.SetBool("IsRunning", isRunning);
        }
        else
        {
            animator.SetFloat("Speed", 0f);
            isRunning = false;
            animator.SetBool("IsRunning", false);
        }
    }

    void Jump()
    {
        if (!isGrounded || !canJump) return;

        canJump = false;
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        Invoke(nameof(ResetJump), jumpCooldown);
    }

    void ResetJump() => canJump = true;

    void ApplyBetterFall()
    {
        if (!isGrounded)
            rb.AddForce(Vector3.down * 10f);
    }

    void CheckGround()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down,
                                    groundCheckDistance, groundLayer);
    }

    void HandleStamina()
    {
        if (isRunning)
            currentStamina -= staminaDrainRate * Time.fixedDeltaTime;
        else
            currentStamina += staminaRecoveryRate * Time.fixedDeltaTime;

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }

    public float GetStamina() => currentStamina / maxStamina;
}
