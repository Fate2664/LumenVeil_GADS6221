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

    [Header("Player Setup")]
    [Space(10)]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Transform ceilingCheck;
    [SerializeField] private Collider2D crouchDisableCollider;

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
    private float moveDir;
    private bool shouldJump = false;
    private bool shouldCrouch = false;
    private Rigidbody2D rb;

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



    void Update()
    {
        shouldJump = false;
        shouldCrouch = false;
        Debug.Log(grounded);
        moveDir = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            shouldJump = true;
        }
        if (Input.GetKey(KeyCode.LeftControl) && grounded)
        {
            shouldCrouch = true;
        }

        MoveCharacter(moveDir,moveSpeed, shouldCrouch ,shouldJump);
    }

    private void FixedUpdate()
    {
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


    private void MoveCharacter(float direction,float moveSpeed, bool crouch, bool jump)
    {
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

        if (grounded && jump)
        {
            grounded = false;
            rb.AddForce(new Vector2(rb.linearVelocity.x, jumpForce * 100));
            if (animator != null)
            {
                animator.SetBool("isJumping", true);
            }
        }
    }

}
