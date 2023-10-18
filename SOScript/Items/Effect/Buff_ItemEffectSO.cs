using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Item/Effect/Buff", fileName = "ItemEffect_Buff_")]
public class Buff_ItemEffectSO : ItemEffectSO
{
    [SerializeField] private StatType statType;
    [SerializeField] private StatAddType addType;
    [SerializeField] private float val;
    [SerializeField] private int duration;

    public override void UseEffect(Transform transform)
    {
        Vector3 pos = transform.position;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(pos, 2, LayerMask.GetMask("Agent"));

        foreach(var collider in colliders)
        {
            collider.GetComponent<AgentStat>()?.AddBuff(statType, new StatModifier(val, addType), duration);
        }
    }
}
