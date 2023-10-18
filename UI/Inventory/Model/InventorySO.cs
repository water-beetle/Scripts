using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

[CreateAssetMenu(menuName ="ScriptableObjects/UI/Inventory", fileName = "Inventory_")]
public class InventorySO : ScriptableObject
{
    [SerializeField] private List<InventoryItem> inventoryItems;

    public event Action<Dictionary<int, InventoryItem>> OnInventoryChanged;

    public int size = 10;

    public void Initialize()
    {
        inventoryItems = new List<InventoryItem>();
        for (int i = 0; i < size; i++)
        {
            inventoryItems.Add(InventoryItem.GetEmptyItem());
        }
    }

    public int AddItem(ItemSO item, int quantity)
    {
        if(item.isStackable == false)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                while(quantity > 0 && HasInventorySpace() == true)
                {
                    quantity -= AddItemToFirstFreeSlot(item, 1);

                }
            }

            InformAboutChange();
            return quantity;
        }

        quantity = AddStackableItem(item, quantity);
        InformAboutChange();
        return quantity;

    }

    private int AddItemToFirstFreeSlot(ItemSO item, int quantity)
    {
        InventoryItem newItem = new InventoryItem() {item = item, quantity = quantity};

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
            {
                inventoryItems[i] = newItem;
                return quantity;
            }
        }

        return 0;

    }

    private bool HasInventorySpace() => inventoryItems.Where(item => item.IsEmpty).Any();

    private int AddStackableItem(ItemSO item, int quantity)
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
                continue;
            if (inventoryItems[i].item.ID == item.ID)
            {
                int amountPossibleToTake = inventoryItems[i].item.maxStackSize - inventoryItems[i].quantity;

                if(quantity > amountPossibleToTake)
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].item.maxStackSize);
                    quantity -= amountPossibleToTake;
                }
                else
                {
                    inventoryItems[i] = inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);
                    InformAboutChange();
                    return 0;
                }
            }
        }

        while(quantity > 0 && HasInventorySpace())
        {
            int newQuantity = Mathf.Clamp(quantity, 0, item.maxStackSize);
            quantity -= newQuantity;
            AddItemToFirstFreeSlot(item, newQuantity);
        }

        return quantity;
    }

    public Dictionary<int, InventoryItem> GetCurrentInventoryState()
    {
        Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            if (inventoryItems[i].IsEmpty)
                continue;
            returnValue[i] = inventoryItems[i];
        }

        return returnValue;
    }

    public InventoryItem GetItemAt(int itemIndex)
    {
        return inventoryItems[itemIndex];
    }

    public void AddItem(InventoryItem item)
    {
        AddItem(item.item, item.quantity);
    }

    public void RemoveItem(int itemIndex, int amount)
    {
        if(inventoryItems.Count > itemIndex)
        {
            if (inventoryItems[itemIndex].IsEmpty)
                return;
            int remainder = inventoryItems[itemIndex].quantity - amount;
            if(remainder <= 0)
            {
                inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
            }
            else
            {
                inventoryItems[itemIndex] = inventoryItems[itemIndex].ChangeQuantity(remainder);
            }

            InformAboutChange();
        }
    }

    public bool TryGetInventoryItem(int index, out InventoryItem item)
    {
        Dictionary<int, InventoryItem> inventoryDict = GetCurrentInventoryState();

        if(inventoryDict.TryGetValue(index, out item))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void SwapItems(int itemIndex1, int itemIndex2)
    {
        if(itemIndex1 < 0 || itemIndex2 < 0)
            return;
        InventoryItem item1 = inventoryItems[itemIndex1];
        inventoryItems[itemIndex1] = inventoryItems[itemIndex2];
        inventoryItems[itemIndex2] = item1;
        InformAboutChange();
    }

    private void InformAboutChange()
    {
        OnInventoryChanged?.Invoke(GetCurrentInventoryState());
    }
}

[Serializable]
public struct InventoryItem
{
    public int quantity;
    public ItemSO item;

    public InventoryItem(ItemSO item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public bool IsEmpty => item == null;

    public InventoryItem ChangeQuantity(int newQuantity)
    {
        return new InventoryItem
        {
            item = this.item,
            quantity = newQuantity
        };
    }

    public static InventoryItem GetEmptyItem()
    {
        return new InventoryItem
        {
            item = null,
            quantity = 0
        };
    }
}