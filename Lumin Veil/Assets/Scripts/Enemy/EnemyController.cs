using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Health & Damage")]
    [SerializeField] private int maxHeath = 100;
    [SerializeField] private int contactDamage = 10;
    [SerializeField] private float damageCooldown = 1f;

    [Header("Knockback")]
    [SerializeField] private float knockbackX = 5f;
    [SerializeField] private float knockbackY = 2f;

    [Header("Player References")]
    [SerializeField] private PlayerHeath playerHealth;
    [SerializeField] private PlayerController playerController;

    [Header("Particles & Visuals")]
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private Color damageColor = Color.red;

    private int currentHealth;
    private float lastDamageTime = -999f;
    private Coroutine damageCoroutine;

    private void Start()
    {
        currentHealth = maxHeath;
    }

    public void TakeDamage(int damage, Vector2 attackDirection)
    {
        currentHealth -= damage;
        SpawnDamageParticles(attackDirection);

        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);
        damageCoroutine = StartCoroutine(Damage(0.5f));

        if (currentHealth <= 0)
        {
            SpawnDeathParticles();
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    private void SpawnDamageParticles(Vector2 attackDirection)
    {
        if (damageParticles != null)
        {
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector2.right, attackDirection);
            Instantiate(damageParticles, transform.position, spawnRotation);
        }
    }

    private void SpawnDeathParticles()
    {
        if (deathParticles != null)
        {
            ParticleSystem ps = Instantiate(deathParticles, transform.position, Quaternion.identity);
            ps.Play();

            // Ensure particle system has time to finish
            Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
        }
    }

    private IEnumerator Damage(float duration, float flashSpeed = 0.1f)
    {
        SpriteRenderer spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        float elapsed = 0f;
        while (elapsed < duration)
        {
            spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(flashSpeed);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(flashSpeed);
            elapsed += flashSpeed * 2;
        }
        spriteRenderer.color = Color.white;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageCooldown)
            {
                var playerHealth = collision.gameObject.GetComponent<PlayerHeath>();
                if (playerHealth != null)
                {
                    playerHealth.TakeDamage(contactDamage);
                    lastDamageTime = Time.time;

                    if (playerController != null)
                        playerController.ApplyKnockback(transform, knockbackX, knockbackY);
                }
            }
        }
    }
}
