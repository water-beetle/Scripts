using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class UIInventoryPage : MonoBehaviour
{
    [SerializeField] private UIInventoryItem itemPrefab;
    [SerializeField] private RectTransform contentPanel;
    [SerializeField] private UIInventoryDescription uiDescription;
    [SerializeField] private UIInventoryBackground backgroundInventoryUI;
    [SerializeField] private MouseFollower mouseFollower;

    List<UIInventoryItem> inventoryItemLists = new List<UIInventoryItem>();

    public event Action<int> OnDescriptionRequested, OnItemActionRequested, OnStartDragging;
    public event Action<int, int> OnSwapItems;

    private bool isOpen = false;
    private int currentDragingIndex = -1;

    public void InitializeInventoryUI(int inventorysize)
    {

        for(int i = 0; i < inventorysize; i++)
        {
            UIInventoryItem uiItem = CreateItem();
            SubscribeItemEvent(uiItem);
        }
    }

    private UIInventoryItem CreateItem()
    {
        UIInventoryItem uiItem = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
        uiItem.transform.SetParent(contentPanel);
        uiItem.transform.localScale = Vector3.one;
        inventoryItemLists.Add(uiItem);

        return uiItem;
    }

    protected void SubscribeItemEvent(UIInventoryItem uiItem)
    {
        uiItem.OnItemClicked += HandleItemSelection;
        uiItem.OnItemBeginDrag += HandleBeginDrag;
        uiItem.OnItemDroppedOn += HandleSwap;
        uiItem.OnItemEndDrag += HandleEndDrag;
        uiItem.OnRightMouseBtnClick += HandleUseItemActions;
    }

    public void UpdateItemDataByIndex(int itemIndex, Sprite itemImage, int itemQuantity)
    {
        if(inventoryItemLists.Count > itemIndex)
        {
            inventoryItemLists[itemIndex].SetData(itemImage, itemQuantity);
        }
    }

    private void HandleUseItemActions(UIInventoryItem inventoryItemUI)
    {
        int index = inventoryItemLists.IndexOf(inventoryItemUI);

        if (index == -1)
            return;
        OnItemActionRequested?.Invoke(index);
    }

    private void HandleEndDrag(UIInventoryItem inventoryItemUI)
    {
        ResetDraggedItem();
    }

    private void HandleSwap(UIInventoryItem inventoryItemUI)
    {
        int index = inventoryItemLists.IndexOf(inventoryItemUI);

        if (index == -1)
            return;
        OnSwapItems?.Invoke(currentDragingIndex, index);
        HandleItemSelection(inventoryItemUI);
    }

    private void ResetDraggedItem()
    {
        mouseFollower.Toggle(false);
        currentDragingIndex = -1;
    }

    private void HandleBeginDrag(UIInventoryItem inventoryItemUI)
    {
        int index = inventoryItemLists.IndexOf(inventoryItemUI);
        if (index == -1)
            return;
        currentDragingIndex = index;
        HandleItemSelection(inventoryItemUI);
        OnStartDragging?.Invoke(index);
    }

    public void CreateDraggedItem(Sprite sprite, int quantity)
    {
        mouseFollower.Toggle(true);
        mouseFollower.SetData(sprite, quantity);
    }

    private void HandleItemSelection(UIInventoryItem inventoryItemUI)
    {

        int index = inventoryItemLists.IndexOf(inventoryItemUI);
        if (index == -1)
            return;

        OnDescriptionRequested?.Invoke(index);
    }



    public void InventoryOpen()
    {
        backgroundInventoryUI.Open();
        ResetSelection();
    }

    public void ResetSelection()
    {
        uiDescription.InitDescription();
        DeselectAllItems();
    }

    private void DeselectAllItems()
    {
        foreach (UIInventoryItem item in inventoryItemLists)
        {
            item.DeSelectItem();
        }
    }

    public void InventoryClose()
    {
        backgroundInventoryUI.Close();
        ResetDraggedItem();
    }

    public void ToggleInventory()
    {
        if (backgroundInventoryUI.isDoing)
            return;

        if (!isOpen)
        {
            InventoryOpen();
        }
        else
        {
            InventoryClose();
        }

        isOpen = !isOpen;

    }

    public void UpdateDescription(int itemIndex, Sprite itemImage, string itemName, string itemDescription)
    {
        uiDescription.SetDescription(itemImage, itemName, itemDescription, "asdf");
        DeselectAllItems();
        inventoryItemLists[itemIndex].SelectItem();
    }

    public void ResetAllItems()
    {
        foreach(var item in inventoryItemLists)
        {
            item.ResetData();
            item.DeSelectItem();
        }
    }

    public List<UIInventoryItem> GetFiveItems()
    {
        return inventoryItemLists.GetRange(0, 5);
    }
}
