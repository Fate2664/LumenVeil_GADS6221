using UnityEngine;

public class RangeAttackPickup : MonoBehaviour
{
    public ItemDescription rangeAttackPowerUp;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryPanel inventoryPanel = FindObjectOfType<InventoryPanel>();
            if (inventoryPanel != null)
            {
                inventoryPanel.AddItemToPowerUpsInventory(rangeAttackPowerUp, 1);
                AudioManager.Instance?.PlaySFX("PowerupPickup");
            }
            Destroy(gameObject); // Destroy the orb after pickup
        }
    }
}
