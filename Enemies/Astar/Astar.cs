using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Astar : MonoBehaviour
{
    [SerializeField]
    private DungeonData dungeonData;

    private void Awake()
    {
        dungeonData = FindAnyObjectByType<DungeonData>();
    }

    public Stack<Vector3Int> BuildPath(Vector3Int startGridPosition, Vector3Int targetGridPosition)
    {
        List<Node> openNodeList = new List<Node>();
        HashSet<Node> closedNodeHashSet = new HashSet<Node>();
        int pursueDistance = Settings.Astar.PursueDistance;
        int gridNodeSize = Settings.Astar.PursueDistance * 2 + 1;
        GridNodes gridNodes = new GridNodes(gridNodeSize);

        // 시작, 끝 좌표를 gridNode 좌표로 변환
        // (PursueDistance, PursueDistance)만큼 값을 빼주어서 시작 좌표를 gridNode 중심점으로 변환
        Vector3Int movePosition =  startGridPosition - new Vector3Int(pursueDistance, pursueDistance, 0);
        movePosition.z = 0;
        Vector3Int targetPosition = targetGridPosition - movePosition;
        Vector3Int sourcePosition = new Vector3Int(pursueDistance, pursueDistance, 0);


        if (!CheckIsPositionInGraph(targetPosition.x, targetPosition.y, gridNodes))
            return new Stack<Vector3Int>();

        Node startNode = gridNodes.GetGridNode(sourcePosition.x, sourcePosition.y);
        Node targetNode = gridNodes.GetGridNode(targetPosition.x, targetPosition.y);

        Node endPathNode = FindShortestPath(startNode, targetNode, gridNodes, openNodeList, closedNodeHashSet, movePosition);

        if (endPathNode != null)
        {
            return CreatePathStack(endPathNode, startGridPosition, movePosition);
        }

        return new Stack<Vector3Int>();
    }

    private Node FindShortestPath(Node startNode, Node targetNode, GridNodes gridNodes, List<Node> openNodeList, HashSet<Node> closedNodeHashSet, Vector3Int movePosition)
    {
        openNodeList.Add(startNode);

        while(openNodeList.Count > 0)
        {
            openNodeList.Sort();

            Node currentNode = openNodeList[0];
            openNodeList.RemoveAt(0);
            closedNodeHashSet.Add(currentNode);

            if (currentNode == targetNode)
                return currentNode;

            EvaluateCurrentNodeNeighbours(currentNode, targetNode, gridNodes, openNodeList, closedNodeHashSet, movePosition);
        }

        return null;
    }

    private void EvaluateCurrentNodeNeighbours(Node currentNode, Node targetNode, GridNodes gridNodes, List<Node> openNodeList, HashSet<Node> closedNodeHashSet, Vector3Int movePosition)
    {
        Vector3Int currentNodeGridPosition = currentNode.gridPosition;
        Node validNeighbourNode;

        for(int i=-1; i<=1; i++)
        {
            for(int j=-1; j<=1; j++)
            {
                if (i == 0 && j == 0)
                    continue;

                validNeighbourNode = GetValidNodeNeighbour(currentNodeGridPosition.x + i, currentNodeGridPosition.y + j, gridNodes, closedNodeHashSet, movePosition, currentNodeGridPosition);

                if(validNeighbourNode != null)
                {
                    int newCostToNeighbour;

                    newCostToNeighbour = currentNode.gCost + GetDistance(currentNode, validNeighbourNode);

                    bool isValidNeighbourNodeInOpenList = openNodeList.Contains(validNeighbourNode);

                    if (newCostToNeighbour < validNeighbourNode.gCost || !isValidNeighbourNodeInOpenList)
                    {
                        validNeighbourNode.gCost = newCostToNeighbour;
                        validNeighbourNode.hCost = GetDistance(validNeighbourNode, targetNode);
                        validNeighbourNode.parentNode = currentNode;

                        if (!isValidNeighbourNodeInOpenList)
                            openNodeList.Add(validNeighbourNode);
                    }
                }

            }
        }
        
    }

    private int GetDistance(Node nodeA, Node nodeB)
    {
        int dstX = Mathf.Abs(nodeA.gridPosition.x - nodeB.gridPosition.x);
        int dstY = Mathf.Abs(nodeA.gridPosition.y - nodeB.gridPosition.y);

        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
    }

    private Node GetValidNodeNeighbour(int neighbourNodeXPosition, int neighbourNodeYPosition, GridNodes gridNodes, HashSet<Node> closedNodeHashSet, Vector3Int movePosition, Vector3Int currentPos)
    {
        if (!CheckIsPositionInGraph(neighbourNodeXPosition, neighbourNodeYPosition, gridNodes))
            return null;

        Node neighbourNode = gridNodes.GetGridNode(neighbourNodeXPosition, neighbourNodeYPosition);


        if (closedNodeHashSet.Contains(neighbourNode))
        {
            return null;
        }
        else if (dungeonData.totalObstaclePos.Contains(neighbourNode.gridPosition + movePosition))
        {
            return null;
        }
        // 대각선으로 이동 시 벽 체크
        else if (dungeonData.totalObstaclePos.Contains(new Vector3Int(currentPos.x, neighbourNodeYPosition, currentPos.z) + movePosition) ||
            dungeonData.totalObstaclePos.Contains(new Vector3Int(neighbourNodeXPosition, currentPos.y, currentPos.z) + movePosition))
        {
            return null;
        }
        else
        {
            return neighbourNode;
        }
    }

    private bool CheckIsPositionInGraph(int xPos, int yPos, GridNodes gridNodes)
    {
        if (xPos >= gridNodes.width || xPos < 0 || yPos >= gridNodes.height || yPos < 0)
        {
            return false;
        }

        return true;
    }


    private Stack<Vector3Int> CreatePathStack(Node targetNode, Vector3Int startGridPosition, Vector3Int movePosition)
    {
        Stack<Vector3Int> movementPathStack = new Stack<Vector3Int>();

        Node nextNode = targetNode;
        while (nextNode != null)
        {
            Vector3Int pos = nextNode.gridPosition + movePosition;
            movementPathStack.Push(pos);
            nextNode = nextNode.parentNode;
        }
        return movementPathStack;
    }
}
