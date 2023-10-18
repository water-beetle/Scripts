using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class AgentStat : MonoBehaviour
{
    [SerializeField] private AgentStatDataSO agentStatDataSO;
    protected List<StatData> statDataList = new List<StatData>();

    private void Awake()
    {
        for(int i = 0; i < agentStatDataSO.agentStats.Count; i++)
        {
            StatType statType = agentStatDataSO.agentStats[i].statType;
            float val = agentStatDataSO.agentStats[i].val;

            statDataList.Add(new StatData(statType, val));
        }
    }

    public float GetStat(StatType _statType)
    {
        return statDataList.Find(x => x.statType == _statType).Value;
    }

    public virtual void AddStatModifier(StatType _statType, StatModifier _statModifier)
    {
        StatData statData = statDataList.Find(x => x.statType == _statType);
        statData.AddModifier(_statModifier);
    }

    public virtual void RemoveStatModifier(StatType _statType, StatModifier _statModifier)
    {
        StatData statData = statDataList.Find(x => x.statType == _statType);
        statData.RemoveModifier(_statModifier);
    }

    public void AddBuff(StatType _statType, StatModifier _statModifier, int duration)
    {
        AddStatModifier(_statType, _statModifier);
        StartCoroutine(RemoveBuff(_statType, _statModifier, duration));
    }

    public IEnumerator RemoveBuff(StatType _statType, StatModifier _statModifier, int duration)
    {
        yield return new WaitForSeconds(duration);
        RemoveStatModifier(_statType, _statModifier);
    }

}
