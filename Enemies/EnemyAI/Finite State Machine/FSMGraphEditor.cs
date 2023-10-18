using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class FSMGraphEditor : EditorWindow
{
    private GUIStyle stateNodeStyle;
    private GUIStyle stateNodeSelectedStyle;
    private static FSMGraphSO currentFSMGraph;

    private StateNodeSO currentStateNode;
    private StateNodeSO lastSelectNode;

    // 노드 스타일 설정 값
    private const float nodeWidth = 160f;
    private const float nodeHeight = 75f;
    private const int nodePadding = 25;
    private const int nodeBorder = 12;


    // 팝업창 설정
    public Rect windowRect = new Rect(0, 0, 300, 500);

    // 선 스타일
    private const float connectingLineWidth = 3f;
    private const float connectingLineArrowSize = 6f;

    // 그래프 설정
    private Vector2 graphOffset;
    private Vector2 graphDrag;




    // 에디터 창 실행 함수
    [MenuItem("FSM Graph Editor", menuItem = "Window/FSM Graph Editor")]
    private static void OpenWindow()
    {
        GetWindow<FSMGraphEditor>("FSM Graph Editor");
    }

    private void OnEnable()
    {
        // 나중에 추가
        //Selection.selectionChanged += InspectorSelectionChanged;

        // 에디터에서 생성할 노드 스타일 설정
        stateNodeStyle = new GUIStyle();
        stateNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
        stateNodeStyle.normal.textColor = Color.white;
        stateNodeStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        stateNodeStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

        stateNodeSelectedStyle = new GUIStyle();
        stateNodeSelectedStyle.normal.background = EditorGUIUtility.Load("node1 on") as Texture2D;
        stateNodeSelectedStyle.normal.textColor = Color.white;
        stateNodeSelectedStyle.padding = new RectOffset(nodePadding, nodePadding, nodePadding, nodePadding);
        stateNodeSelectedStyle.border = new RectOffset(nodeBorder, nodeBorder, nodeBorder, nodeBorder);

    }

    // 유니티 창에서 에셋을 더블클릭 실행했을시
    // 실행되는 UnityEditor.Callbacks의 콜백함수
    [OnOpenAsset(0)]
    public static bool OnDoubleClickAsset(int instanceID, int line)
    {
        FSMGraphSO fsmGraph = EditorUtility.InstanceIDToObject(instanceID) as FSMGraphSO;

        if(fsmGraph != null)
        {
            OpenWindow();
            currentFSMGraph = fsmGraph;
            return true;
        }

        return false;
    }

    // 에디터의 메인 함수
    private void OnGUI()
    {

        if (currentFSMGraph && currentFSMGraph.stateNodeList.Count == 0)
        {
            CreateStateNode(new Vector2(100, 100));
            currentFSMGraph.startStateNode = currentFSMGraph.stateNodeList.First();
        }

        if (currentFSMGraph != null)
        {
            BeginWindows();
            windowRect = GUILayout.Window(1, windowRect, DoWindow, "State Decision Info");
            EndWindows();


            DrawDraggedLine();
            // 에디터의 이벤트 처리 - 클릭, 드래그 등등
            ProcessEvents(Event.current);
            DrawStateConnections();
            DrawStateNodes();
        }
    }

    void DoWindow(int unusedWindowID)
    {
        if(lastSelectNode != null)
        {
            int count = 0;

            foreach(var decision in lastSelectNode.stateDecisionList)
            {
                EditorGUILayout.BeginHorizontal();
                GUI.color = Color.green;
                GUILayout.Label("Decision_" + count + ": " + decision.currentState.stateType + "->" + decision.nextState.stateType);
                GUI.color = Color.white;
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("distanceCheck");
                decision.distanceCheck = EditorGUILayout.Toggle(decision.distanceCheck);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("isDistanceLarger");
                decision.isDistanceLarger = EditorGUILayout.Toggle(decision.isDistanceLarger);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("distance");
                decision.distance = EditorGUILayout.IntSlider(decision.distance, 1, 20);
                EditorGUILayout.EndHorizontal();

                count += 1;

                GUI.changed = true;
            }

            
            
        }
            
        GUI.DragWindow();
    }

    private void DrawStateNodes()
    {
        foreach(StateNodeSO stateNode in currentFSMGraph.stateNodeList)
        {
            if (stateNode.isSelected)
                stateNode.Draw(stateNodeSelectedStyle);
            else
                stateNode.Draw(stateNodeStyle);
        }

        GUI.changed = true;
    }

    private void DrawStateConnections()
    {
        foreach(StateNodeSO stateNode in currentFSMGraph.stateNodeList)
        {
            if(stateNode.childStateNodeIDList.Count > 0)
            {
                foreach(string childStateNodeID in stateNode.childStateNodeIDList)
                {
                    if (currentFSMGraph.stateNodeDictionary.ContainsKey(childStateNodeID))
                    {
                        DrawConnectionLine(stateNode, currentFSMGraph.stateNodeDictionary[childStateNodeID]);
                        GUI.changed = true;
                     
                    }
                }
            }
        }
    }

    private void DrawConnectionLine(StateNodeSO parentStateNode, StateNodeSO childStateNode)
    {
        Vector2 startPosition = parentStateNode.rect.center;
        Vector2 endPosition = childStateNode.rect.center;
        Vector2 midPosition = (endPosition + startPosition) / 2f;

        Vector2 direction = endPosition - startPosition;

        Vector2 arrowTailPoint1 = midPosition - new Vector2(-direction.y, direction.x).normalized * connectingLineArrowSize;
        Vector2 arrowTailPoint2 = midPosition + new Vector2(-direction.y, direction.x).normalized * connectingLineArrowSize;

        Vector2 arrowHeadPoint = midPosition + direction.normalized * connectingLineArrowSize;

        Handles.DrawBezier(arrowHeadPoint, arrowTailPoint1, arrowHeadPoint, arrowTailPoint1, Color.white, null, connectingLineWidth);
        Handles.DrawBezier(arrowHeadPoint, arrowTailPoint2, arrowHeadPoint, arrowTailPoint2, Color.white, null, connectingLineWidth);

        Handles.DrawBezier(startPosition, endPosition, startPosition, endPosition, Color.white, null, connectingLineWidth);

        GUI.changed = true;


    }

    private void DrawDraggedLine()
    {
        if(currentFSMGraph.linePosition != Vector2.zero)
        {
            Handles.DrawBezier(currentFSMGraph.stateNodeToDrawLineFrom.rect.center, currentFSMGraph.linePosition,
                currentFSMGraph.stateNodeToDrawLineFrom.rect.center, currentFSMGraph.linePosition, Color.white, null, connectingLineWidth);
        }
    }

    private void ProcessEvents(Event currentEvent)
    {
        graphDrag = Vector2.zero;

        if(currentStateNode == null || currentStateNode.isLeftClickDragging == false)
        {
            currentStateNode = IsMouseOverStateNode(currentEvent);
        }
        if(currentStateNode == null || currentFSMGraph.stateNodeToDrawLineFrom != null)
        {
            ProcessGraphEvents(currentEvent);
        }
        else
        {
            currentStateNode.ProcessEvents(currentEvent);
            SelectNode(currentEvent);
        }



    }

    private void SelectNode(Event currentEvent)
    {
        if(currentEvent.button == 0 && currentEvent.type == EventType.MouseDown)
        {
            lastSelectNode = currentStateNode;
        }
    }

    private StateNodeSO IsMouseOverStateNode(Event currentEvent)
    {
        for(int i = currentFSMGraph.stateNodeList.Count - 1; i>= 0; i--)
        {
            if (currentFSMGraph.stateNodeList[i].rect.Contains(currentEvent.mousePosition))
            {
                return currentFSMGraph.stateNodeList[i];
            }
        }

        return null;
    }

    private void ProcessGraphEvents(Event currentEvent)
    {
        switch (currentEvent.type)
        {
            // Process Mouse Down Events
            case EventType.MouseDown:
                ProcessMouseDownEvent(currentEvent);
                break;

            // Process Mouse Up Events
            case EventType.MouseUp:
                ProcessMouseUpEvent(currentEvent);
                break;

            // Process Mouse Drag Event
            case EventType.MouseDrag:
                ProcessMouseDragEvent(currentEvent);

                break;

            default:
                break;
        }
    }

    private void ProcessMouseDownEvent(Event currentEvent)
    {
        // 우클릭 -> 해당 지점에 생성 메뉴창 보이기
        if (currentEvent.button == 1)
        {
            ShowContextMenu(currentEvent.mousePosition);
        }
        // 빈 곳에 클릭시 그리고 있는 선과 선택 지우기
        else if (currentEvent.button == 0)
        {
            ClearLineDrag();
            ClearAllSelectedStateNodes();
        }
    }

    private void ClearAllSelectedStateNodes()
    {
        foreach (StateNodeSO stateNode in currentFSMGraph.stateNodeList)
        {
            if (stateNode.isSelected)
            {
                stateNode.isSelected = false;
                GUI.changed = true;
            }
        }
    }

    private void ClearLineDrag()
    {
        currentFSMGraph.stateNodeToDrawLineFrom = null;
        currentFSMGraph.linePosition = Vector2.zero;
        GUI.changed = true;
    }

    private void ShowContextMenu(Vector2 mousePosition)
    {
        GenericMenu menu = new GenericMenu();

        // 메뉴창에 보일 이름, 미리 선택 여부, 콜백함수, 함수인자
        menu.AddItem(new GUIContent("Create State Node"), false, CreateStateNode, mousePosition);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Select All State Nodes"), false, SelectAllStateNodes);
        menu.AddSeparator("");
        menu.AddItem(new GUIContent("Delete Selected State Node Links"), false, DeleteSelectedStateNodeLinks);
        menu.AddItem(new GUIContent("Delete Selected State Nodes"), false, DeleteSelectedStateNodes);

        menu.ShowAsContext();
    }

    private void DeleteSelectedStateNodes()
    {
        Queue<StateNodeSO> stateNodeDeletionQueue = new Queue<StateNodeSO>();

        // 선택된 노드의 부모, 자식에서 현재 노드에 대한 정보를 제거하기
        foreach(StateNodeSO stateNode in currentFSMGraph.stateNodeList)
        {
            if(stateNode.isSelected == true)
            {
                stateNodeDeletionQueue.Enqueue(stateNode);

                foreach(string childStateNodeID in stateNode.childStateNodeIDList)
                {
                    StateNodeSO childStateNode = currentFSMGraph.GetStateNodeByID(childStateNodeID);

                    if (childStateNode != null)
                    {
                        childStateNode.RemoveParentStateNodeID(stateNode.id);
                    }      
                }

                foreach(string parentStateNodeID in stateNode.parentStateNodeIDList)
                {
                    StateNodeSO parentStateNode = currentFSMGraph.GetStateNodeByID(parentStateNodeID);

                    if(parentStateNode != null)
                    {
                        parentStateNode.RemoveParentStateNodeID(stateNode.id);
                        parentStateNode.RemoveDecision(stateNode.id);
                    }
                        
                }

                foreach(var decision in stateNode.stateDecisionList)
                {
                    DestroyImmediate(decision, true);
                    AssetDatabase.SaveAssets();
                }
            }
        }

        while (stateNodeDeletionQueue.Count > 0)
        {
            StateNodeSO stateNodeToDelete = stateNodeDeletionQueue.Dequeue();
            currentFSMGraph.stateNodeDictionary.Remove(stateNodeToDelete.id);
            currentFSMGraph.stateNodeList.Remove(stateNodeToDelete);

            // 그래프에서 제거 후, 실제 파일도 제거해야함
            // UnityEngine의 DestroyImmediate 사용
            DestroyImmediate(stateNodeToDelete, true);
            AssetDatabase.SaveAssets();
        }
    }

    private void DeleteSelectedStateNodeLinks()
    {
        StateDecisionSO toDelete= null;

        foreach(StateNodeSO stateNode in currentFSMGraph.stateNodeList)
        {
            if(stateNode.isSelected && stateNode.childStateNodeIDList.Count > 0)
            {
                for(int i = stateNode.childStateNodeIDList.Count - 1; i >= 0; i--)
                {
                    StateNodeSO childStateNode = currentFSMGraph.GetStateNodeByID(stateNode.childStateNodeIDList[i]);

                    if(childStateNode != null && childStateNode.isSelected)
                    {
                        stateNode.RemoveChildStateNodeID(childStateNode.id);
                        childStateNode.RemoveParentStateNodeID(stateNode.id);

                        foreach(var decision in stateNode.stateDecisionList)
                        {
                            if(decision.nextState == childStateNode)
                            {
                                toDelete = decision;
                                break;
                            }
                        }

                        if(toDelete != null)
                        {
                            stateNode.stateDecisionList.Remove(toDelete);
                            DestroyImmediate(toDelete, true);
                            AssetDatabase.SaveAssets();

                            toDelete = null;
                        }
                    }
                }
            }
        }

        ClearAllSelectedStateNodes();
    }

    private void SelectAllStateNodes()
    {
        foreach(StateNodeSO stateNode in currentFSMGraph.stateNodeList)
        {
            stateNode.isSelected = true;
        }
        // 모든 노드들이 선택되었으므로
        // 선택된 노드 모양으로 이미지가 바뀌어야함
        GUI.changed = true;
    }

    private void CreateStateNode(object data)
    {
        Vector2 mousePosition = (Vector2)data;

        // 노드 만들고, 그래프의 노드 리스트에 추가, 노드 SO에 생성 위치정보와 그래프 정보 저장
        StateNodeSO stateNode = ScriptableObject.CreateInstance<StateNodeSO>();
        currentFSMGraph.stateNodeList.Add(stateNode);
        stateNode.Initialise(new Rect(mousePosition, new Vector2(nodeWidth, nodeHeight)), currentFSMGraph);

        //생성된 stateNode를 현재 FSM Graph SO 안에 넣어주기
        AssetDatabase.AddObjectToAsset(stateNode, currentFSMGraph);
        AssetDatabase.SaveAssets();

        // 그래프의 딕셔너리 초기화
        currentFSMGraph.OnValidate();
    }

    private void ProcessMouseUpEvent(Event currentEvent)
    {
        // 우클릭 up이고, 선을 그리고 있는 중 있는중이면
        // 마우스를 떼는 부분이 다른 노드이면 edge를 그리고
        // 빈 곳이면 그냥 선 지우기
        if(currentEvent.button == 1 && currentFSMGraph.stateNodeToDrawLineFrom != null)
        {
            StateNodeSO stateNode = IsMouseOverStateNode(currentEvent);

            if(stateNode != null && stateNode != currentFSMGraph.stateNodeToDrawLineFrom)
            {
                currentFSMGraph.stateNodeToDrawLineFrom.AddChildStateNodeID(stateNode.id);
                stateNode.AddParentStateNodeID(currentFSMGraph.stateNodeToDrawLineFrom.id);

                CreateDecisionEdge(currentFSMGraph.stateNodeToDrawLineFrom, stateNode);

            }

            ClearLineDrag();
        }
    }

    private void CreateDecisionEdge(StateNodeSO from, StateNodeSO to)
    {
        StateDecisionSO stateDecision = ScriptableObject.CreateInstance<StateDecisionSO>();
        stateDecision.SetState(from, to);
        from.AddDecision(stateDecision);

        AssetDatabase.AddObjectToAsset(stateDecision, from);
        AssetDatabase.SaveAssets();
    }

    private void ProcessMouseDragEvent(Event currentEvent)
    {
        if (currentEvent.button == 1)
        {
            ProcessRightMouseDragEvent(currentEvent);
        }
        else if (currentEvent.button == 0)
        {
            ProcessLeftMouseDragEvent(currentEvent.delta);
        }
    }

    private void ProcessLeftMouseDragEvent(Vector2 dragDelta)
    {
        graphDrag = dragDelta;

        for(int i=0; i<currentFSMGraph.stateNodeList.Count; i++)
        {
            currentFSMGraph.stateNodeList[i].DragNode(dragDelta);
        }

        GUI.changed = true;
    }

    private void ProcessRightMouseDragEvent(Event currentEvent)
    {

        if(currentFSMGraph.stateNodeToDrawLineFrom != null)
        {
            DragConnectingLine(currentEvent.delta);
            GUI.changed = true;
        }
    }

    private void DragConnectingLine(Vector2 delta)
    {
        currentFSMGraph.linePosition += delta;
    }
}
