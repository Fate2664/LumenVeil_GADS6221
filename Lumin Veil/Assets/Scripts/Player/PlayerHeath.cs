using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHeath : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private HealthBarView healthBarView;
    public DeathScreen deathScreen;

    private int maxHealth;
    private Rigidbody2D rb;
    private int currentHealth;
    [HideInInspector]
    public bool isDead = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        switch (SettingsManager.Instance.Difficulty)
        {
            case 0:
                maxHealth = 200; break;
            case 1:
                maxHealth = 100; break;
            case 2:
                maxHealth = 50; break;
            case 3:
                maxHealth = 10; break;  
        }

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
