using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>マップのデータを保存するクラス</summary>
[Serializable]
public class MapData
{
    private int _width, _height;
        
    /// <summary>マップデータの配列、2次元配列だとSerialize不可能なため1次元配列で保存</summary>
    private TileType[] _gridMapData;
        
    public int Width => _width;
    public int Height => _height;
    
    public TileType[] GridMapData => _gridMapData;
    
    public void InitData(int width, int height)
    {
        _gridMapData = new TileType[width * height];
        _width = width;
        _height = height;
    }

    public void SetTileType(int tilePosX, int tilePosY, TileType tileType)
    {
        _gridMapData[tilePosX + _width * tilePosY] = tileType;
    }
}

/// <summary> マップの管理を行うクラス </summary>
public class MapGenerater
{
    /// <summary>分割されたエリアのデータを保存するクラス</summary>
    #region AreaDataClass
    [Serializable]
    public class AreaData
    {
        private Vector2Int _areaTopPos;
        private Vector2Int _areaBottomPos;

        public Vector2Int AreaTopPos => _areaTopPos;
        public Vector2Int AreaBottomPos => _areaBottomPos;
    
        public void SetAreaPos(Vector2Int areaTopPos, Vector2Int areaBottomPos)
        {
            _areaTopPos = areaTopPos;
            _areaBottomPos = areaBottomPos;
        }
    }
    #endregion

    /// <summary> 最低限のエリア内の部屋の外の余白スペース </summary>
    private int _areaUnwalkableSpace = 2;
    
    /// <summary> Mapのデータクラス </summary>
    private MapData _mapData = null;

    /// <summary>各エリアのデータを保存するリスト</summary>
    private List<AreaData> _areaDataList;
    
    /// <summary>部屋のデータを取得し保存するリスト</summary>
    private RoomData[] _roomDataList;
    
    /// <summary> 部屋の最小サイズ </summary>
    private RoomData _minRoomData = null;
    
    /// <summary> マップのエリアを作る際外側2マスを必ず空ける </summary>
    private const int _mapBorderSpace = 2;
    
    ///<summary> エリアの分割に必要なエリアの長さ </summary>
    private int _divideMinAreaSizeX,_divideMinAreaSizeY;
    
	/// <summary> mapデータのプロパティ </summary>
  	public MapData Mapdata => _mapData;	

    private static MapGenerater _instance = default;
    
    public static MapGenerater Instance
    {
        get
        {
            if (_instance == null) { Init(); }

            return _instance;
        }
    }

    public static void Init() => _instance = new();
    
    /// <summary> エリアの初期化 </summary>
    /// <param name="width">マップの横の長さ</param>
    /// <param name="height">マップの縦の長さ</param>
    /// <param name="divideNum">マップの分割回数</param>
    /// <param name="roomDataPath">部屋の保存先のパス</param>
    public async void InitMap(int width, int height, int divideNum, string roomDataPath)
    {
        _mapData = new ();
        _mapData.InitData(width, height);
        _areaDataList = new List<AreaData>();
        _roomDataList = await LoadRoomAssets(roomDataPath);
        _minRoomData = GetMinRoom();
        
        //最初のエリアを作る
        AreaData firstArea = new AreaData();
        
        //エリアの分割に必要な
        int　mustDivideSpace = _areaUnwalkableSpace * 4 + 1;
        _divideMinAreaSizeX = _minRoomData.Width * 2 + mustDivideSpace;
        _divideMinAreaSizeY = _minRoomData.Height * 2 + mustDivideSpace;
        
        //Mapの外側2マスをあけてエリアを分割していく
        Vector2Int firstAreaTopPos = new Vector2Int(_mapBorderSpace,_mapBorderSpace);
        Vector2Int firstAreaBottomPos = new Vector2Int(width - _mapBorderSpace, height - _mapBorderSpace);
        
        _areaDataList.Add(firstArea);
        firstArea.SetAreaPos(firstAreaTopPos, firstAreaBottomPos);
        
        //マップの分裂処理
        if(divideNum > 0) AreaMaking(divideNum, firstArea);

        // foreach (AreaData areaData in _areaDataList)
        // {
        //     BuildRoom(areaData);
        // }
    }
    
