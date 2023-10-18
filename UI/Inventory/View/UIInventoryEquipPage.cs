using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIInventoryEquipPage : MonoBehaviour
{
    [SerializeField] private RectTransform contentPanel;
    List<UIInventoryItem> equipItemList = new List<UIInventoryItem>();

    public event Action<int> OnItemActionRequested;


    public void InitializeInventoryEquipUI()
    {
        for (int i = 0; i < contentPanel.childCount; i++)
        {
            UIInventoryItem equipItem = contentPanel.GetChild(i).GetComponent<UIInventoryItem>();
            equipItemList.Add(equipItem);
            SubscribeItemEvent(equipItem);
        }
    }

    private void SubscribeItemEvent(UIInventoryItem uiItem)
    {
        uiItem.OnRightMouseBtnClick += HandleUseItemActions;
    }

    private void HandleUseItemActions(UIInventoryItem inventoryItemUI)
    {
        int index = equipItemList.IndexOf(inventoryItemUI);
        OnItemActionRequested?.Invoke(index);
    }

    public void UpdateUI(List<EquipItemSO> equipDataList)
    {
        for(int i=0; i< equipDataList.Count; i++)
        {
            if (equipDataList[i] == null)
            {
                equipItemList[i].ResetData();
            }
            else
            {
                equipItemList[i].SetData(equipDataList[i].itemImage, 1);
            }
        }
    }
}
