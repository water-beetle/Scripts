using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIPlayerStatItem : MonoBehaviour
{
    public StatType statType;
    public TMP_Text statTypeName;
    public TMP_Text statVal;

    private void Awake()
    {
        statTypeName = GetComponentsInChildren<TMP_Text>()[0];
        statVal = GetComponentsInChildren<TMP_Text>()[1];
    }

    public void SetStatText(StatType _statType, float _statVal)
    {
        statType = _statType;
        statTypeName.SetText(_statType.ToString());
        statVal.SetText(_statVal.ToString());
    }

    public void UpdateVal(float _statVal)
    {
        statVal.SetText(_statVal.ToString());
    }
}