    #region AreaMaking
    ///<summary> 1つのマップを複数のエリアに分裂させる </summary>
    /// <param name="divideNum"> エリアの分割回数 </param>
    /// <param name="firstArea"> 分割する最初のおおもとのエリア </param>
    private void AreaMaking(int divideNum, AreaData firstArea)
    {
        AreaData widestArea = null;//Map内に存在する最も大きいエリアのデータ
        
        for (int i = 0; i < divideNum; i++)
        {
            //最も大きいエリアを検索
            widestArea = SearchWidestArea();

            if (TryDivideArea(widestArea))
            {
                //一番大きいエリアの分割
                AreaDivide(widestArea);
                
            }
            else
            {
                break;
            }
        }
    }

	/// <summary>1つのエリアを2つに分割する</summary>
    /// <param name="divideArea">分割するエリア</param>
    private void AreaDivide(AreaData divideArea)
    {
        int randomDividePos;
        AreaData newArea = new AreaData();
        
        //エリアの長さ
        int areaLengthX = divideArea.AreaBottomPos.x - divideArea.AreaTopPos.x;
        int areaLengthY = divideArea.AreaBottomPos.y - divideArea.AreaTopPos.y;
        
        int buildAreaSpace = _areaUnwalkableSpace * 2;

        //エリアが横に大きければ縦に分割
        if (areaLengthX > areaLengthY)
        {
            randomDividePos = 
                UnityEngine.Random.Range(divideArea.AreaTopPos.x + _minRoomData.Width + buildAreaSpace,
                    divideArea.AreaBottomPos.x - _minRoomData.Width - buildAreaSpace);
            
            divideArea.SetAreaPos(new Vector2Int(divideArea.AreaTopPos.x, divideArea.AreaTopPos.y),
                new Vector2Int(randomDividePos - 1, divideArea.AreaBottomPos.y));
            
            newArea.SetAreaPos(new Vector2Int(randomDividePos + 1, divideArea.AreaTopPos.y),
                new Vector2Int(divideArea.AreaBottomPos.x, divideArea.AreaBottomPos.y));
            
            //分割線の部分を埋める
            SetMapTiles(TileType.UnWalkable, randomDividePos, randomDividePos, divideArea.AreaTopPos.y, divideArea.AreaBottomPos.y);
        }
        //エリアが縦に大きければ横に分割
        else
        {
            randomDividePos = 
                UnityEngine.Random.Range(divideArea.AreaTopPos.y + _minRoomData.Height + buildAreaSpace,
                    divideArea.AreaBottomPos.y - _minRoomData.Height - buildAreaSpace);
            
            divideArea.SetAreaPos(new Vector2Int(divideArea.AreaTopPos.x, divideArea.AreaTopPos.y),
                new Vector2Int(divideArea.AreaBottomPos.x, randomDividePos - 1));
            
            newArea.SetAreaPos(new Vector2Int(divideArea.AreaTopPos.x, randomDividePos + 1),
                new Vector2Int(divideArea.AreaBottomPos.x, divideArea.AreaBottomPos.y));
            
            //分割線の部分を埋める
            SetMapTiles(TileType.UnWalkable, divideArea.AreaTopPos.x, divideArea.AreaBottomPos.x, randomDividePos, randomDividePos);
        }
        
        _areaDataList.Add(newArea);
    }

    /// <summary> 一番大きいエリアをリスト内から探す </summary>
    /// <returns></returns>
    private AreaData SearchWidestArea()
    {
        AreaData widestArea = null;   
        foreach (AreaData areaData in _areaDataList)
        {
            if(widestArea == null)
                widestArea = areaData;
            else if (areaData.AreaBottomPos.x * areaData.AreaBottomPos.y 
                     > widestArea.AreaBottomPos.x * widestArea.AreaBottomPos.y)
                widestArea = areaData;
        }
        
        return widestArea;
    }

