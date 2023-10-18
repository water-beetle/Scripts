using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static TreeEditor.TreeEditorHelper;

public class StateNodeSO : ScriptableObject
{
    [HideInInspector] public string id;
    [HideInInspector] public List<string> parentStateNodeIDList = new List<string>();
    [HideInInspector] public List<string> childStateNodeIDList = new List<string>();
    [HideInInspector] public FSMGraphSO fsmGraph;
    [HideInInspector] public State stateType = State.IdleState;
    [HideInInspector] public List<StateDecisionSO> stateDecisionList = new List<StateDecisionSO>();

    public StateNodeSO PlayStateDecision()
    {
        foreach(var decision in stateDecisionList)
        {
            if (decision.check())
            {
                return decision.nextState;
            }
        }

        return null;
    }

#if UNITY_EDITOR
    // �����Ϳ��� �߰������� ������ ���� ������ �Լ����� �ʿ��ϴ�.

    // Initialise
    // StateNode�� �����Ϳ��� �����Ǹ� ��� �����Ǿ����� ��ǥ�� �����ؾ� �Ѵ�.
    // �簢�� �̹����� �����ǹǷ� Rect ������ ����

    [HideInInspector] public Rect rect;
    public void Initialise(Rect rect, FSMGraphSO fsmGraph)
    {
        this.rect = rect;
        this.id = Guid.NewGuid().ToString();
        this.name = "RoomNode";
        this.fsmGraph = fsmGraph;
    }

    public bool RemoveDecision(string childStateID)
    {
        foreach(var decision in stateDecisionList)
        {
            if(decision.nextState.id == childStateID)
            {
                stateDecisionList.Remove(decision);
                return true;
            }
        }
        return false;
    }

    public bool RemoveParentStateNodeID(string parentId)
    {
        if (parentStateNodeIDList.Contains(parentId))
        {
            parentStateNodeIDList.Remove(parentId);
            return true;
        }

        return false;
    }

    public bool RemoveChildStateNodeID(string childID)
    {
        if (childStateNodeIDList.Contains(childID))
        {
            childStateNodeIDList.Remove(childID);
            return true;
        }

        return false;
    }

    public bool AddDecision(StateDecisionSO decision)
    {
        stateDecisionList.Add(decision);
        return true;
    }

    public bool AddChildStateNodeID(string childID)
    {
        childStateNodeIDList.Add(childID);
        return true;
    }

    public bool AddParentStateNodeID(string parentID)
    {
        parentStateNodeIDList.Add(parentID);
        return true;
    }

    // �����Ϳ��� �ش� ��尡 ���õǾ����� ����, ���� �� �̵��� ���� ���
    [HideInInspector] public bool isSelected = false;
    // �ش� ��带 �巡���ؼ� �̵������� üũ
    [HideInInspector] public bool isLeftClickDragging = false;

    public void DragNode(Vector2 delta)
    {
        rect.position += delta;
        EditorUtility.SetDirty(this);
    }

    // �����Ϳ��� �ش� ��忡 ���콺�� ���� �� �̺�Ʈ ó��
    public void ProcessEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;

            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;

            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);
                break;

            default:
                break;
        }
    }

    private void ProcessMouseDragEvent(Event currentEvent)
    {
        // process left click drag event
        if (currentEvent.button == 0)
        {
            ProcessLeftMouseDragEvent(currentEvent);
        }
    }


    private void ProcessLeftMouseDragEvent(Event currentEvent)
    {
        isLeftClickDragging = true;
        DragNode(currentEvent.delta);
        GUI.changed = true;
    }


    private void ProcessMouseUpEvent(Event currentEvent)
    {
        // ��� �ű��(���� ���콺 �巡�� �� up) ������
        if(currentEvent.button == 0)
        {
            ProcessLeftClickUpEvent();
        }
    }

    private void ProcessLeftClickUpEvent()
    {
        if (isLeftClickDragging)
        {
            isLeftClickDragging = false;
        }
    }

    private void ProcessMouseDownEvent(Event currentEvent)
    {
        if (currentEvent.button == 0)
        {
            // ��� ��Ŭ�� �� ��� ���� ó��
            ProcessLeftClickDownEvent();
        }

        else if (currentEvent.button == 1)
        {
            // ��� ��Ŭ���� �ٸ� ���� �̾����� �� �׸���
            ProcessRightClickDownEvent(currentEvent);
        }
    }

    private void ProcessRightClickDownEvent(Event currentEvent)
    {
        fsmGraph.SetNodeToDrawConnectionLineFrom(this, currentEvent.mousePosition);
    }


    private void ProcessLeftClickDownEvent()
    {
        Selection.activeObject = this;
        

        if (isSelected == true)
            isSelected = false;
        else
            isSelected = true;
    }

    public void Draw(GUIStyle nodeStyle)
    {
        GUILayout.BeginArea(rect, nodeStyle);
        EditorGUI.BeginChangeCheck();

        int selected = (int)stateType;
        int selection = EditorGUILayout.Popup("", selected, GetStateNodeTypesToDisPlay());

        stateType = (State)selection;
            

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(this);

        GUILayout.EndArea();
    }

    private string[] GetStateNodeTypesToDisPlay()
    {
        string[] nodeTypeList = new string[System.Enum.GetValues(typeof(State)).Length];

        for(int i=0; i< nodeTypeList.Length; i++)
        {
            nodeTypeList[i] = ((State)i).ToString();
        }


        return nodeTypeList;

    }


#endif
}
