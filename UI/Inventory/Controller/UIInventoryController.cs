using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIInventoryController : MonoBehaviour
{
    [SerializeField] private UIInventoryPage inventoryUI;
    [SerializeField] private UIInventoryShortcut inGameInventoryUI;
    [SerializeField] private UIInventoryEquipPage inventoryEquipUI;

    [SerializeField] private InventorySO inventoryData;
    [SerializeField] private EquipmentInventorySO inventoryEquipData;

    [SerializeField] private AgentThrow agentThrow;

    public List<InventoryItem> initialItems = new List<InventoryItem>();

    private int throwIndex = -1;

    private void Awake()
    {
        agentThrow = GetComponent<AgentThrow>();
    }

    private void Start()
    {
        PrepareUI();
        PrepareInventoryData();
    }

    private void PrepareInventoryData()
    {
        inventoryData.Initialize();
        inventoryEquipData.Initialize();
        inGameInventoryUI.InitializeIngameInventoryUI();
        inventoryData.OnInventoryChanged += UpdateInventoryUI;
        foreach (InventoryItem item in initialItems)
        {
            if (item.IsEmpty)
                continue;
            inventoryData.AddItem(item);
        }
    }

    private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
    {
        inventoryUI.ResetAllItems();
        foreach (var item in inventoryState)
        {
            inventoryUI.UpdateItemDataByIndex(item.Key, item.Value.item.itemImage, item.Value.quantity);
        }

        inGameInventoryUI.ResetAll();
        foreach(var item in inventoryState)
        {
            if (item.Key >= 5)
                continue;

            inGameInventoryUI.UpdateIngameInventory(item.Key, inventoryState[item.Key].item.itemImage, inventoryState[item.Key].quantity);
        }


    }

    private void PrepareUI()
    {
        inventoryEquipUI.InitializeInventoryEquipUI();
        inventoryEquipUI.OnItemActionRequested += HandleItemUnEquiped;

        inventoryUI.InitializeInventoryUI(inventoryData.size);
        inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
        inventoryUI.OnSwapItems += HandleSwapItems;
        inventoryUI.OnStartDragging += HandleDragging;
        inventoryUI.OnItemActionRequested += HandleItemActionRequested;
    }

    private void HandleItemActionRequested(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
            return;
        UseItem(itemIndex, inventoryItem);
    }

    private void HandleDragging(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
            return;
        inventoryUI.CreateDraggedItem(inventoryItem.item.itemImage, inventoryItem.quantity);
    }

    private void HandleSwapItems(int itemIndex1, int itemIndex2)
    {
        inventoryData.SwapItems(itemIndex1, itemIndex2);
    }

    private void HandleDescriptionRequest(int itemIndex)
    {
        InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            inventoryUI.ResetSelection();
            return;
        }
            

        ItemSO item = inventoryItem.item;
        inventoryUI.UpdateDescription(itemIndex, item.itemImage, item.itemName, item.itemDescription);
    }

    public void ToggleInventory()
    {
        inventoryUI.ToggleInventory();
        UpdateInventory();
    }

    private void UpdateInventory()
    {
        foreach (var item in inventoryData.GetCurrentInventoryState())
        {
            inventoryUI.UpdateItemDataByIndex(item.Key, item.Value.item.itemImage, item.Value.quantity);
        }

    }

    private void UpdateEquipInventory()
    {
        inventoryEquipUI.UpdateUI(inventoryEquipData.GetEquipItemList());
    }

    public void ItemButtonPressed(int index)
    {
        InventoryItem inventoryItem;

        if(!inventoryData.TryGetInventoryItem(index, out inventoryItem))
            return;

        if (inventoryItem.item.isThrowable)
        {
            throwIndex = index;
            agentThrow.SetTrajectory(true);
        }
        else
        {
            UseItem(index, inventoryItem);
        }
    }

    private void UseItem(int index, InventoryItem inventoryItem)
    {
        if(inventoryItem.item.itemType == ItemType.Equipment)
        {
            UnEquipItem(((EquipItemSO)inventoryItem.item).GetEquipIndex());
            EquipItem((EquipItemSO)inventoryItem.item);
        }

        inventoryItem.item.UseItem(gameObject.transform);
        inventoryData.RemoveItem(index, 1);
        UpdateEquipInventory();
    }


    private void UnEquipItem(int unEquipIndex)
    {
        EquipItemSO unequipItem = inventoryEquipData.GetItem(unEquipIndex);

        if (unequipItem != null)
        {
            unequipItem.UnEquipItem(transform);
            inventoryData.AddItem(unequipItem, 1);
        }
    
        inventoryEquipData.UnEquipItem(unEquipIndex);
        UpdateEquipInventory();
    }

    private void EquipItem(EquipItemSO equipItem)
    {
        int equipIndex = equipItem.GetEquipIndex();
        inventoryEquipData.EquipItem(equipIndex, equipItem);
    }

    public void ResetThrowItemIndex(Component sender, object data)
    {
        ResetThrowItemIndex();
    }

    public void ResetThrowItemIndex()
    {
        throwIndex = -1;
    }

    public void ThrowItem(Component sender, object data)
    {
        if (throwIndex == -1)
            return;

        agentThrow.ThrowItem(inventoryData.GetItemAt(throwIndex).item);
        inventoryData.RemoveItem(throwIndex, 1);
        ResetThrowItemIndex();

    }

    public void HandleItemUnEquiped(int itemIndex)
    {
        UnEquipItem(itemIndex);
    }
}
