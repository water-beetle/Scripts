using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryShortcut : MonoBehaviour
{

    List<UIInventoryItem> shortCutItemLists = new List<UIInventoryItem>();

    public void InitializeIngameInventoryUI()
    {
        for(int i=0; i<5; i++)
        {
            UIInventoryItem item = transform.GetChild(i).GetComponent<UIInventoryItem>();
            shortCutItemLists.Add(item);
        }
    }

    public void ResetAll()
    {
        foreach(var item in shortCutItemLists)
        {
            item.ResetData();
        }
    }

    public void UpdateIngameInventory(int index, Sprite itemImage, int quantity)
    {
        shortCutItemLists[index].SetData(itemImage, quantity);
    }
}
