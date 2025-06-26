using UnityEngine;

public class EnemyAttackRange : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private PlayerHeath playerHeath;
    [SerializeField] private Enemy enemy;
    [SerializeField] private PlayerController playerController;
    [HideInInspector]
    public bool inAttackRange = false;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inAttackRange = true;
            animator.SetBool("isAttack", true);
            Invoke(nameof(TakeDamage),.3f);
            Invoke(nameof(TakeKnockback),.3f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inAttackRange = false;
            animator.SetBool("isAttack", false);
        }
    }

    private void TakeDamage()
    {
        playerHeath.TakeDamage(enemy.contactDamage);
    }

    private void TakeKnockback()
    {
        playerController.ApplyKnockback(enemy.transform, enemy.knockbackX, enemy.knockbackY);
    }
}
