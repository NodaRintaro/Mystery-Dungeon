using System;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    /// <summary> 生成するダンジョンの1マスの大きさ </summary>
    [SerializeField] private int _gridSize = 10;
    
    [Header("ダンジョンの大きさ")]
    /// <summary> 生成するダンジョンの横の大きさ </summary>
    [SerializeField] private int _dungeonLengthX = 100;
    /// <summary> 生成するダンジョンの縦の大きさ </summary>
    [SerializeField] private int _dungeonLengthY = 100;
    
    /// <summary> 生成するダンジョンの部屋の個数 </summary>
    [SerializeField,Header("生成するダンジョンの部屋の個数")] private int _roomsNum = 6;
    
    /// <summary> 生成する部屋の種類 </summary>
    [SerializeField,Header("生成する部屋の種類")] private string _generateRoomPath = "defaultRoom";
    
    /// <summary> Tileの保存先のPath </summary>
    [SerializeField,Header("Tileの保存先のPath")] private string _dungeonTypePath = "MapTile/Default";
    
    private MapGenerater _mapGenerater;

    private void Awake()
    {
        _mapGenerater = MapGenerater.Instance;
    }

    private void Start()
    {
        _mapGenerater.InitMap(_dungeonLengthX, _dungeonLengthY, _roomsNum, _generateRoomPath);
        
        MapGenerate(_mapGenerater.Mapdata, _dungeonTypePath);
    }
    
    /// <summary>  </summary>
    private void MapGenerate(MapData mapData, string tilePath)
    {
        TileType tile;
        for (var tileCount = 0; tileCount < mapData.GridMapData.Length; tileCount++)
        {
            tile = mapData.GridMapData[tileCount];
            {
                switch (tile)
                {
                    case TileType.Walkable:
                        TileInstantiate(tileCount, _gridSize, Resources.Load<GameObject>(tilePath + "/Walkable"));
                        break;
                    case TileType.UnWalkable:
                        TileInstantiate(tileCount, _gridSize, Resources.Load<GameObject>(tilePath + "/UnWalkable"));
                        break;
                    default:
                        Debug.Log("タイルが設定されていません");
                        break;
                }
            }
        }
    }
    
    
    private void TileInstantiate(int arrNum ,int width , GameObject tile)
    {
        int xPos = arrNum;
        int yPos = 0;
        while (xPos >= width)
        {
            xPos = -width;
            yPos++;
        }
        Instantiate(tile, new Vector3(xPos + _gridSize, 0, yPos + _gridSize), Quaternion.identity);
    }
}
