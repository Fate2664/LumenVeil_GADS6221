using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private int attackDamage = 50;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeSpan = 5f;
   
    private Vector2 direction;

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    private void Start()
    {
        Destroy(gameObject, lifeSpan);
    }
    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Vector2 attackDirection = (collision.transform.position - transform.position).normalized;
            Enemy enemyComponent = collision.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.TakeDamage(attackDamage, attackDirection);
                enemyComponent.ApplyKnockback(transform, 5, 6);
            }
        }
        else
        if (collision.CompareTag("TallGrass"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

    }
}
