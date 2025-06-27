using System.Collections;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [Space(10)]
    [Range(0, 10)]
    [SerializeField] private float moveSpeed;
    [Range(0, 1)]
    [SerializeField] private float crouchSpeedMultiplier;
    [Range(5, 15)]
    [SerializeField] private float jumpForce;
    [Range(2, 5)]
    [SerializeField] private float rangeAttackRate = 2f;

    [Header("Player Setup")]
    [Space(10)]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Collider2D crouchDisableCollider;
    [SerializeField] private Transform playerGraphics;
    [SerializeField] private Vector3 lockedScale = new Vector3(3f, 3f, 3f);
    [SerializeField] private Color knockbackColor = Color.red;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform fireballSpawnPoint;

    [Header("Connections")]
    [SerializeField] private InventoryPanel inventoryPanel;
    [SerializeField] private GameObject inventoryPanelPrefab;

    [Header("Animator Component")]
    [Space(10)]
    [SerializeField] private Animator animator;


    [Header("Events")]
    [Space]
    public UnityEvent OnLandEvent;

    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }

    public BoolEvent OnCrouchEvent;
    private bool wasCrouching = false;

    private const float groundedRadius = .2f;
    private const float ceilingRadius = .2f;
    private bool grounded;
    private PlayerHeath playerHealth;
    private float moveDir;
    private bool shouldJump = false;
    private bool shouldCrouch = false;
    private bool isKnockback = false;
    private Coroutine flashRoutine;
    private Rigidbody2D rb;
    private float nextRangeAttackTime = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (OnLandEvent == null)
        {
            OnLandEvent = new UnityEvent();
        }
        if (OnCrouchEvent == null)
        {
            OnCrouchEvent = new BoolEvent();
        }
    }

    private void Start()
    {
        playerHealth = GetComponent<PlayerHeath>();
    }

    void Update()
    {
        if (playerHealth.isDead)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.E) && inventoryPanel.HasRangeAttack() && Time.time >= nextRangeAttackTime)
        {
            Invoke(nameof(SpawnFireBall), .2f);
            animator.SetTrigger("isRangeAttack");
            AudioManager.Instance?.PlaySFX("FireBall");
            nextRangeAttackTime = Time.time + 1f / rangeAttackRate;
        }

        playerGraphics.localScale = lockedScale;
        shouldJump = false;
        shouldCrouch = false;

        moveDir = 0f;
        if (PlayerControlSettings.CurrentPreset == "WASD")
        {
            if (Input.GetKey(KeyCode.A))
            {
                moveDir = -1f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveDir = 1f;
            }
        }
        else
        if (PlayerControlSettings.CurrentPreset == "ArrowKeys")
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                moveDir = -1f;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                moveDir = 1f;
            }
        }


        if (PlayerControlSettings.CurrentPreset == "WASD")
        {
            if (Input.GetKeyDown(KeyCode.Space) && grounded)
                shouldJump = true;
            if (Input.GetKey(KeyCode.LeftControl) && grounded)
                shouldCrouch = true;
        }
        else if (PlayerControlSettings.CurrentPreset == "ArrowKeys")
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && grounded)
                shouldJump = true;
            if (Input.GetKey(KeyCode.DownArrow) && grounded)
                shouldCrouch = true;
        }

        bool isInAir = !grounded && Mathf.Abs(rb.linearVelocity.y) > 0.1f;
        animator.SetBool("isJumping", isInAir);

        MoveCharacter(moveDir, moveSpeed, shouldCrouch, shouldJump);
    }

    private void FixedUpdate()
    {
        if (playerHealth.isDead)
        {
            return;
        }

        if (Input.GetAxis("Horizontal") < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }

        bool wasGrounded = grounded;
        grounded = false;


        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;
                if (!wasGrounded)
                {
                    OnLandEvent.Invoke();
                }
            }
        }
    }

    private void SpawnFireBall()
    {
        Vector2 fireDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
        fireball.GetComponent<FireBall>().SetDirection(fireDirection);
    }
    private void ToggleInventory()
    {
        if (inventoryPanelPrefab != null)
        {
            bool isActive = inventoryPanelPrefab.activeSelf;
            inventoryPanelPrefab.SetActive(!isActive);

            if (!isActive)
            {
                rb.linearVelocity = Vector2.zero; // Stop player movement when inventory is open
                inventoryPanel.CharacterGrid.Refresh();
                inventoryPanel.PowerUpGrid.Refresh();
            }
        }
    }

    private void MoveCharacter(float direction, float moveSpeed, bool crouch, bool jump)
    {
        if (isKnockback)
        {
            return;
        }
        if (!crouch)
        {
            if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
            {
                crouch = true;
            }
        }

        if (grounded)
        {
            if (crouch)
            {
                if (!wasCrouching)
                {
                    wasCrouching = true;
                    OnCrouchEvent.Invoke(true);
                }


                if (crouchDisableCollider != null)
                {
                    crouchDisableCollider.enabled = false;
                }
            }
            else
            {
                if (crouchDisableCollider != null)
                {
                    crouchDisableCollider.enabled = true;
                }
                if (wasCrouching)
                {
                    wasCrouching = false;
                    OnCrouchEvent.Invoke(false);
                }
            }
        }
        float modifiedSpeed = crouch ? moveSpeed * crouchSpeedMultiplier : moveSpeed;
        rb.linearVelocity = new Vector2(direction * modifiedSpeed, rb.linearVelocity.y);
        animator.SetFloat("Speed", Mathf.Abs(rb.linearVelocity.x));

        if (grounded && jump)
        {
            grounded = false;
            rb.AddForce(new Vector2(rb.linearVelocity.x, jumpForce * 100));
        }
    }

    public void ApplyKnockback(Transform source, float horizontalForce, float verticalForce)
    {
        if (playerHealth.isDead) return;

        isKnockback = true;
        Invoke(nameof(ResetKnockback), 0.3f); // Reset knockback after 0.5 seconds

        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
        }
        flashRoutine = StartCoroutine(FlashDuringKnockback(0.5f)); // Flash for 0.5 seconds

        // Determine direction: +1 = right, -1 = left
        float direction = transform.position.x > source.position.x ? 1f : -1f;

        // Clear current velocity
        rb.linearVelocity = Vector2.zero;

        // Apply force separately on X and Y
        Vector2 knockback = new Vector2(direction * horizontalForce, verticalForce);
        rb.AddForce(knockback, ForceMode2D.Impulse);

        // Debug.DrawRay(transform.position, knockback, Color.red, 1f);
    }

    private void ResetKnockback()
    {
        isKnockback = false;
    }

    private IEnumerator FlashDuringKnockback(float duration, float flashSpeed = 0.1f)
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        float elapsed = 0f;
        while (elapsed < duration)
        {
            spriteRenderer.enabled = false; // Hide sprite
            yield return new WaitForSeconds(flashSpeed);
            spriteRenderer.enabled = true; // Show sprite
            spriteRenderer.color = knockbackColor;
            yield return new WaitForSeconds(flashSpeed);
            elapsed += flashSpeed * 2; // Increment elapsed time
        }
        spriteRenderer.color = Color.white; // Reset color to white
        spriteRenderer.enabled = true; // Ensure sprite is visible after flashing
    }
}
