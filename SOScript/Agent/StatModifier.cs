using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StatModifier
{
    public float Value;
    public StatAddType Type;
    public int Order;
    public object Source;

    public StatModifier(float value, StatAddType type, int order, object source)
    {
        Value = value;
        Type = type;
        Order = order;
        Source = source;
    }

    public StatModifier(float value, StatAddType type) : this(value, type, (int)type, null) { }
    public StatModifier(float value, StatAddType type, int order) : this(value, type, order, null) { }
    public StatModifier(float value, StatAddType type, object source) : this(value, type, (int)type, source) { }

}
