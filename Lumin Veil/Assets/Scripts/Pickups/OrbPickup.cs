using UnityEngine;

public class OrbPickup : MonoBehaviour
{
    public ItemDescription orbItem;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryPanel inventoryPanel = FindObjectOfType<InventoryPanel>();
            if (inventoryPanel != null)
            {
                inventoryPanel.AddItemToCharacterInventory(orbItem, 1);
            }
                Destroy(gameObject); // Destroy the orb after pickup
        }
    }
}
