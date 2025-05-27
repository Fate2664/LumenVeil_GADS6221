using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    [Space(10)]
    [Range(0, 10)]
    [SerializeField] private float moveSpeed;
    [Range(5, 15)]
    [SerializeField] private float jumpForce;

    [Header("Animator Component")]
    [Space(10)]
    [SerializeField] private Animator animator;



    private float moveDir;
    private bool canJump = false;
    private Rigidbody2D rb;



    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveDir = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && canJump)
        {
            rb.AddForce(new Vector2(rb.linearVelocity.x, jumpForce * 100));
            if (animator != null)
            {
                animator.SetBool("isJumping", true);
            }
            canJump = false;
        }
        MoveCharacter(moveDir);
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
    }


    private void MoveCharacter(float direction)
    {
        rb.linearVelocity = new Vector2(direction * moveSpeed, rb.linearVelocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (animator != null)
            {
                animator.SetBool("isJumping", false);
            }
            canJump = true;
        }
    }
}
