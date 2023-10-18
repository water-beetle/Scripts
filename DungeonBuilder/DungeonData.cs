using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonData : Singleton<DungeonData>
{
    public List<DungeonRoomData> DungeonRooms = new List<DungeonRoomData>();
    public HashSet<Vector3Int> totalFloorPos = new HashSet<Vector3Int>();
    public HashSet<Vector3Int> totalCorridorPos = new HashSet<Vector3Int>();
    public HashSet<Vector3Int> totalWallPos = new HashSet<Vector3Int>();
    public HashSet<Vector3Int> totalObstaclePos = new HashSet<Vector3Int>();
}

public class DungeonRoomData
{
    public BoundsInt roomRect { get; set; }

    public Vector3Int roomCenter
    {
        get
        {
            return new Vector3Int(Mathf.RoundToInt(roomRect.center.x), Mathf.RoundToInt(roomRect.center.y), Mathf.RoundToInt(roomRect.center.z));
        }
    }

    public HashSet<Vector3Int> FloorPositions { get; private set; } = new HashSet<Vector3Int>();


    public HashSet<Vector3> PropPositions { get; set; } = new HashSet<Vector3>();
    public HashSet<Vector3Int> TorchPositions { get; set; } = new();


    public List<GameObject> PropObjectReferences { get; set; } = new List<GameObject>();
    public List<GameObject> EnemiesInTheRoom { get; set; } = new List<GameObject>();

    public DungeonRoomData(BoundsInt roomRect)
    {
        this.roomRect = roomRect;

    }
}
