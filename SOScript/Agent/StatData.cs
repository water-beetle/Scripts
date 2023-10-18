using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatData
{
    public StatType statType; 
    public float BaseValue;
    private float lastBaseValue = float.MinValue;

    public StatData(StatType _statType, float _BaseValue)
    {
        statType = _statType;
        BaseValue = _BaseValue;
    }

    public float Value
    {
        get
        {
            if (isDirty || BaseValue != lastBaseValue)
            {
                lastBaseValue = BaseValue;
                _value = CalculateFinalValue();
                isDirty = false;
            }
            return _value;
        }
    }

    private bool isDirty = true;
    private float _value;
    public List<StatModifier> statModifiers = new List<StatModifier>();

    public void AddModifier(StatModifier mod)
    {
        isDirty = true;
        statModifiers.Add(mod);
        statModifiers.Sort(CompareModifierOrder);
    }

    private int CompareModifierOrder(StatModifier a, StatModifier b)
    {
        if (a.Order < b.Order)
            return -1;
        else if (a.Order > b.Order)
            return 1;
        return 0;
    }

    public bool RemoveModifier(StatModifier mod)
    {

        if (statModifiers.Remove(mod))
        {
            isDirty = true;
            return true;
        }

        return false;
    }

    public bool RemoveAllModifiersFromsource(object source)
    {
        bool didRemove = false;

        for(int i=statModifiers.Count - 1; i >= 0; i--)
        {
            if(statModifiers[i].Source == source)
            {
                isDirty = true;
                didRemove = true;
                statModifiers.RemoveAt(i);
            }
        }

        return didRemove;
    }

    private float CalculateFinalValue()
    {
        float finalValue = BaseValue;
        float sumPercentAdd = 0;

        for(int i=0; i<statModifiers.Count; i++)
        {
            StatModifier mod = statModifiers[i];

            if(mod.Type == StatAddType.Flat)
            {
                finalValue += mod.Value;
            }
            else if (mod.Type == StatAddType.PercentAdd)
            {
                sumPercentAdd += mod.Value;

                if(i + 1 >= statModifiers.Count || statModifiers[i+1].Type != StatAddType.PercentAdd)
                {
                    finalValue *= 1 + sumPercentAdd;
                    sumPercentAdd = 0;
                }
            }
            else if(mod.Type == StatAddType.PercentMul)
            {
                finalValue *= 1 + mod.Value;
            }


        }

        return (float)Math.Round(finalValue, 4);
    }

}
