using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemEffectSO : ScriptableObject
{
    public abstract void UseEffect(Transform transform);
}