    /// <summary> エリアの分割が可能かどうかの判定 </summary>
    /// <param name="divideArea"> 分割したいエリア </param>
    /// <returns> 分割可能ならTrue </returns>
    private bool TryDivideArea(AreaData divideArea)
    {
        int areaLengthX = divideArea.AreaBottomPos.x - divideArea.AreaTopPos.x;
        int areaLengthY = divideArea.AreaBottomPos.y - divideArea.AreaTopPos.y;

        if (areaLengthX > areaLengthY)
        {
            if (areaLengthX < _divideMinAreaSizeX || areaLengthY < _minRoomData.Height + _areaUnwalkableSpace * 2)
            {
                Debug.Log("分割限界");
                return false;
            }
        }
        else if (areaLengthX < areaLengthY)
        {
            if (areaLengthY < _divideMinAreaSizeY && areaLengthY < _minRoomData.Height + _areaUnwalkableSpace * 2)
            {
                Debug.Log("分割限界");
                return false;
            }
        }

        return true;
    }
    #endregion
    
    #region BuildRoom
    /// <summary> 指定エリア内に部屋を生成 </summary>
    private void BuildRoom(AreaData areaData)
    {
        List<RoomData> canBuildRooms = new List<RoomData>();//Area内に生成可能な部屋
        int generateSpaceX,generateSpaceY;
        foreach (RoomData roomData in _roomDataList)
        {
            generateSpaceX = areaData.AreaBottomPos.x - areaData.AreaTopPos.x - _areaUnwalkableSpace;
            generateSpaceY = areaData.AreaBottomPos.y - areaData.AreaTopPos.y - _areaUnwalkableSpace;
            if (roomData.Width < generateSpaceX &&
                roomData.Height < generateSpaceY)
            {
                canBuildRooms.Add(roomData);
            }
        }

        int randomRoomNum = UnityEngine.Random.Range(0, canBuildRooms.Count);
        RoomData generateRoom = canBuildRooms[randomRoomNum];

        int firstGenerateTilePosX = UnityEngine.Random.Range(areaData.AreaTopPos.x - _areaUnwalkableSpace, areaData.AreaBottomPos.x - generateRoom.Width - _areaUnwalkableSpace);
        int firstGenerateTilePosY = UnityEngine.Random.Range(areaData.AreaTopPos.y - _areaUnwalkableSpace, areaData.AreaBottomPos.y - generateRoom.Height - _areaUnwalkableSpace);
        
        for (int y = 0; y < generateRoom.Height; y++)
        {
            for (int x = 0; x < generateRoom.Width; x++)
            {
                _mapData.SetTileType(firstGenerateTilePosX + x,firstGenerateTilePosY + y, generateRoom.GridRoomData[x + generateRoom.Width * y]);
            }
        }
    }
    
    /// <summary>部屋のデータを取得</summary>
    /// <param name="roomDataPath">取得したいデータのラベル名</param>
    private async UniTask<RoomData[]> LoadRoomAssets(string roomDataPath)
    {
        RoomData[] roomDataList = Resources.LoadAll<RoomData>("RoomData/" + roomDataPath);
        Debug.Log(roomDataList.Length);
        return await UniTask.Run(() => roomDataList);
    }
    #endregion
    /// <summary>一定の範囲のタイルを埋める<summary>
    private void SetMapTiles(TileType putTile, int topPosX, int bottomPosX, int topPosY, int bottomPosY)
    {
        for (int y = topPosY; y <= bottomPosY; y++)
        {
            for (int x = topPosX; x <= bottomPosX; x++)
            {
                _mapData.SetTileType(x, y, putTile);
            }
        }
    }

    /// <summary> 取得した部屋の最小サイズを検索 </summary>
    private RoomData GetMinRoom()
    {
        RoomData minRoomData = null;
        
        foreach (RoomData roomData in _roomDataList)
        {
            if (minRoomData == null)
                minRoomData = roomData;
            else if (roomData.Width * roomData.Height < minRoomData.Width * minRoomData.Height)
                minRoomData = roomData;
        }
        
        return minRoomData;
    }
}

[System.Serializable]
public enum TileType
{
    Empty = 0,
    Walkable = 1,
    UnWalkable = 2,
}