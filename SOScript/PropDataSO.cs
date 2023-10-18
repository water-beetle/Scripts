using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PropData_", menuName = "ScriptableObjects/Props")]
public class PropDataSO : ScriptableObject
{
    public Sprite sprite;
    public Vector2 size;
    [Range(1, 10)]
    public int minPropNum;
    [Range(1, 10)]
    public int maxPropNum;
    public bool isBreakable;

#if UNITY_EDITOR
    private void OnValidate()
    {
        
    }

#endif
}
