using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private BoxCollider2D swordHitbox;
    [SerializeField] private LayerMask enemyLayer;
    [Range(0, 100)]
    [SerializeField] private int attackDamage = 20;
    [Range(0, 10)]
    [SerializeField] private float attackRate = 2f; // Attacks per second

    private float nextAttackTime = 0f;
    private int attackTrigger = Animator.StringToHash("isAttacking");
    private static bool isAttacking = false;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nextAttackTime)
        {
            Attack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    private void Attack()
    {
        isAttacking = true;
        animator.SetTrigger(attackTrigger);
        // Activate the hitbox after a small delay (matches swing animation timing)
        Invoke(nameof(ActivateAttackHitbox), 0.1f);
        // Deactivate hitbox and reset attack
        Invoke(nameof(DeactivateAttackHitbox), 0.4f);
    }

    void ActivateAttackHitbox()
    {
        swordHitbox.gameObject.SetActive(true);

        // Do damage check at the moment hitbox is active
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(
            swordHitbox.bounds.center,
            swordHitbox.bounds.size,
            0f,
            enemyLayer
        );

        foreach (Collider2D enemy in hitEnemies)
        {
            Vector2 attackDirection = (enemy.transform.position - transform.position).normalized;
            Enemy enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(attackDamage, attackDirection);
            }
        }
    }

    void DeactivateAttackHitbox()
    {
        swordHitbox.gameObject.SetActive(false);
        isAttacking = false;
    }
}
