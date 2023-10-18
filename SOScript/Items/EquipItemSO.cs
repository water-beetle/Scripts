using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/Item/Equipment", fileName = "Equipment_")]
public class EquipItemSO : ItemSO
{
    public EquipType equipType;
    [SerializeField] private List<Equip_ItemEffectSO> itemEffectsList;

    // ������ ��� -> ��� ���� & �԰��ִ� ��� ����
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
