using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Agent/Stat")]
public class AgentStatDataSO : ScriptableObject
{
    public List<StatInfo> agentStats = new List<StatInfo>() { };


    [ContextMenu("Initialize")]
    public void Initialize()
    {
        agentStats.Clear();
        foreach (StatType i in Enum.GetValues(typeof(StatType)))
        {
            agentStats.Add(new StatInfo(i));
        }
    }
}

[System.Serializable]
public class StatInfo
{
    public StatType statType;
    public float val;

    public StatInfo(StatType statType)
    {
        this.statType = statType;
    }
}
