using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/UI/EquipInventory", fileName = "EquipmentInventory_")]
public class EquipmentInventorySO : ScriptableObject
{

    [SerializeField] private List<EquipItemSO> equipItems;

    public void Initialize()
    {
        equipItems = new List<EquipItemSO>();
        for(int i=0; i<5; i++)
        {
            equipItems.Add(null);
        }
    }

    public void EquipItem(int index, EquipItemSO equipItem)
    {
        equipItems[index] = equipItem;
    }

    public void UnEquipItem(int index)
    {
        equipItems[index] = null;
    }

    public EquipItemSO GetItem(int index)
    {
        return equipItems[index];
    }

    public List<EquipItemSO> GetEquipItemList() 
    { 
        return equipItems;
    }
}
