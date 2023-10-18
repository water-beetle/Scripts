using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Item/Equipment", fileName = "Equipment_")]
public class EquipItemSO : ItemSO
{
    public EquipType equipType;
    [SerializeField] private List<Equip_ItemEffectSO> itemEffectsList;

    // 아이템 사용 -> 장비 장착 & 입고있는 장비 해제
    public override void UseItem(Transform transform)
    {
        EquipItem(transform);
    }

    public void EquipItem(Transform transform)
    {
        foreach(var effect in itemEffectsList)
        {
            effect.UseEffect(transform);
        }
    }

    public void UnEquipItem(Transform transform)
    {
        foreach(var effect in itemEffectsList)
        {
            effect.RemoveEffect(transform);
        }
    }

    public int GetEquipIndex()
    {
        return (int)equipType;
    }

}
