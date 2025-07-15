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
    
    public async UniTask InitData(int width, int height)
    {
        _gridMapData = new TileType[width * height];
        _width = width;
        _height = height;
        Debug.Log("配列の長さ" + GridMapData.Length);
    }

    public void SetTileType(int tilePosX, int tilePosY, TileType tileType)
    {
        _gridMapData[tilePosX + _width * tilePosY] = tileType;
    }
}

/// <summary> マップの管理を行うクラス </summary>
[Serializable]
public class MapGenerater
{
    /// <summary>分割されたエリアのデータを保存するクラス</summary>
    #region AreaDataClass
    [Serializable]
    public class AreaData
    {
        //エリアの頂点と底点
        private Vector2Int _areaTopPos;
        private Vector2Int _areaBottomPos;
        
        //エリア内に生成した部屋のデータ
        private Vector2Int _roomTopPos;
        private Vector2Int _roomBottomPos;
        private RoomData _roomData;

        public Vector2Int AreaTopPos => _areaTopPos;
        public Vector2Int AreaBottomPos => _areaBottomPos;
        
        public Vector2Int RoomTopPos => _roomTopPos;
        
        public Vector2Int RoomBottomPos => _roomBottomPos;
        
        public RoomData RoomData => _roomData;
    
        /// <summary>
        /// エリアの一番大きい座標と一番小さい座標を登録
        /// </summary>
        /// <param name="areaTopPos"> 一番小さい座標 </param>
        /// <param name="areaBottomPos"> 1番大きい座標 </param>
        public void SetAreaPos(Vector2Int areaTopPos, Vector2Int areaBottomPos)
        {
            if (areaTopPos.x > areaBottomPos.x || areaTopPos.y > areaBottomPos.y)
            {
                Debug.LogError("座標の大きさが逆転しています");
            }
            _areaTopPos = areaTopPos;
            _areaBottomPos = areaBottomPos;
        }

        public void SetRoomData(Vector2Int roomTopPos, Vector2Int roomBottomPos, RoomData roomData)
        {
            _roomTopPos = roomTopPos;
            _roomBottomPos = roomBottomPos;
            _roomData = roomData;
        }
    }
    #endregion
    
    /// <summary> 分割線同士の交差点 </summary>
    public struct TurningPoint
    {
        public int posX;
        public int posY;
        public bool isConnect;
    }
    
    /// <summary> 部屋から伸びる道の本数の最高値 </summary>
    [SerializeField,Header("部屋から伸びる道の本数の最高値")] int _maxRoomLinkLoadNum = 4;
    
    /// <summary> 各エリアのデータを保存するリスト </summary>
    [SerializeField,Header("各エリアのデータを保存するリスト")] private List<AreaData> _areaDataList;

    /// <summary> 分割線同士の交差点の位置 </summary>
    [SerializeField,Header("分割線同士の交差点の位置")] private List<TurningPoint> _turningPointList;
    
    /// <summary> 部屋のデータを取得し保存するリスト </summary>
    private RoomData[] _roomDataList;
    
    /// <summary> 部屋の最小サイズ </summary>
    private RoomData _minRoomData = null;
    
    /// <summary> マップのエリアを作る際外側2マスを必ず空ける </summary>
    private const int _mapBorderSpace = 2;
    
    ///<summary> 1つのエリアの最小サイズ </summary>
    private int _minAreaSizeX,_minAreaSizeY;

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
    
