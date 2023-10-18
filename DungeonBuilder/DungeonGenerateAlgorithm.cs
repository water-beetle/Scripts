using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public static class DungeonGenerateAlgorithm
{
    public static List<BoundsInt> BinarySpacePartitioning(int minWidth, int minHeight, BoundsInt dungeonRect)
    {
        Queue<BoundsInt> rectQueue = new Queue<BoundsInt>();
        List<BoundsInt> roomRects = new List<BoundsInt>();
        BoundsInt currentRect;

        rectQueue.Enqueue(dungeonRect);

        while(rectQueue.Count > 0)
        {
            currentRect = rectQueue.Dequeue();

            // 분할을 위한 적절한 길이 선택
            int divideLength = currentRect.size.x > currentRect.size.y ? currentRect.size.x : currentRect.size.y;
            divideLength = Mathf.RoundToInt(Random.Range(0.4f, 0.7f) * divideLength);

            // 방의 최소사이즈 크면 분할
            if(currentRect.size.x > minWidth && currentRect.size.y >= minHeight)
            {
                if(currentRect.size.x >= currentRect.size.y && currentRect.size.x > minWidth * 2)
                {
                    BoundsInt divideRect1 = new BoundsInt(currentRect.min, new Vector3Int(divideLength, currentRect.size.y, currentRect.size.z));
                    BoundsInt divideRect2 = new BoundsInt(new Vector3Int(currentRect.min.x + divideLength, currentRect.min.y, currentRect.min.z),
                        new Vector3Int(currentRect.size.x - divideLength, currentRect.size.y, currentRect.size.z));
                    rectQueue.Enqueue(divideRect1);
                    rectQueue.Enqueue(divideRect2);
                }
                else if (currentRect.size.y > currentRect.size.x && currentRect.size.y > minHeight * 2)
                {
                    BoundsInt divideRect1 = new BoundsInt(currentRect.min, new Vector3Int(currentRect.size.x, divideLength, currentRect.size.z));
                    BoundsInt divideRect2 = new BoundsInt(new Vector3Int(currentRect.min.x, currentRect.min.y + divideLength, currentRect.min.z),
                        new Vector3Int(currentRect.size.x, currentRect.size.y - divideLength, currentRect.size.z));
                    rectQueue.Enqueue(divideRect1);
                    rectQueue.Enqueue(divideRect2);
                }
                else
                {
                    roomRects.Add(currentRect);
                }
            }
        }

        return roomRects;
    }

    public static HashSet<Vector3Int> RandomWalkIteration(DungeonSettingSO dungeonSetting, DungeonRoomData room)
    {
        HashSet<Vector3Int> totalFloorPositions = new HashSet<Vector3Int>();
        Vector3Int startPosition = room.roomCenter;

        for (int i = 0; i < dungeonSetting.iteration; i++)
        {           
            totalFloorPositions.UnionWith(RandomWalk(dungeonSetting, room, startPosition));
            startPosition = totalFloorPositions.ElementAt(Random.Range(0, totalFloorPositions.Count));
        }

        return totalFloorPositions;
    }

    public static HashSet<Vector3Int> RandomWalk(DungeonSettingSO dungeonSetting, DungeonRoomData dungeomRoomData, Vector3Int startPosition)
    {
        Vector3Int currentPosition = startPosition;
        HashSet<Vector3Int> floorPositions = new HashSet<Vector3Int>();

        for (int i = 0; i < dungeonSetting.length; i++)
        {
            if (dungeomRoomData.roomRect.xMin + dungeonSetting.offset < currentPosition.x && currentPosition.x < dungeomRoomData.roomRect.xMax - dungeonSetting.offset &&
                dungeomRoomData.roomRect.yMin + dungeonSetting.offset < currentPosition.y && currentPosition.y < dungeomRoomData.roomRect.yMax - dungeonSetting.offset)
            {
                floorPositions.Add(currentPosition);
            }
            else
            {
                currentPosition = floorPositions.ElementAt(Random.Range(0, floorPositions.Count));
            }
            currentPosition += N_way.GetRandomNWay(Way.FOUR);
        }
        return floorPositions;
    }

    public static List<Edge> GetShortestEdge(DungeonData dungeonData)
    {
        Kruskal kruskal = new Kruskal(dungeonData);
        List<Edge> edges = kruskal.GetShortestPath();

        return edges;
    }

    public class Kruskal
    {
        List<Edge> edges = new List<Edge>();
        List<Edge> edgesInMST = new List<Edge>();
        int roomNum;
        Dictionary<Vector3Int, Vector3Int> UFdict = new Dictionary<Vector3Int, Vector3Int>();

        public Kruskal(DungeonData dungeonData)
        {

            for (int i = 0; i < dungeonData.DungeonRooms.Count; i++)
            {
                for (int j = i + 1; j < dungeonData.DungeonRooms.Count; j++)
                {
                    edges.Add(new Edge(dungeonData.DungeonRooms[i], dungeonData.DungeonRooms[j]));
                }
            }

            foreach (var room in dungeonData.DungeonRooms)
            {
                UFdict[room.roomCenter] = room.roomCenter;
            }

            roomNum = dungeonData.DungeonRooms.Count;
            edges.Sort();
        }

        public List<Edge> GetShortestPath()
        {
            foreach(var edge in edges)
            {
                if (edgesInMST.Count == roomNum)
                    break;

                if(!IsConnected(edge.srouce.roomCenter, edge.dest.roomCenter))
                {
                    edgesInMST.Add(edge);
                    Union(edge.srouce.roomCenter, edge.dest.roomCenter);
                }
            }

            return edgesInMST;
        }

        public Vector3Int FindRoot(Vector3Int v)
        {
            while(v != UFdict[v])
                v = UFdict[v];
            return v;
        }

        public bool IsConnected(Vector3Int a, Vector3Int b)
        {
            return FindRoot(a) == FindRoot(b);
        }

        public void Union(Vector3Int a, Vector3Int b)
        {
            a = FindRoot(a);
            b = FindRoot(b);
            UFdict[a] = b;
        }
    }

    public static HashSet<Vector3Int> MakeWallPositions(HashSet<Vector3Int> floorPositions)
    {
        HashSet<Vector3Int> wallPositions = new HashSet<Vector3Int>();

        foreach(var position in floorPositions)
        {
            foreach(var direction in N_way.eightWay)
            {
                var neighbourPos = position + direction;
                if (!floorPositions.Contains(neighbourPos))
                {
                    wallPositions.Add(neighbourPos);
                }
            }
        }

        return wallPositions;
    }

    public static string MakeWallType(HashSet<Vector3Int> totalFloorPos, Vector3Int wallPos, Way way)
    {
        string type = "";
        List<Vector3Int> n_way;

        switch (way)
        {
            case Way.FOUR:
                n_way = N_way.fourWay;
                break;
            case Way.EIGHT:
                n_way = N_way.eightWay;
                break;
            default:
                n_way = N_way.fourWay;
                break;
        }

        foreach(var dir in n_way)
        {
            var checkPos = wallPos + dir;

            if (totalFloorPos.Contains(checkPos))
            {
                type += "1";
            }
            else
            {
                type += "0";
            }
        }
        return type;
    }
}




