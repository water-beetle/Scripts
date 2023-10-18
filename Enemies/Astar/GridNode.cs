using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNodes
{
    public int width;
    public int height;

    private Node[,] gridNode;

    public GridNodes(int size)
    {
        width = size;
        height = size;

        gridNode = new Node[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                gridNode[x, y] = new Node(new Vector3Int(x, y));
            }
        }
    }

    public Node GetGridNode(int xPosition, int yPosition)
    {
        if (xPosition < width && yPosition < height)
        {
            return gridNode[xPosition, yPosition];
        }
        else
        {
            Debug.Log("Requested grid node is out of range" + xPosition + ", " + yPosition);
            return null;
        }
    }
}
