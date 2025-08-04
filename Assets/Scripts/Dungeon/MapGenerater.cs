using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary> マップの管理を行うクラス </summary>
[Serializable]
public class MapGenerater
{
    /// <summary> 部屋から追加で伸ばす道の本数の最高値と最低値 </summary>
    [SerializeField, Header("部屋から追加で伸ばす道の本数の最高値")] int _maxPlusLoadNum = 4;
    [SerializeField, Header("部屋から追加で伸ばす道の本数の最低値")] int _minPlusLoadNum = 1;
    
    /// <summary> 各エリアのデータを保存するリスト </summary>
    [SerializeField,Header("各エリアのデータを保存するリスト")] private List<AreaData> _areaDataList;
    
    private MapData _newMapData;

    /// <summary> 部屋のデータを取得し保存するリスト </summary>
    private RoomData[] _roomDataList;
    
    /// <summary> 部屋の最小サイズ </summary>
    private RoomData _minRoomData = null;
    
    /// <summary> エリアを作る際の外側2マスを必ず空ける </summary>
    private const int _areaBorderSpace = 2;
    
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
        _newMapData = new();
        _newMapData.InitData(width, height);
        BuildMapBorderLine(width, height);
        _areaDataList = new();

        //生成する部屋のデータの取得
        _roomDataList = await GetRoomAssets(roomDataPath);
        _minRoomData = GetMinRoom();
        
        //最初のエリアを作る
        AreaData firstArea = new AreaData();
        
        //1つのエリアの大きさの最低値(最小の部屋 + 部屋と分割線との最低限の空白)
        _minAreaSizeX = _minRoomData.Width + _areaBorderSpace;
        _minAreaSizeY = _minRoomData.Height + _areaBorderSpace;
        
        //Mapの外側2マスをあけてエリアを分割していく
        Vector2Int firstAreaTopLeftPos = new Vector2Int(_areaBorderSpace, _areaBorderSpace);
        Vector2Int firstAreaTopRightPos = new Vector2Int(width - _areaBorderSpace, _areaBorderSpace);
        Vector2Int firstAreaBottomLeftPos = new Vector2Int(_areaBorderSpace, height - _areaBorderSpace);
        Vector2Int firstAreaBottomRightPos = new Vector2Int(width - _areaBorderSpace, height - _areaBorderSpace);
        
        _areaDataList.Add(firstArea);
        firstArea.SetAreaPos(firstAreaTopLeftPos, firstAreaTopRightPos, firstAreaBottomLeftPos, firstAreaBottomRightPos);
        
        //マップの分裂処理
        if(divideNum > 0)
        {
            AreaMaking(divideNum, firstArea);

            BuildRoom();

            ConnectArea();
        }
        else
        {
            BuildRoom();
        }
        