    /// <summary> Mapの初期化 </summary>
    /// <param name="mapData"> 初期化するマップのデータ </param>
    /// <param name="width">マップの横の長さ</param>
    /// <param name="height">マップの縦の長さ</param>
    /// <param name="divideNum">マップの分割回数</param>
    /// <param name="roomDataPath">部屋の保存先のパス</param>
    public async UniTask<MapData> InitMap(int width, int height, int divideNum, string roomDataPath)
    {
        //マップデータの初期化
        MapData newMapData = new();
        await newMapData.InitData(width, height);
        BuildMapBorderLine(newMapData, width, height);
        _areaDataList = new List<AreaData>();
        
        // 分岐点の配列の初期化
        _turningPointList = new List<TurningPoint>();
        
        //生成する部屋のデータの取得
        _roomDataList = await GetRoomAssets(roomDataPath);
        _minRoomData = GetMinRoom();
        Debug.Log("部屋の最小データ:x" + _minRoomData.Width + " y" + _minRoomData.Height);
        
        //最初のエリアを作る
        AreaData firstArea = new AreaData();
        
        //1つのエリアの大きさの最低値(最小の部屋 + 部屋と分割線との最低限の空白)
        _minAreaSizeX = _minRoomData.Width + 2;
        _minAreaSizeY = _minRoomData.Height + 2;
        
        //Mapの外側2マスをあけてエリアを分割していく
        Vector2Int firstAreaTopPos = new Vector2Int(_mapBorderSpace,_mapBorderSpace);
        Vector2Int firstAreaBottomPos = new Vector2Int(width - _mapBorderSpace, height - _mapBorderSpace);
        
        _areaDataList.Add(firstArea);
        firstArea.SetAreaPos(firstAreaTopPos, firstAreaBottomPos);
        
        //マップの分裂処理
        if(divideNum > 0) AreaMaking(newMapData, divideNum, firstArea);
        
        BuildRoom(newMapData);
        
        return newMapData;
    }
    
    // 1つのマップを複数のエリアに分裂させる機能
    #region AreaMaking
    ///<summary> 1つのマップを複数のエリアに分裂させる関数 </summary>
    /// <param name="newMapData"> 新しく作るマップのデータ </param>
    /// <param name="divideNum"> エリアの分割回数 </param>
    /// <param name="firstArea"> 分割する最初のおおもとのエリア </param>
    private void AreaMaking(MapData newMapData, int divideNum, AreaData firstArea)
    {
        AreaData wideArea = null;//Map内に存在する最も大きいエリアのデータ
        
        for (int i = 0; i < divideNum; i++)
        {
            //最も大きいエリアを検索
            wideArea = SearchWideArea();

            if (TryDivideArea(wideArea))
            {
                //一番大きいエリアの分割
                AreaDivide(newMapData, wideArea);
            }
            else
            {
                Debug.Log(i + "回の分割に成功");
                break;
            }
        }
    }

	/// <summary>1つのエリアを2つに分割する</summary>
	/// <param name="mapData"> エリアを分割するマップのデータ </param>
    /// <param name="divideArea">分割するエリア</param>
    private void AreaDivide(MapData mapData, AreaData divideArea)
    {
        //エリアを保存する際に分割線から1マスずらす
        const int skipPos = 1;
        int randomDividePos;
        AreaData newArea = new AreaData();
        
        //エリアの長さ
        int areaLengthX = divideArea.AreaBottomPos.x - divideArea.AreaTopPos.x;
        int areaLengthY = divideArea.AreaBottomPos.y - divideArea.AreaTopPos.y;

        //エリアが横に大きければ縦に分割
        if (areaLengthX >= areaLengthY)
        {
            randomDividePos = 
                UnityEngine.Random.Range(divideArea.AreaTopPos.x + _minAreaSizeX,
                    divideArea.AreaBottomPos.x - _minAreaSizeX);
            
            newArea.SetAreaPos(new Vector2Int(randomDividePos + skipPos, divideArea.AreaTopPos.y),
                new Vector2Int(divideArea.AreaBottomPos.x, divideArea.AreaBottomPos.y));
            
            divideArea.SetAreaPos(new Vector2Int(divideArea.AreaTopPos.x, divideArea.AreaTopPos.y),
                new Vector2Int(randomDividePos - skipPos, divideArea.AreaBottomPos.y));
            
            SetMapTiles(mapData, TileType.UnWalkable, randomDividePos, randomDividePos, divideArea.AreaTopPos.y, divideArea.AreaBottomPos.y);
            
            //分割線の両端の交差点を分岐点に加える
            SetTurningPoint(randomDividePos, divideArea.AreaTopPos.y - skipPos, false);
            SetTurningPoint(randomDividePos, divideArea.AreaBottomPos.y + skipPos, false);  
        }
        //エリアが縦に大きければ横に分割
        else
        {
            randomDividePos = 
                UnityEngine.Random.Range(divideArea.AreaTopPos.y + _minAreaSizeY,
                    divideArea.AreaBottomPos.y - _minAreaSizeY);
            
            newArea.SetAreaPos(new Vector2Int(divideArea.AreaTopPos.x, randomDividePos + skipPos),
                new Vector2Int(divideArea.AreaBottomPos.x, divideArea.AreaBottomPos.y));
            
            divideArea.SetAreaPos(new Vector2Int(divideArea.AreaTopPos.x, divideArea.AreaTopPos.y),
                new Vector2Int(divideArea.AreaBottomPos.x, randomDividePos - skipPos));
            
            SetMapTiles(mapData, TileType.UnWalkable, divideArea.AreaTopPos.x, divideArea.AreaBottomPos.x, randomDividePos, randomDividePos);

            //分割線の両端の交差点を分岐点に加える
            SetTurningPoint(divideArea.AreaTopPos.x - skipPos, randomDividePos, false);
            SetTurningPoint(divideArea.AreaBottomPos.x + skipPos, randomDividePos, false);
        }
        
        _areaDataList.Add(newArea);
    }

