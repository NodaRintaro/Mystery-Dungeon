using System;
using UnityEngine;

#region MapDataClass
[Serializable]
public class MapData
{
    private int _width, _height;
    
    [SerializeField] private TileType[] _gridMapData;

    public int Width => _width;
    public int Height => _height;

    public TileType[] GridMapData => _gridMapData;

    public void InitData(int width, int height)
    {
        _gridMapData = new TileType[width * height];
        _width = width;
        _height = height;
        Debug.Log("マップ全体の長さ" + GridMapData.Length);
    }

    public void SetTileType(int tilePosX, int tilePosY, TileType tileType)
    {
        _gridMapData[tilePosX + _width * tilePosY] = tileType;
    }

    public TileType GetTileType(int tilePosX, int tilePosY)
    {
        return _gridMapData[tilePosX + _width * tilePosY];
    }
}
#endregion