        return _newMapData;
    }
    
    // 1つのマップを複数のエリアに分裂させる機能
    #region AreaMaking
    ///<summary> 1つのマップを複数のエリアに分裂させる関数 </summary>
    /// <param name="_newMapData"> 新しく作るマップのデータ </param>
    /// <param name="divideNum"> エリアの分割回数 </param>
    /// <param name="firstArea"> 分割する最初のおおもとのエリア </param>
    private void AreaMaking(int divideNum, AreaData firstArea)
    {
        AreaData wideArea = null;//Map内に存在する最も大きいエリアのデータ
        
        for (int i = 0; i < divideNum; i++)
        {
            //最も大きいエリアを検索
            wideArea = SearchWideArea();

            if (TryDivideArea(wideArea))
            {
                //一番大きいエリアの分割
                AreaDivide(_newMapData, wideArea);
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
        const int shiftDivideLineSpace = 1;
        int randomDividePos;
        AreaData newArea = new AreaData();
        
        //エリアの長さ
        int areaLengthX = divideArea.AreaBottomRightPos.x - divideArea.AreaTopLeftPos.x;
        int areaLengthY = divideArea.AreaBottomRightPos.y - divideArea.AreaTopLeftPos.y;

        //エリアが横に大きければ縦に分割
        if (areaLengthX >= areaLengthY)
        {
            randomDividePos = 
                UnityEngine.Random.Range(divideArea.AreaTopLeftPos.x + _minAreaSizeX,
                    divideArea.AreaBottomRightPos.x - _minAreaSizeX);

            //分割したエリアの左半分を保存
            newArea.SetAreaPos(
                divideArea.AreaTopLeftPos,
                new Vector2Int(randomDividePos - shiftDivideLineSpace, divideArea.AreaTopLeftPos.y),
                divideArea.AreaBottomLeftPos,
                new Vector2Int(randomDividePos - shiftDivideLineSpace, divideArea.AreaBottomLeftPos.y)
                );

            //分割したエリアの右半分を保存
            divideArea.SetAreaPos(
                new Vector2Int(randomDividePos + shiftDivideLineSpace, divideArea.AreaTopRightPos.y),
                divideArea.AreaTopRightPos,
                new Vector2Int(randomDividePos + shiftDivideLineSpace, divideArea.AreaBottomRightPos.y),
                divideArea.AreaBottomRightPos
                );
            
            SetMapTiles(TileType.UnWalkable, randomDividePos, randomDividePos, divideArea.AreaTopLeftPos.y, divideArea.AreaBottomRightPos.y);
        }
        //エリアが縦に大きければ横に分割
        else
        {
            randomDividePos = 
                UnityEngine.Random.Range(divideArea.AreaTopLeftPos.y + _minAreaSizeY,
                    divideArea.AreaBottomRightPos.y - _minAreaSizeY);
            
            //分割したエリアの下半分を保存
            newArea.SetAreaPos(
                new Vector2Int(divideArea.AreaTopLeftPos.x, randomDividePos - shiftDivideLineSpace),
                new Vector2Int(divideArea.AreaTopRightPos.x, randomDividePos - shiftDivideLineSpace),
                divideArea.AreaBottomLeftPos,
                divideArea.AreaBottomRightPos);
            
            //分割したエリアの上半分を保存
            divideArea.SetAreaPos(
                divideArea.AreaTopLeftPos,
                divideArea.AreaTopRightPos,
                new Vector2Int(divideArea.AreaBottomLeftPos.x, randomDividePos + shiftDivideLineSpace),
                new Vector2Int(divideArea.AreaBottomRightPos.x, randomDividePos + shiftDivideLineSpace)
                );
            
            SetMapTiles(TileType.UnWalkable, divideArea.AreaTopLeftPos.x, divideArea.AreaBottomRightPos.x, randomDividePos, randomDividePos);
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
            newAreaSize = (areaData.AreaBottomRightPos.x - areaData.AreaTopLeftPos.x) * (areaData.AreaBottomRightPos.y - areaData.AreaTopLeftPos.y);
            currentWideAreaSize = (wideArea.AreaBottomRightPos.x - wideArea.AreaTopLeftPos.x) * (wideArea.AreaBottomRightPos.y - wideArea.AreaTopLeftPos.y);
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
        int areaLengthX = divideArea.AreaBottomRightPos.x - divideArea.AreaTopLeftPos.x;
        int areaLengthY = divideArea.AreaBottomRightPos.y - divideArea.AreaTopLeftPos.y;
        
        //分割に必要なエリアの大きさ(部屋が二個分と分割線1本分)
        int minDivideAreaSizeX = _minAreaSizeX * 2 + 1;
        int minDivideAreaSizeY = _minAreaSizeY * 2 + 1;
        
        //もし分割しようとしているエリアの大きさが必要な大きさに達していなければfalseを返す
        if (areaLengthX >= areaLengthY)
        {
            if (areaLengthX < minDivideAreaSizeX || areaLengthY < _minAreaSizeY)
            {
                return false;
            }
        }
        else
        {
            if (areaLengthY < minDivideAreaSizeY || areaLengthX < _minAreaSizeX)
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
    private void BuildRoom()
    {
        List<RoomData> canBuildRoomDataList = new();
        int canBuildSpaceX, canBuildSpaceY;
        RoomData buildRoom = null;
        
        foreach (AreaData areaData in _areaDataList)
        {
            //部屋を作ることのできるエリアを求める
            canBuildSpaceX = areaData.AreaBottomRightPos.x - areaData.AreaTopLeftPos.x - _areaBorderSpace;
            canBuildSpaceY = areaData.AreaBottomRightPos.y - areaData.AreaTopLeftPos.y - _areaBorderSpace;
            
            //エリア内に建てることが可能な部屋を取得
            canBuildRoomDataList = new();
            foreach (RoomData roomData in _roomDataList)
            {
                if(canBuildSpaceX >= roomData.Width && canBuildSpaceY >= roomData.Height)
                    canBuildRoomDataList.Add(roomData);
            }
            
            //取得した部屋からランダムに部屋を選択して生成
            buildRoom = canBuildRoomDataList[UnityEngine.Random.Range(0, canBuildRoomDataList.Count)];
            SetRoom(areaData, buildRoom);
        }
    }

    private void SetRoom(AreaData areaData, RoomData roomData)
    {
        //部屋がエリアの外側の分割線と隣接しないようにする
        const int blankSpace = 1;
        int roomTopPosX = UnityEngine.Random.Range(areaData.AreaTopLeftPos.x + blankSpace, areaData.AreaBottomRightPos.x - roomData.Width - blankSpace);
        int roomTopPosY = UnityEngine.Random.Range(areaData.AreaTopLeftPos.y + blankSpace, areaData.AreaBottomRightPos.y - roomData.Height - blankSpace);

        Vector2Int leftTopPos = new Vector2Int(roomTopPosX, roomTopPosY);
        Vector2Int leftBottomPos = new Vector2Int(roomTopPosX, roomTopPosY + roomData.Height);
        Vector2Int rightTopPos = new Vector2Int(roomTopPosX + roomData.Width, roomTopPosY);
        Vector2Int rightBottomPos = new Vector2Int(roomTopPosX + roomData.Width, roomTopPosY + roomData.Height);


        for (int y = 0; y < roomData.Height; y++)
        {
            for (int x = 0; x < roomData.Width; x++)
            {
                TileType tile = roomData.GridRoomData[x + y * roomData.Width];
                _newMapData.SetTileType(roomTopPosX + x, roomTopPosY + y, tile);
            }
        }

        SaveRoomData(areaData, leftTopPos, leftBottomPos, rightTopPos, rightBottomPos, roomData);
    }

    /// <summary> 部屋のデータの保存処理 </summary>
    private void SaveRoomData(AreaData areaData, Vector2Int leftTopPos, Vector2Int leftBottomPos, Vector2Int rightTopPos, Vector2Int rightBottomPos, RoomData roomData)
    {
        areaData.SetRoomData(leftTopPos, leftBottomPos,　rightTopPos, rightBottomPos, roomData);
    }
    #endregion

    //todo:
    //部屋同士をつなぐ道を作る機能
    #region ConnectRooms
    /// <summary> 部屋同士をつなぐ道を作る関数 </summary>
    private void ConnectArea()
    {
        //todo:部屋同士を完全につなぐ処理
        //エリア同士をつなぐための始点となるエリアを決める
        int areaDataRange = _areaDataList.Count - 1;
        int randomAreaDataNum = UnityEngine.Random.Range(0, areaDataRange);
        AreaData currentConnectArea = _areaDataList[randomAreaDataNum];
        AreaData saveBeforeConnectArea = null;//先ほどまでいたエリアのセーブ用変数

        while (TryConnectAreaFind(out currentConnectArea))
        {
            //Todo:TryConnectAreaで取得したエリアと先ほどまでいたエリア同士を道でつなげる処理
            ConnectArea(saveBeforeConnectArea, currentConnectArea);
        }

        int addLoadNum = UnityEngine.Random.Range(_minPlusLoadNum, _maxPlusLoadNum);//追加する道の本数
        for (int addLoadCount = 0; addLoadNum > addLoadCount; addLoadCount++)
        {
            //Todo:道の本数をテキトーに増やす処理
        }
    }

    private bool TryConnectAreaFind(out AreaData currentConnectArea)
    {
        //ToDo:次に接続可能なエリアが存在するかどうか探して存在するのであればその中からランダムにエリアを選択する
        bool success = false;
        AreaData nextConnectArea = null;
        currentConnectArea = nextConnectArea;
        return success;
    }

    private void ConnectArea(AreaData startConnectArea, AreaData goalConnectArea)
    {
        //Todo:エリア同士を道でつなげる処理
    }

    #endregion

    //上記以外の機能まとめ
    #region OtherMethod
    /// <summary>部屋のデータを取得</summary>
    /// <param name="roomDataPath">取得したいデータのラベル名</param>
    private async UniTask<RoomData[]> GetRoomAssets(string roomDataPath)
    {
        List<RoomData> roomList = new();
        await Addressables.LoadAssetsAsync<RoomData>(roomDataPath, roomList.Add);
        return roomList.ToArray();
    }

    /// <summary>一定の範囲のタイルを同じタイルで埋める</summary>
    private void SetMapTiles(TileType putTile, int topPosX, int bottomPosX, int topPosY, int bottomPosY)
    {
        for (int y = topPosY; y <= bottomPosY; y++)
            for (int x = topPosX; x <= bottomPosX; x++)
                _newMapData.SetTileType(x, y, putTile);
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
    private void BuildMapBorderLine(int width, int height)
    {
        const int borderLine = 2;
        
        // 上2行
        for (int y = 0; y < borderLine; y++)
        {
            for (int x = 0; x < width; x++)
            {
                _newMapData.SetTileType(x, y, TileType.UnWalkable);
            }
        }

        // 下2行
        for (int y = height - borderLine; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                _newMapData.SetTileType(x, y, TileType.UnWalkable);
            }
        }

        // 左2列（中間部分だけ）
        for (int y = borderLine; y < height - borderLine; y++)
        {
            for (int x = 0; x < borderLine; x++)
            {
                _newMapData.SetTileType(x, y, TileType.UnWalkable);
            }
        }

        // 右2列（中間部分だけ）
        for (int y = borderLine; y < height - borderLine; y++)
        {
            for (int x = width - borderLine; x < width; x++)
            {
                _newMapData.SetTileType(x, y, TileType.UnWalkable);
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