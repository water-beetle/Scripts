using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TorchPlacer : MonoBehaviour
{
    [SerializeField]
    private DungeonData dungeonData;
    [SerializeField]
    private GameObject torchPrefab;

    [Space(10), Header("Torch Settings")]
    [SerializeField]
    [Range(1, 10)]
    int minTorchNum;
    [SerializeField]
    [Range(1, 10)]
    int maxTorchNum;

    private void Awake()
    {
        dungeonData = GetComponentInParent<DungeonData>();
    }

    public void PlaceTorch()
    {
        foreach(var room in dungeonData.DungeonRooms)
        {
            PlaceTorchInEachRoom(room, torchPrefab);
        }
    }

    private void PlaceTorchInEachRoom(DungeonRoomData room, GameObject torchPrefab)
    {
        List<Vector3Int> availablePosition = GetAvailablePosition(room);
        int howMany = Random.Range(Math.Clamp(minTorchNum, 0, availablePosition.Count),
            Math.Clamp(maxTorchNum, 0, availablePosition.Count));

        for (int i = 0; i < howMany; i++)
        {
            GameObject createdTorchObject = Instantiate(torchPrefab, availablePosition[i] + new Vector3(0.5f, 0.5f, 0),
                Quaternion.identity, gameObject.transform);
        }
    }

    private List<Vector3Int> GetAvailablePosition(DungeonRoomData room)
    {
        List<Vector3Int> availablePositions = room.FloorPositions.ToList();
        availablePositions = availablePositions.OrderBy(x => Guid.NewGuid()).ToList();
        return availablePositions;
    }
}
