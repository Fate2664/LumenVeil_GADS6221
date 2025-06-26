using System.Runtime.CompilerServices;
using UnityEngine;

public class TallGrass : MonoBehaviour
{
    [SerializeField] private GameObject tallGrassPrefab;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("RangeAttack"))
        {
            Destroy(collision.gameObject);
            Destroy(tallGrassPrefab);
        }
    }
}
