using Nova;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Linq;

public class InventoryPanel : MonoBehaviour
{
    public ItemDatabase ItemDatabase = null;

    [Header("Character")]
    public GridView CharacterGrid = null;
    public int CharacterCount = 24;

    [Header("PowerUps")]
    public GridView PowerUpGrid = null;
    public int PowerUpCount = 24;

    [Header("Row Styling")]
    public RadialGradient RowGradient;

    [HideInInspector]
    public List<InventoryItem> characterItems = null;
    private List<InventoryItem> powerUpItems = null;

    private void Start()
    {
        characterItems = ItemDatabase.GetEmptyItems(CharacterCount);
        powerUpItems = ItemDatabase.GetEmptyItems(PowerUpCount);

        InitGrid(CharacterGrid, characterItems);
        InitGrid(PowerUpGrid, powerUpItems);
    }

    public int GetOrbCount()
    {
        return characterItems.FindAll(x => x.item != null && x.item.Name == "Orb").Sum(x => x.count);
    }

    public bool HasRangeAttack()
    {
        return powerUpItems.Any(x => x.item != null && x.item.Name == "RangeAttack");
    }
    public void AddItemToCharacterInventory(ItemDescription itemDesc, int count = 1)
    {
        var existing = characterItems.Find(x => x.item == itemDesc);

        if (existing != null)
        {
            existing.count = Mathf.Min(existing.count + count, InventoryItem.maxCount);
        }
        else
        {
            int emptyIndex = characterItems.FindIndex(x => x.isEmpty);
            if (emptyIndex != -1)
            {
                characterItems[emptyIndex] = new InventoryItem
                {
                    item = itemDesc,
                    count = count
                };
            }
        }

        if (CharacterGrid.gameObject.activeInHierarchy)
        {
            CharacterGrid.Refresh();
        }
    }

    public void AddItemToPowerUpsInventory(ItemDescription itemDesc, int count = 1)
    {
        var existing = powerUpItems.Find(x => x.item == itemDesc);

        if (existing != null)
        {
            existing.count = Mathf.Min(existing.count + count, InventoryItem.maxCount);
        }
        else
        {
            int emptyIndex = powerUpItems.FindIndex(x => x.isEmpty);
            if (emptyIndex != -1)
            {
                powerUpItems[emptyIndex] = new InventoryItem
                {
                    item = itemDesc,
                    count = count
                };
            }
        }

        if (PowerUpGrid.gameObject.activeInHierarchy)
        {
            PowerUpGrid.Refresh();
        }
    }

    private void InitGrid(GridView grid, List<InventoryItem> datasource)
    {

        grid.AddDataBinder<InventoryItem, InventoryItemVisuals>(BindItem);

        grid.SetSliceProvider(ProvideSlice);

        grid.AddGestureHandler<Gesture.OnHover, InventoryItemVisuals>(HandleHover);
        grid.AddGestureHandler<Gesture.OnUnhover, InventoryItemVisuals>(HandleUnhover);
        grid.AddGestureHandler<Gesture.OnPress, InventoryItemVisuals>(HandlePress);
        grid.AddGestureHandler<Gesture.OnRelease, InventoryItemVisuals>(HandleRelease);

        grid.SetDataSource(datasource);
    }


    private void HandleRelease(Gesture.OnRelease evt, InventoryItemVisuals target, int index) => target.Release();
    private void HandlePress(Gesture.OnPress evt, InventoryItemVisuals target, int index) => target.Press();
    private void HandleUnhover(Gesture.OnUnhover evt, InventoryItemVisuals target, int index) => target.Unhover();
    private void HandleHover(Gesture.OnHover evt, InventoryItemVisuals target, int index) => target.Hover();


    private void ProvideSlice(int sliceIndex, GridView gridview, ref GridSlice2D gridslice)
    {
        gridslice.Layout.AutoSize.Y = AutoSize.Shrink;
        gridslice.AutoLayout.AutoSpace = true;
        gridslice.Layout.Padding.Value = 30;

        gridslice.Gradient = RowGradient;
    }
    private void BindItem(Data.OnBind<InventoryItem> evt, InventoryItemVisuals target, int index) => target.Bind(evt.UserData);
}
