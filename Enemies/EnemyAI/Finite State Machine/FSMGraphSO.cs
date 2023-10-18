using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

[CreateAssetMenu(fileName = "FSM_", menuName = "ScriptableObjects/FSM")]
public class FSMGraphSO : ScriptableObject
{
    [HideInInspector] public List<StateNodeSO> stateNodeList = new List<StateNodeSO>();
    [HideInInspector] public Dictionary<string, StateNodeSO> stateNodeDictionary = new Dictionary<string, StateNodeSO>();
    [HideInInspector] public StateNodeSO startStateNode;
    [HideInInspector] public EnemyAI enemyAI;
    

    private void Awake()
    {
        LoadRoomDictionary();
    }

    private void LoadRoomDictionary()
    {
        stateNodeDictionary.Clear();

        foreach(StateNodeSO node in stateNodeList)
        {
            stateNodeDictionary[node.id] = node;
        }
    }

    public StateNodeSO GetStateNodeByID(string stateNodeID)
    {
        if(stateNodeDictionary.TryGetValue(stateNodeID, out StateNodeSO stateNode))
        {
            return stateNode;
        }

        return null;
    }

#if UNITY_EDITOR

    [HideInInspector] public StateNodeSO stateNodeToDrawLineFrom = null;
    [HideInInspector] public Vector2 linePosition;

    public void OnValidate()
    {
        LoadRoomDictionary();
    }

    internal void SetNodeToDrawConnectionLineFrom(StateNodeSO node, Vector2 mousePosition)
    {
        stateNodeToDrawLineFrom = node;
        linePosition = mousePosition;
    }
#endif
}
