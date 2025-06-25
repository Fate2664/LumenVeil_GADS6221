using UnityEngine;

public class EnemyLookRange : MonoBehaviour
{
    [HideInInspector]
    public bool inLookRange = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inLookRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inLookRange = false;
        }
    }
}
