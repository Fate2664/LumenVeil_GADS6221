using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [Header("Health & Damage")]
    [SerializeField] private int maxHeath = 100;
    [SerializeField] public int contactDamage = 10;
    [SerializeField] private float damageCooldown = 1f;

    [Header("Knockback")]
    [SerializeField] public float knockbackX = 5f;
    [SerializeField] public float knockbackY = 2f;

    [Header("Player References")]
    [SerializeField] private PlayerHeath playerHealth;
    [SerializeField] private PlayerController playerController;

    [Header("Particles & Visuals")]
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private ParticleSystem damageParticles;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private Color damageColor = Color.red;

    private int currentHealth;
    private float lastDamageTime = -999f;
    private Coroutine damageCoroutine;
    private bool isKnockback = false;
    private Rigidbody2D rb;

    private void Start()
    {
        currentHealth = maxHeath;
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage, Vector2 attackDirection)
    {
        currentHealth -= damage;
        if (SettingsManager.Instance.ParticlesEnabled)
        {
            SpawnDamageParticles(attackDirection);
        }

        if (currentHealth <= 0)
        {
            if (SettingsManager.Instance.ParticlesEnabled)
            {
                SpawnDeathParticles();
            }
            SpawnOrb();
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void ApplyKnockback(Transform source, float horizontalForce, float verticalForce)
    {
        if (playerHealth.isDead) return;

        isKnockback = true;
        Invoke(nameof(ResetKnockback), 0.3f); // Reset knockback after 0.5 seconds

        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
        damageCoroutine = StartCoroutine(Damage(0.5f)); // Flash for 0.5 seconds

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

    private void SpawnDamageParticles(Vector2 attackDirection)
    {
        if (damageParticles != null)
        {
            Quaternion spawnRotation = Quaternion.FromToRotation(Vector2.right, attackDirection);
            Instantiate(damageParticles, transform.position, spawnRotation);
        }
    }

    private void SpawnOrb()
    {
        Vector2 spawnPos = (Vector2)transform.position - new Vector2(0, 1f); // Adjust spawn position as needed
        if (orbPrefab != null)
        {
            GameObject orb = Instantiate(orbPrefab, spawnPos, Quaternion.identity);
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
