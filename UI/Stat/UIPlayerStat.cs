using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPlayerStat : MonoBehaviour
{
    [SerializeField] private UIPlayerStatItem itemPrefab;
    private List<UIPlayerStatItem> statList = new List<UIPlayerStatItem>();

    private int ItemHeightSpace = -30;
    private int StartItemHeight = 0;
    private int StartItemWidth = 100;


    public void InitStatList(List<StatData> statData)
    {
            statList.Clear();
            for (int i = 0; i < statData.Count; i++)
            {
                statList.Add(CreatePrefab(i, statData[i]));
            }
    }

    public void UpdateStatList(StatType _statType, float val)
    {
        statList.Find(x => x.statType == _statType).UpdateVal(val);
    }

    private UIPlayerStatItem CreatePrefab(int num, StatData data)
    {
        UIPlayerStatItem created = Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);
        created.transform.SetParent(transform);
        created.SetStatText(data.statType, data.Value);
        created.GetComponent<RectTransform>().anchoredPosition = GetPositionByNum(num);

        return created;
    }

    private Vector3 GetPositionByNum(int num)
    {
        return new Vector3(StartItemWidth, StartItemHeight + ItemHeightSpace * num, 0);
    }
}
