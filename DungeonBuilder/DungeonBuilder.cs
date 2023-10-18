using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;
using Debug = UnityEngine.Debug;

public class DungeonBuilder : MonoBehaviour
{
    [SerializeField]
    private DungeonData dungeonData;
    [SerializeField]
    private TilePainter tilePainter;
    [SerializeField]
    private DungeonBuildAlgorithm buildAlgorithm;
    [SerializeField]
    private PropPlacer propPlacer;
    [SerializeField]
    private TorchPlacer torchPlacer;

    [Space(10), Header("Dungeon Level Settings")]
    [SerializeField]
    private DungeonSettingSO dungeonLevelSettings;

    public void Awake()
    {
        tilePainter = GetComponent<TilePainter>();
        buildAlgorithm = GetComponent<DungeonBuildAlgorithm>();
        propPlacer = GetComponentInChildren<PropPlacer>();
        torchPlacer = GetComponentInChildren<TorchPlacer>();
    }

    private void Start()
    {
        dungeonData = DungeonData.Instance;
        BuildDungeon();
        PlaceProps();
        PlaceTorchs();
    }

    private void PlaceTorchs()
    {
        torchPlacer.PlaceTorch();
    }

    private void PlaceProps()
    {
        propPlacer.PlaceProps();
    }

    public void BuildDungeon()
    {
        GenerateRooms();
        GenerateCorridors();
        GenerateWalls();

        PaintDungeonTiles();
    }

    private void GenerateRooms()
    {
        GenerateRoomRects();
        GenerateFloors();
    }

    private void GenerateRoomRects()
    {
        BoundsInt dungeonRect = new BoundsInt(new Vector3Int(0, 0, 0), new Vector3Int(dungeonLevelSettings.width, dungeonLevelSettings.height, 0));
        List<BoundsInt> roomRects = buildAlgorithm.BinarySpacePartitioning(dungeonLevelSettings.minWidth, dungeonLevelSettings.minHeight, dungeonRect);

        for(int i=0; i<20; i++)
        {
            if(roomRects.Count > dungeonLevelSettings.maxRoomNums)
            {
                roomRects = roomRects.GetRange(0, dungeonLevelSettings.maxRoomNums);
                break;
            }
            if (roomRects.Count > dungeonLevelSettings.minRoomNums)
                break;
        }

        if (roomRects.Count < dungeonLevelSettings.minRoomNums)
            Debug.Log("Error : Need bigger dungeon Size, Can't make more rooms!");

        foreach (var roomRect in roomRects)
        {
            DungeonRoomData roomData = new DungeonRoomData(roomRect);
            dungeonData.DungeonRooms.Add(roomData);
        }
    }

    private void GenerateFloors()
    {
        HashSet<Vector3Int> roomFloorsPositions;

        foreach (var room in dungeonData.DungeonRooms)
        {
            roomFloorsPositions = buildAlgorithm.RandomWalkIteration(dungeonLevelSettings, room);
            room.FloorPositions.UnionWith(roomFloorsPositions);
            dungeonData.totalFloorPos.UnionWith(roomFloorsPositions);
        }
    }

    private void GenerateCorridors()
    {
        List<Edge> edges = buildAlgorithm.GetShortestEdge(dungeonData);
        GenerateEdgesToCorridors(dungeonData, edges);
        //ExtendCorridors(dungeonData);
    }

    private void GenerateEdgesToCorridors(DungeonData dungeonData, List<Edge> edges)
    {
        Vector3Int source, dest;

        foreach (var edge in edges)
        {
            source = edge.srouce.roomCenter;
            dest = edge.dest.roomCenter;

            while (source.x != dest.x)
            {
                if (source.x > dest.x)
                    source.x -= 1;
                else
                    source.x += 1;

                if (!edge.srouce.FloorPositions.Contains(source) && !edge.srouce.FloorPositions.Contains(dest))
                {
                    
                    dungeonData.totalCorridorPos.Add(source);
                    for(int i = 2; i <= dungeonLevelSettings.corridorSize; i++)
                    {
                        Vector3Int wideCorridorPos = source;
                        int move_pos = i / 2;
                        if (i % 2 == 1)
                            move_pos *= -1;
                        wideCorridorPos.y += move_pos;
                        dungeonData.totalCorridorPos.Add(wideCorridorPos);
                    }
                }
            }

            while (source.y != dest.y)
            {
                if (source.y > dest.y)
                    source.y -= 1;
                else
                    source.y += 1;

                dungeonData.totalCorridorPos.Add(source);
                for (int i = 2; i <= dungeonLevelSettings.corridorSize; i++)
                {
                    Vector3Int wideCorridorPos = source;
                    int move_pos = i / 2;
                    if (i % 2 == 1)
                        move_pos *= -1;
                    wideCorridorPos.x += move_pos;
                    dungeonData.totalCorridorPos.Add(wideCorridorPos);
                }
            }
        }

        dungeonData.totalFloorPos.UnionWith(dungeonData.totalCorridorPos);
    }

    private void ExtendCorridors(DungeonData dungeonData)
    {
        throw new NotImplementedException();
    }

    private void GenerateWalls()
    {
        CreateWallPositions();
    }

    private void CreateWallPositions()
    {
        HashSet<Vector3Int> wallPositions;
        wallPositions = buildAlgorithm.MakeWallPositions(dungeonData.totalFloorPos);
        dungeonData.totalWallPos.UnionWith(wallPositions);
        dungeonData.totalObstaclePos.UnionWith(wallPositions);
    }


    private void PaintDungeonTiles()
    {
        PaintFloorTiles();
        PaintWallTiles();
    }
    private void PaintFloorTiles()
    {
        tilePainter.PaintFloorTiles(dungeonData.totalFloorPos);
        tilePainter.PaintFloorTiles(dungeonData.totalCorridorPos);
    }

    private void PaintWallTiles()
    {
        foreach(var wallPos in dungeonData.totalWallPos)
        {
            String eightwayWallType = buildAlgorithm.MakeWallType(dungeonData.totalFloorPos, wallPos, Way.EIGHT);

            // 동서남북이 바닥타일(구멍)이면 그냥 바닥타일로 취급
            if ((Convert.ToInt32(eightwayWallType, 2) & Convert.ToInt32("10101010", 2)) == Convert.ToInt32("10101010", 2))
                tilePainter.PaintFloorTiles(wallPos);
            else
                tilePainter.PaintWallTiles(eightwayWallType, wallPos);
        }
    }

}



