using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ItemDescription
{
    public string Name;
    public Texture2D Icon;

}

public class InventoryItem
{
    public ItemDescription item;
    public int count;

    public bool isEmpty => this == Empty;

    public const int maxCount = 20;
    public static readonly InventoryItem Empty = new InventoryItem();
   
}

[CreateAssetMenu(menuName = "Inventory/Items")]
public class ItemDatabase : ScriptableObject
{
    [SerializeField]
    private List<ItemDescription> items = new List<ItemDescription>();

    public List<InventoryItem> GetRandomItems(int count)
    {
        List<InventoryItem> toRet = new List<InventoryItem>();
        for (int i = 0; i < count ; i++)
        {
            if (Random.Range(0, 1f) > .5f)
            {
                toRet.Add(InventoryItem.Empty);
            }
            else
            {
                toRet.Add(new InventoryItem
                {
                    item = items[Random.Range(0, items.Count)],
                    count = Random.Range(1, InventoryItem.maxCount) 
                });
            }
        }

        return toRet;
    }

    public List<InventoryItem> GetEmptyItems(int count)
    {
        List<InventoryItem> toRet = new List<InventoryItem>();
        for (int i = 0; i < count; i++)
        {
            toRet.Add(InventoryItem.Empty);
        }
        return toRet;
    }
}
