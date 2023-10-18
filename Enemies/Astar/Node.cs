using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable<Node>
{
    public Vector3Int gridPosition;
    public int gCost = 0; // distance from starting node
    public int hCost = 0; // estimated distance to destination
    public Node parentNode;

    public Node(Vector3Int gridPosition)
    {
        this.gridPosition = gridPosition;
        parentNode = null;
    }

    public int FCost
    {
        get { return gCost + hCost; }
    }


    // Node ���� ����
    // FCost�� ���� ���� ���� ������
    // FCost�� ���� �� GCost�� ���� ���� ������
    public int CompareTo(Node other)
    {
        int compare = FCost.CompareTo(other.FCost);

        if (compare == 0)
            compare = hCost.CompareTo(other.hCost);

        return compare;
    }
}
