using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHeath : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Animator animator;
    [SerializeField] private HealthBarView healthBarView;
    public DeathScreen deathScreen;

    private Rigidbody2D rb;
    private int currentHealth;
    [HideInInspector]
    public bool isDead = false;    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        healthBarView.SetHealth(currentHealth, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBarView.SetHealth(currentHealth, maxHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        // Play death animation
        if (animator != null)
        {
            animator.SetTrigger("isDead");
        }
        deathScreen.ShowDeathScreen();
    }

  
}
