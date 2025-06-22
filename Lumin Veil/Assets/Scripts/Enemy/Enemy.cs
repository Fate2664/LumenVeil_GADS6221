using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int maxHeath = 100;
    

    private int currentHealth;

    private void Start()
    {
        currentHealth = maxHeath;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (this.gameObject != null)
        {
            Destroy(this.gameObject);
        }
    }

}
