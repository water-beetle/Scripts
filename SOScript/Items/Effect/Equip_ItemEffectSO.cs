using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item/Effect/EquipBuff", fileName = "ItemEffect_Equip_")]
public class Equip_ItemEffectSO : ItemEffectSO
{
    [SerializeField] private StatType statType;
    [SerializeField] private StatAddType addType;
    [SerializeField] private float val;
    private StatModifier statModifier;


    public override void UseEffect(Transform transform)
    {
        if(statModifier.Value == 0)
            statModifier = new StatModifier(val, addType);
        transform.GetComponent<AgentStat>()?.AddStatModifier(statType, statModifier);
    }

    public void RemoveEffect(Transform transform)
    {
        transform.GetComponent<AgentStat>()?.RemoveStatModifier(statType, statModifier);        
    }

}
