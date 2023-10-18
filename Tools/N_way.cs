using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class N_way : MonoBehaviour
{
    public static List<Vector3Int> fourWay = new List<Vector3Int>()
    {
        new Vector3Int(0, 1, 0), //up
        new Vector3Int(1, 0, 0), //right
        new Vector3Int(0, -1, 0), //down
        new Vector3Int(-1, 0, 0), //left
    };

    public static List<Vector3Int> eightWay = new List<Vector3Int>()
    {
        new Vector3Int(0, 1, 0), //up
        new Vector3Int(1, 1, 0), // upright
        new Vector3Int(1, 0, 0), //right
        new Vector3Int(1, -1, 0), // downright
        new Vector3Int(0, -1, 0), //down
        new Vector3Int(-1, -1, 0), // downleft
        new Vector3Int(-1, 0, 0), //left
        new Vector3Int(-1, 1, 0) // upleft
    };

    public static Vector3Int GetRandomNWay(Way n)
    {
        List<Vector3Int> nWayList;

        switch (n)
        {
            case Way.FOUR:
                nWayList = fourWay;
                break;
            case Way.EIGHT:
                nWayList = eightWay;
                break;
            default:
                nWayList = fourWay;
                break;
        }

        return nWayList[Random.Range(0, nWayList.Count)];         
    }
}

public enum Way
{
    FOUR,
    EIGHT
}
