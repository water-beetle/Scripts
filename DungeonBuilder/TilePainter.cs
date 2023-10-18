using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TilePainter : MonoBehaviour
{
    public Tilemap floorTilemap, wallTilemap, wallTopTilemap, wallTopRightTilemap, AstarTilemap;

    [Space(10)]
    [Header("Floor Tiles")]
    public List<TileBase> floorTiles;
    private TileBase floorTile;
    [Space(10)]
    [Header("Wall Tiles")]
    public TileBase wallMid;
    public TileBase wallTopMid, wallTopRight, wallTopLeft; 
    [Space(10)]
    public TileBase wallCornerRight;
    public TileBase wallCornerTopRight, wallCornerBottomRight, wallCornerFrontRight, wallInnerCornerMidRight,
        wallCornerLeft, wallCornerTopLeft, wallCornerBottomLeft, wallCornerFrontLeft, wallInnerCornerMidLeft;
    [Space(10)]
    public TileBase wallSideFrontLeft;
    public TileBase wallSideMidLeft, wallSideTopLeft, wallSideFrontRight, wallSideMidRight, wallSideTopRight;

    [Space(10), Header("Test tiles")]
    public TileBase testTile;


    public void PaintFloorTiles(HashSet<Vector3Int> floorPositions)
    {
        foreach (var pos in floorPositions)
        {
            floorTile = PickRandomFloorTile();
            floorTilemap.SetTile(floorTilemap.WorldToCell(pos), floorTile);
        }
    }

    public void PaintFloorTiles(Vector3Int floorPositions)
    {
        floorTile = PickRandomFloorTile();
        floorTilemap.SetTile(floorTilemap.WorldToCell(floorPositions), floorTile);

    }

    private TileBase PickRandomFloorTile()
    {
        if(Random.Range(0, 100) > 95)
        {
            return floorTiles[Random.Range(1, floorTiles.Count)];
        }
        return floorTiles[0];
    }

    public void PaintWallTiles(string type, Vector3Int wallPos) 
    {
        int typeInt = Convert.ToInt32(type, 2);
        TileBase tile = null;
        TileBase tile2 = null;
        TileBase tile3 = null;
        Tilemap tilemap = wallTilemap;
        Tilemap topTilemap = wallTopTilemap;

        HashSet<Vector3Int> newFloorTiles = new HashSet<Vector3Int>();

        if (WallByteTypes.wallSideMidLeft.Contains(typeInt))
        {
            tile = wallSideMidLeft;
        }
        else if (WallByteTypes.wallSideMidRight.Contains(typeInt))
        {
            tile = wallSideMidRight;
        }
        else if (WallByteTypes.wallTop.Contains(typeInt))
        {
            tile = wallMid;
            tile2 = wallTopMid;
        }
        else if (WallByteTypes.wallBottom.Contains(typeInt))
        {
            tile = wallMid;
            tile2 = wallTopMid;
        }
        else if (WallByteTypes.wallCornerTopLeft.Contains(typeInt))
        {
            tile = wallCornerLeft;
            tile2 = wallCornerTopLeft;
        }
        else if (WallByteTypes.wallCornerTopRight.Contains(typeInt))
        {
            tile = wallCornerRight;
            tile2 = wallCornerTopRight;
            topTilemap = wallTopRightTilemap;
        }
        else if (WallByteTypes.wallCornerBottomRight.Contains(typeInt))
        {
            tile = wallCornerFrontRight;
            tile2 = wallCornerBottomRight;
            tile3 = wallSideTopLeft;
            topTilemap = wallTopRightTilemap;
        }
        else if (WallByteTypes.wallCornerBottomLeft.Contains(typeInt))
        {
            tile = wallCornerFrontLeft;
            tile2 = wallCornerBottomLeft;
            tile3 = wallSideTopRight;
        }
        else if (WallByteTypes.wallSideFrontLeft.Contains(typeInt))
        {
            tile = wallSideFrontLeft;
        }
        else if (WallByteTypes.wallSideFrontRight.Contains(typeInt))
        {
            tile = wallSideFrontRight;
        }
        else if (WallByteTypes.wallSideTopRight.Contains(typeInt))
        {
            tile = wallSideMidRight;
            tile2 = wallSideTopRight;
            topTilemap = wallTopRightTilemap;
        }
        else if (WallByteTypes.wallSideTopLeft.Contains(typeInt))
        {
            tile = wallSideMidLeft;
            tile2 = wallSideTopLeft;
        }
        else if (WallByteTypes.wallDownBlank.Contains(typeInt))
        {
            tile = wallCornerLeft;
            tile2 = wallCornerTopLeft;
            DrawSingleTile(wallPos, wallTopTilemap, wallSideMidLeft);
        }
        else if (WallByteTypes.wallUpBlank.Contains(typeInt))
        {
            tile = wallCornerFrontLeft;
            tile2 = wallCornerBottomLeft;
            DrawSingleTile(wallPos + new Vector3Int(0, 1, 0), wallTopRightTilemap, wallSideMidLeft);
        }
        else if (WallByteTypes.wallLeftBlank.Contains(typeInt))
        {
            tile = wallCornerRight;
            tile2 = wallCornerTopRight;
            //DrawSingleTile(basicWallPos, wallTopTilemap, wallTopMid);
        }
        else if (WallByteTypes.wallRightBlank.Contains(typeInt))
        {
            tile = wallCornerLeft;
            tile2 = wallCornerTopLeft;
            //DrawSingleTile(basicWallPos, wallTopTilemap, wallTopMid);
        }
        else if (WallByteTypes.wallUpDown.Contains(typeInt))
        {
            tile = wallMid;
            tile2 = wallTopMid;
            DrawSingleTile(wallPos, wallTopTilemap, wallTopMid);
        }
        else if (WallByteTypes.wallLeftRight.Contains(typeInt))
        {
            tile = wallMid;
            DrawSingleTile(wallPos, wallTopTilemap, wallSideMidLeft);
            DrawSingleTile(wallPos, wallTopRightTilemap, wallSideMidRight);
        }

        DrawSingleTile(wallPos, wallTilemap, tile);
        if (tile2 != null)
            DrawSingleTile(wallPos + new Vector3Int(0, 1, 0), topTilemap, tile2);
        if (tile3 != null)
            DrawSingleTile(wallPos + new Vector3Int(0, 2, 0), topTilemap, tile3);

    }

    public void DrawSingleTile(Vector3Int position, Tilemap tilemap, TileBase tile)
    {
        tilemap.SetTile(tilemap.WorldToCell(position), tile);
    }

    public void ClearTilemap(Tilemap tilemap)
    {
        tilemap.ClearAllTiles();
    }
}



