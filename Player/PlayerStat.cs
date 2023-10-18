using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStat : AgentStat
{
    [SerializeField] private UIPlayerStat uiPlayerStat;

    // Start is called before the first frame update
    void Start()
    {
        uiPlayerStat.InitStatList(statDataList);
    }

    private void UpdateUI(StatType _statType)
    {
        uiPlayerStat.UpdateStatList(_statType, statDataList.Find(x => x.statType == _statType).Value);
    }

    public override void AddStatModifier(StatType _statType, StatModifier _statModifier)
    {
        base.AddStatModifier(_statType, _statModifier);
        UpdateUI(_statType);
    }

    public override void RemoveStatModifier(StatType _statType, StatModifier _statModifier)
    {
        base.RemoveStatModifier(_statType, _statModifier);
        UpdateUI(_statType);
    }

}
