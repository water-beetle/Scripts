using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;
using Random = UnityEngine.Random;

public class PropPlacer : MonoBehaviour
{
    [SerializeField]
    private List<PropDataSO> props = new();
    [SerializeField]
    private DungeonData dungeonData;
    [SerializeField]
    private GameObject basePropPrefab;
    [SerializeField]
    private GameObject propsParentObject;

    private void Awake()
    {
        dungeonData = GetComponentInParent<DungeonData>();
    }

    public void PlaceProps()
    {
        foreach(var prop in props)
        {
            
            PlacePropInEachRooms(prop, dungeonData.DungeonRooms);
        }
    }

    private PropPlacementSetting GetPropPlacementSetting(PropDataSO prop)
    {
        PropPlacementSetting setting = new PropPlacementSetting();
        setting.howMany = Random.Range(prop.minPropNum, prop.maxPropNum);
        setting.howManyWidth = Random.Range(1, setting.howMany);
        setting.howManyHeight = setting.howMany - setting.howManyWidth + 1;
        setting.needWidthSize = Mathf.CeilToInt(prop.size.x * setting.howManyWidth);
        setting.needHeightSize = Mathf.CeilToInt(prop.size.y * setting.howManyHeight);

        return setting;
    }

    private void PlacePropInEachRooms(PropDataSO prop, List<DungeonRoomData> dungeonRooms)
    {
        foreach(var room in dungeonRooms)
        {
            for(int i=0; i<Random.Range(1, 3); i++)
            {
                PropPlacementSetting propPlacementSetting = GetPropPlacementSetting(prop);
                Vector3Int availablePropPosition = GetAvailablePropPositions(propPlacementSetting.needWidthSize, propPlacementSetting.needHeightSize, room);
                PlacePropInAvailablePositions(prop, propPlacementSetting, availablePropPosition, room);
            }          
        }
        
    }

    private void PlacePropInAvailablePositions(PropDataSO prop, PropPlacementSetting propPlacementSetting, Vector3Int availablePropPosition, DungeonRoomData room)
    {
        
        GameObject createdProp;
        int propCount = 0;

        for (int i=0; i<propPlacementSetting.howManyWidth; i++)
        {
            for(int j=0; j<propPlacementSetting.howManyHeight; j++)
            {
         
                if (propCount > propPlacementSetting.howMany)
                    return;

                Vector3 propPosition = availablePropPosition + new Vector3(i * prop.size.x, j * prop.size.y, 0);
                createdProp = createPropObject(prop, propPosition);

                room.PropObjectReferences.Add(createdProp);
                room.PropPositions.Add(propPosition);
                dungeonData.totalObstaclePos.Add(Vector3Int.CeilToInt(propPosition));

                propCount += 1;

            }
        }


    }

    private GameObject createPropObject(PropDataSO prop, Vector3 propPosition)
    {
        GameObject createdProp = Instantiate(basePropPrefab, propPosition, Quaternion.identity, gameObject.transform);
        createdProp.GetComponent<BoxCollider2D>().size = prop.size;
        createdProp.GetComponent<BoxCollider2D>().offset = new Vector3(0.5f, 0.5f, 0);
        createdProp.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = prop.sprite;
        createdProp.transform.GetChild(0).localPosition = new Vector3(0.5f, 0.5f, 0);

        return createdProp;
    }

    private Vector3Int GetAvailablePropPositions(int needWidthSize, int needHeightSize, DungeonRoomData roomData)
    {
        List<Vector3Int> avaiablePositions = new();

        foreach(var pos in roomData.FloorPositions)
        {
            bool isAvailable = true;
            for (int i = 0; i < needWidthSize; i++)
            {
                for (int j = 0; j < needHeightSize; j++)
                {
                    Vector3Int nextPos = pos;
                    nextPos.x += i; nextPos.y += j;
                    if(!roomData.FloorPositions.Contains(nextPos))
                        isAvailable = false;
                }
            }
            if (isAvailable)
                avaiablePositions.Add(pos);   
        }

        int ind = Random.Range(0, avaiablePositions.Count);
        for (int i = 0; i < needWidthSize; i++)
        {
            for (int j = 0; j < needHeightSize; j++)
            {
                Vector3Int nextPos = avaiablePositions[ind] + new Vector3Int(i, j);
                roomData.FloorPositions.Remove(nextPos);
            }
        }

        // 에러처리 - 배치할 수 있는 공간이 없을 경우
        if (avaiablePositions.Count == 0)
        {
            Debug.Log("Cant' Place Props!");
            return new Vector3Int(0, 0, 0);
        }

        return avaiablePositions[ind];
    }

public struct PropPlacementSetting{

    public int howMany;
    public int howManyWidth;
    public int howManyHeight;
    public int needWidthSize;
    public int needHeightSize;
}
}
