using UnityEngine;
using UnityEngine.Rendering;

public class PlayerHeath : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private Animator animator;

    private int currentHealth;
    [HideInInspector]
    public bool isDead = false;    

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
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

        // Play death animation
        if (animator != null)
        {
            animator.SetTrigger("isDead");
        }
    }

  
}
