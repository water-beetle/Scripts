using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DungeonSetting_", menuName = "ScriptableObjects/Dungeon/DungeonSetting")]
public class DungeonSettingSO : ScriptableObject
{
    [Space(10)]
    [Header("RandomWalk Parameters")]
    public int iteration;
    public int length;

    [Space(10)]
    [Header("BinaryPartitioning Parameters")]
    public int offset = 1;
    public int width, height;
    public int minWidth, minHeight;

    [Space(10), Header("Dungeon Setting")]
    [Range(1, 5)]
    public int corridorSize = 1;
    public int minRoomNums = 5;
    public int maxRoomNums = 10;
}