    /// <summary> 一番大きいエリアをリスト内から探す </summary>
    private AreaData SearchWideArea()
    {
        AreaData wideArea = null;
        int newAreaSize, currentWideAreaSize;
        foreach (AreaData areaData in _areaDataList)
        {
            //Nullなら切り抜け
            if (wideArea == null)
            {
                wideArea = areaData;
                continue;
            }
            
            //Nullじゃなければエリアの大きさを比較して更新
            newAreaSize = (areaData.AreaBottomPos.x - areaData.AreaTopPos.x) * (areaData.AreaBottomPos.y - areaData.AreaTopPos.y);
            currentWideAreaSize = (wideArea.AreaBottomPos.x - wideArea.AreaTopPos.x) * (wideArea.AreaBottomPos.y - wideArea.AreaTopPos.y);
            if (newAreaSize > currentWideAreaSize)
                wideArea = areaData;
        }
        
        return wideArea;
    }

    /// <summary> エリアの分割が可能かどうかの判定 </summary>
    /// <param name="divideArea"> 分割したいエリア </param>
    /// <returns> 分割可能ならTrue </returns>
    private bool TryDivideArea(AreaData divideArea)
    {
        //エリアの長さ
        int areaLengthX = divideArea.AreaBottomPos.x - divideArea.AreaTopPos.x;
        int areaLengthY = divideArea.AreaBottomPos.y - divideArea.AreaTopPos.y;
        
        //分割に必要なエリアの大きさ(部屋が二個分と分割線1本分)
        int mustAreaSizeX = _minAreaSizeX * 2 + 1;
        int mustAreaSizeY = _minAreaSizeY * 2 + 1;
        
        //もし分割しようとしているエリアの大きさが必要な大きさに達していなければfalseを返す
        if (areaLengthX >= areaLengthY)
        {
            if (areaLengthX < mustAreaSizeX || areaLengthY < _minAreaSizeY)
            {
                return false;
            }
        }
        else
        {
            if (areaLengthY < mustAreaSizeY || areaLengthX < _minAreaSizeX)
            {
                return false;
            }
        }

        return true;
    }
    #endregion
    
    //指定エリア内に部屋を生成する機能
    #region BuildRoom
    /// <summary> 指定エリア内に部屋を生成する関数 </summary>
    private void BuildRoom(MapData mapData)
    {
        List<RoomData> canBuildRoomDataList = new();
        int canBuildAreaX, canBuildAreaY;
        int arrNum;
        RoomData buildRoom = null;
        
        foreach (AreaData areaData in _areaDataList)
        {
            //部屋を作ることのできるエリアを求める
            canBuildAreaX = areaData.AreaBottomPos.x - areaData.AreaTopPos.x - 2;
            canBuildAreaY = areaData.AreaBottomPos.y - areaData.AreaTopPos.y - 2;
            
            //エリア内に建てることが可能な部屋を取得
            canBuildRoomDataList.Clear();
            foreach (RoomData roomData in _roomDataList)
            {
                if(canBuildAreaX >= roomData.Width && canBuildAreaY >= roomData.Height)
                    canBuildRoomDataList.Add(roomData);
            }
            
            //取得した部屋からランダムに部屋を選択して生成
            buildRoom = canBuildRoomDataList[UnityEngine.Random.Range(0, canBuildRoomDataList.Count - 1)];
            SetRoomTile(mapData, areaData, buildRoom);
        }
    }

