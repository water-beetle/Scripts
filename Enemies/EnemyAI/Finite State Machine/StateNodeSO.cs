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
    // 에디터에서 추가적으로 다음과 같은 변수와 함수들이 필요하다.

    // Initialise
    // StateNode가 에디터에서 생성되면 어디서 생성되었는지 좌표를 관리해야 한다.
    // 사각형 이미지가 생성되므로 Rect 변수로 관리

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

    // 에디터에서 해당 노드가 선택되었는지 여부, 삭제 및 이동을 위해 사용
    [HideInInspector] public bool isSelected = false;
    // 해당 노드를 드래깅해서 이동중인지 체크
    [HideInInspector] public bool isLeftClickDragging = false;

    public void DragNode(Vector2 delta)
    {
        rect.position += delta;
        EditorUtility.SetDirty(this);
    }

    // 에디터에서 해당 노드에 마우스가 있을 시 이벤트 처리
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
        // 노드 옮기기(왼쪽 마우스 드래그 후 up) 끝내기
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
            // 노드 좌클릭 시 노드 선택 처리
            ProcessLeftClickDownEvent();
        }

        else if (currentEvent.button == 1)
        {
            // 노드 우클릭시 다른 노드와 이어지는 선 그리기
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
