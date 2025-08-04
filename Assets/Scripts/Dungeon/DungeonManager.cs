using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class DungeonManager : MonoBehaviour
{
    /// <summary> 生成するダンジョンの1マスの大きさ </summary>
    [SerializeField,Header("生成するダンジョンの1マスの大きさ")] private int _gridSize = 2;
    
    /// <summary> 生成したマップデータのクラス変数 </summary>
    [SerializeField,Header("生成したマップデータのクラス変数")] private static MapData _mapData = null;
    
    [Header("ダンジョンの大きさ")]
    /// <summary> 生成するダンジョンの横の大きさ </summary>
    [SerializeField] private int _dungeonLengthX = 100;
    /// <summary> 生成するダンジョンの縦の大きさ </summary>
    [SerializeField] private int _dungeonLengthY = 100;
    
    /// <summary> 生成したタイルを格納するGameObject </summary>
    [SerializeField] private GameObject _mapObject;
    
    /// <summary> 生成するダンジョンの部屋の個数 </summary>
    [SerializeField,Header("生成するダンジョンの最高分割回数")] private int _divideNum = 6;
    
    /// <summary> 生成する部屋の種類 </summary>
    [SerializeField,Header("生成する部屋の種類")] private string _generateRoomPath = "defaultRoom";
    
    /// <summary> Tileの保存先のPath </summary>
    [SerializeField,Header("Tileの保存先のPath")] private string _dungeonTypePath = "MapTile/Default";
    
    [SerializeField] private MapGenerater _mapGenerater;
    
    /// <summary> マップデータのプロパティ </summary>
    public static MapData MapData => _mapData;

    private void Awake()
    {
        _mapGenerater = MapGenerater.Instance;
    }

    private async void Start()
    {
        _mapObject = new GameObject("MapTiles");
        
        await DungeonGenerate();
    }

    public async UniTask DungeonGenerate()
    {
        _mapData = await _mapGenerater.InitMap(_dungeonLengthX, _dungeonLengthY, _divideNum, _generateRoomPath);
        
        await MapInstantiate(_mapData, _dungeonTypePath);
    }
    
    /// <summary> マップのデータを元にダンジョンを生成する処理 </summary>
    private async UniTask MapInstantiate(MapData mapData, string tilePath)
    {
        GameObject walkableTile = await TileResourceLoad(tilePath + "/Walkable");
        GameObject unWalkableTile = await TileResourceLoad(tilePath + "/UnWalkable");

        for (var tileCount = 0; tileCount < mapData.GridMapData.Length; tileCount++)
        {
            switch (mapData.GridMapData[tileCount])
            {
                case TileType.Walkable:
                    TileInstantiate(tileCount, _dungeonLengthX, walkableTile);
                    break;
                case TileType.UnWalkable:
                    TileInstantiate(tileCount, _dungeonLengthX, unWalkableTile);
                    break;
                default:
                    mapData.GridMapData[tileCount] = TileType.UnWalkable;
                    //TileInstantiate(tileCount, _dungeonLengthX, unWalkableTile);
                    break;
            }
        }
    }
    
    /// <summary> マップのインゲームへの生成処理 </summary>
    /// <param name="arrPos"> 配列の要素番号 </param>
    /// <param name="width"> マップの横の長さ </param>
    /// <param name="tile"> 生成したいObject </param>
    private void TileInstantiate(int arrPos ,int width , GameObject tile)
    {
        int xPos = arrPos;
        int yPos = 0;
        //マップのデータは1次配列で表現されているので一度生成場所x,zの場所を割り出す
        while (xPos >= width)
        {
            xPos -= width;
            yPos++;
        }
        GameObject tileObj = Instantiate(tile, new Vector3(xPos * _gridSize, 0, yPos * _gridSize), Quaternion.identity);
        tileObj.transform.SetParent(_mapObject.transform);
    }

    /// <summary>
    /// Mapのオブジェクトをロードする
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    private async UniTask<GameObject> TileResourceLoad(string filePath)
    {
        return Resources.Load<GameObject>(filePath);
    }
}