    private void SetRoomTile(MapData mapData, AreaData areaData, RoomData roomData)
    {
        //部屋がエリアの外側の分割線と隣接しないようにする
        const int space = 1;
        int roomTopPosX = UnityEngine.Random.Range(areaData.AreaTopPos.x + space, areaData.AreaBottomPos.x - roomData.Width - space);
        int roomTopPosY = UnityEngine.Random.Range(areaData.AreaTopPos.y + space, areaData.AreaBottomPos.y - roomData.Height - space);

        for (int y = 0; y < roomData.Height; y++)
        {
            for (int x = 0; x < roomData.Width; x++)
            {
                TileType tile = roomData.GridRoomData[x + y * roomData.Width];
                mapData.SetTileType(roomTopPosX + x, roomTopPosY + y, tile);
            }
        }
    }
    #endregion

    //todo:
    //部屋同士をつなぐ道を作る機能
    #region ConnectRoom
    /// <summary> 部屋同士をつなぐ道を作る関数 </summary>
    private void ConnectRooms(MapData mapData)
    {
        //todo:部屋同士を完全につなぐ処理
        foreach (AreaData areaData in _areaDataList)
        {
            TurningPoint connectPoint = BuildConnectPoint(mapData, areaData);
        }
    }

    private TurningPoint BuildConnectPoint(MapData mapData, List<AreaData> areaDataList)
    {
        //todo:部屋と道をつなぐターニングポイントを作る処理
        TurningPoint connectRoomPoint = new();
        TurningPoint connectDivideLinePoint = new();
        List<TurningPoint> startPointList = new();
        const int minRoomLinkLoadNum = 1;
        int randomLoadNum;

        foreach (AreaData areaData in areaDataList)
        {
            int yLength = areaData.RoomBottomPos.y - areaData.RoomTopPos.y;
            int xLength = areaData.RoomBottomPos.x - areaData.RoomTopPos.x;
            int 
            for(int i = areaData.)
        }
    }
    
    private void ConnectAllPoint(MapData mapData, List<TurningPoint> startPointList)
    {
        //todo:ターニングポイントからターニングポイントまで道をつなぐ処理
        
    }
    #endregion

    //上記以外の機能まとめ
    #region OtherMethod
    /// <summary>部屋のデータを取得</summary>
    /// <param name="roomDataPath">取得したいデータのラベル名</param>
    private async UniTask<RoomData[]> GetRoomAssets(string roomDataPath)
    {
        return await UniTask.Run(() =>
        {
            RoomData[] roomDataList = Resources.LoadAll<RoomData>("RoomData/" + roomDataPath);

            if (roomDataList == null || roomDataList.Length == 0)
            {
                Debug.LogWarning($"RoomData が見つかりません: {roomDataPath}");
            }

            return roomDataList;
        });
    }
    
    /// <summary>一定の範囲のタイルを同じタイルで埋める</summary>
    private void SetMapTiles(MapData mapData, TileType putTile, int topPosX, int bottomPosX, int topPosY, int bottomPosY)
    {
        for (int y = topPosY; y <= bottomPosY; y++)
        {
            for (int x = topPosX; x <= bottomPosX; x++)
            {
                mapData.SetTileType(x, y, putTile);
            }
        }
    }

    private void SetTurningPoint(int posX, int posY, bool isConnect)
    {
        _turningPointList.Add(new TurningPoint {posX = posX, posY = posY, isConnect = isConnect});
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

    /// <summary> マップの端の部分となる外側2マスを埋める処理 </summary>
    private void BuildMapBorderLine(MapData mapData, int width, int height)
    {
        const int borderLine = 2;
        
        // 上2行
        for (int y = 0; y < borderLine; y++)
        {
            for (int x = 0; x < width; x++)
            {
                mapData.SetTileType(x, y, TileType.UnWalkable);
            }
        }

        // 下2行
        for (int y = height - borderLine; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                mapData.SetTileType(x, y, TileType.UnWalkable);
            }
        }

        // 左2列（中間部分だけ）
        for (int y = borderLine; y < height - borderLine; y++)
        {
            for (int x = 0; x < borderLine; x++)
            {
                mapData.SetTileType(x, y, TileType.UnWalkable);
            }
        }

        // 右2列（中間部分だけ）
        for (int y = borderLine; y < height - borderLine; y++)
        {
            for (int x = width - borderLine; x < width; x++)
            {
                mapData.SetTileType(x, y, TileType.UnWalkable);
            }
        }
    }
    #endregion
}

[System.Serializable]
public enum TileType
{
    Empty = 0,
    Walkable = 1,
    UnWalkable = 2,
}