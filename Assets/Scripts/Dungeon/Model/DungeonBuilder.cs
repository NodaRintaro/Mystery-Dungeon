using Cysharp.Threading.Tasks;
using RandomWeightPick;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Dungeonを生成するためのロジッククラス </summary>
public static class DungeonBuilder
{
    private static DungeonData _dungeonData;
    private const int _cantBuildRoomSpace = 4;
    private static RoomData[] _canBuildData = null;

    /// <summary>
    /// Dungeonの自動生成ロジック
    /// </summary>
    /// <param name="sectionSize"> 生成するダンジョンのセクションの大きさ </param>
    /// <param name="horizontalSectionNum"> 横に生成するセクションの個数 </param>
    /// <param name="VerticalSectionNum"> 縦に生成するセクションの個数 </param>
    /// <param name="minRoomNum"> 生成する部屋の最小個数 </param>
    /// <param name="maxRoomNum"> 生成する部屋の最大個数 </param>
    /// <param name="roomDataPath"> 部屋のDataのPath </param>
    /// <returns> DungeonData </returns>
    public static async UniTask<DungeonData> DungeonBuild(int sectionSize, int horizontalSectionNum, int VerticalSectionNum, int minRoomNum, int maxRoomNum, int addLoadNum, string roomDataPath)
    {
        _dungeonData = new DungeonData();
        int maxRoomSize = sectionSize - _cantBuildRoomSpace;

        //部屋のデータを取得
        _canBuildData = await LoadCanBuildRoomData(roomDataPath, maxRoomSize);

        //ダンジョンのデータを初期化
        _dungeonData.InitDungeonData(horizontalSectionNum, VerticalSectionNum);

        //各セクションのGridDataを構築
        for(int x = 0; x < horizontalSectionNum; x++)
        {
            for(int y = 0; y < VerticalSectionNum; y++)
            {
                _dungeonData.SetSectionData(SectionBuilder.InitSection(sectionSize) ,x, y);
            }
        }

        //部屋を作るセクションを選択
        RandomPickBuildRoomInSections(minRoomNum,maxRoomNum);

        //実際にセクション内部に部屋を作る
        BuildRooms();

        //部屋全体を道で繋げる
        BuildLoad(addLoadNum);


        
        return _dungeonData;
    }

    private static UniTask<RoomData[]> LoadCanBuildRoomData(string roomDataPath, int maxRoomSize)
    {
        RoomData[] roomData = Resources.LoadAll<RoomData>("RoomData/" + roomDataPath);
        List<RoomData> canBuildRoomDataList = new List<RoomData>();

        foreach(var room in  roomData)
        {
            if(room.Height <= maxRoomSize&&room.Width <= maxRoomSize)
            {
                canBuildRoomDataList.Add(room);
            }
        }

        Debug.Log(canBuildRoomDataList.Count);
        return UniTask.FromResult(canBuildRoomDataList.ToArray());
    }

    private static void RandomPickBuildRoomInSections(int minRoomNum, int maxRoomNum)
    {
        const int weight = 1;
        List<RandomPickItem<SectionData>> randomPickSection = new List<RandomPickItem<SectionData>>();
        foreach (var item in _dungeonData.SectionDataArray)
        {
            RandomPickItem<SectionData> sectionData = new RandomPickItem<SectionData>(item, weight);
            randomPickSection.Add(sectionData);
        }

        for(int roomCount = 0;  roomCount < UnityEngine.Random.Range(minRoomNum,maxRoomNum); roomCount++)
        {
            SectionData section = RandomPickItem.SelectRandomItem(randomPickSection, true);
            section.IsBuildRoom = true;
        }
    }

    private static void BuildRooms()
    {
        foreach (var data in _dungeonData.SectionDataArray)
        {
            if (data.IsBuildRoom)
            {
                SectionBuilder.BuildRoom(data, _canBuildData[UnityEngine.Random.Range(0, _canBuildData.Length - 1)]);
                Debug.Log("部屋を生成");
            }
            else
            {
                SectionBuilder.BuildRelay(data);
                Debug.Log("通路を生成");
            }
        }
    }

    private static void BuildLoad(int addLoadNum)
    {
        Vector2Int connectSectionIndex = new Vector2Int(0, 0);
        Vector2Int saveCurrentSectionIndex = connectSectionIndex;
        _dungeonData.SectionDataArray[connectSectionIndex.x, connectSectionIndex.y].IsConnect = true;

        while (TrySearchConnectSection(ref connectSectionIndex, false))
        {
            _dungeonData.SectionDataArray[connectSectionIndex.x, connectSectionIndex.y].IsConnect = true;
            ConnectSections(saveCurrentSectionIndex, connectSectionIndex);
            saveCurrentSectionIndex = connectSectionIndex;
            Debug.Log("道を作ります");
        }

        ConnectAllSections();
        ConnectAllLoad();

        //追加で何本か道を敷いておく
        RandomAddLoad(addLoadNum);
    }

    private static void ConnectAllSections()
    {
        int widthLength = _dungeonData.SectionDataArray.GetLength(0);
        int heightLength = _dungeonData.SectionDataArray.GetLength(1);

        for (int x = 0; x < widthLength; x++)
        {
            for (int y = 0; y < heightLength; y++)
            {
                Vector2Int checkConnectSectionIndex = new Vector2Int(x, y);

                if (!_dungeonData.SectionDataArray[x, y].IsConnect)
                {
                    BuildLoadNotConnectSection(checkConnectSectionIndex);
                }
            }
        }
    }

    private static void ConnectAllLoad()
    {
        const int minLoadNum = 2;
        int widthLength = _dungeonData.SectionDataArray.GetLength(0);
        int heightLength = _dungeonData.SectionDataArray.GetLength(1);

        for (int x = 0; x < widthLength; x++)
        {
            for (int y = 0; y < heightLength; y++)
            {
                Vector2Int checkConnectSectionIndex = new Vector2Int(x, y);

                if (!_dungeonData.SectionDataArray[x, y].IsBuildRoom && ConnectLoadNum(checkConnectSectionIndex) < minLoadNum)
                {
                    Vector2Int saveSectionIndex = checkConnectSectionIndex;
                    TrySearchConnectSection(ref checkConnectSectionIndex, true);
                    ConnectSections(saveSectionIndex, checkConnectSectionIndex);
                }
            }
        }
    }

    private static void RandomAddLoad(int addLoadNum)
    {
        int widthLength = _dungeonData.SectionDataArray.GetLength(0);
        int heightLength = _dungeonData.SectionDataArray.GetLength(1);

        for (int addLoadCount = 0; addLoadCount < addLoadNum; addLoadCount++)
        {
            Vector2Int addLoadSectionIndex =
                new Vector2Int
                {
                    x = Random.Range(0, widthLength),
                    y = Random.Range(0, heightLength)
                };

            Vector2Int saveIndex = addLoadSectionIndex;
            TrySearchConnectSection(ref addLoadSectionIndex, true);
            ConnectSections(addLoadSectionIndex, saveIndex);
        }
    }

    private static void BuildLoadNotConnectSection(Vector2Int connectSectionIndex)
    {
        Vector2Int saveConnectIndex = connectSectionIndex;
        _dungeonData.SectionDataArray[connectSectionIndex.x, connectSectionIndex.y].IsConnect = true;
        
        if(TrySearchConnectSection(ref connectSectionIndex, true))
        {
            ConnectSections(saveConnectIndex, connectSectionIndex);
        }
        else
        {
            if(TrySearchConnectSection(ref connectSectionIndex, false))
            {
                ConnectSections(saveConnectIndex, connectSectionIndex);
                BuildLoadNotConnectSection(connectSectionIndex);
            }
        }
    }

    private static bool TrySearchConnectSection(ref Vector2Int connectSectionIndex, bool isConnectOther)
    {
        bool found = false;
        List<Vector2Int> canConnectSections = CanConnectSectionList(connectSectionIndex, isConnectOther);

        if(canConnectSections.Count > 0)
        {
            connectSectionIndex = canConnectSections[Random.Range(0, canConnectSections.Count)];
            found = true;
        }

        return found;
    }

    private static List<Vector2Int> CanConnectSectionList(Vector2Int connectSectionIndex, bool isConnectOther)
    {
        const int shiftArrPos = 1;
        List<Vector2Int> canConnectSections = new();
        SectionData sectionData = _dungeonData.SectionDataArray[connectSectionIndex.x, connectSectionIndex.y];

        if (!sectionData.IsConnectDirectionDict[ConnectDirection.Top])
        {
            IsValidSection(connectSectionIndex.x, connectSectionIndex.y + shiftArrPos, canConnectSections, isConnectOther);
        }
        if (!sectionData.IsConnectDirectionDict[ConnectDirection.Bottom])
        {
            IsValidSection(connectSectionIndex.x, connectSectionIndex.y - shiftArrPos, canConnectSections, isConnectOther);
        }
        if (!sectionData.IsConnectDirectionDict[ConnectDirection.Left])
        {
            IsValidSection(connectSectionIndex.x - shiftArrPos, connectSectionIndex.y, canConnectSections, isConnectOther);
        }
        if (!sectionData.IsConnectDirectionDict[ConnectDirection.Right])
        {
            IsValidSection(connectSectionIndex.x + shiftArrPos, connectSectionIndex.y, canConnectSections, isConnectOther);
        }

        return canConnectSections;
    }

    private static int ConnectLoadNum(Vector2Int checkSectionIndex)
    {
        int loadNum = 0;
        SectionData sectionData = _dungeonData.SectionDataArray[checkSectionIndex.x, checkSectionIndex.y];

        if (sectionData.IsConnectDirectionDict[ConnectDirection.Top])
        {
            loadNum++;
        }
        if (sectionData.IsConnectDirectionDict[ConnectDirection.Bottom])
        {
            loadNum++;
        }
        if (sectionData.IsConnectDirectionDict[ConnectDirection.Left])
        {
            loadNum++;
        }
        if (sectionData.IsConnectDirectionDict[ConnectDirection.Right])
        {
            loadNum++;
        }

        return loadNum;
    }

    private static void IsValidSection(int x, int y, List<Vector2Int> list ,bool isConnectOther)
    {
        int widthLength = _dungeonData.SectionDataArray.GetLength(0);
        int heightLength = _dungeonData.SectionDataArray.GetLength(1);

        if (x >= 0 && x < widthLength && y >= 0 && y < heightLength && _dungeonData.SectionDataArray[x, y].IsConnect == isConnectOther)
        {
            list.Add(new Vector2Int(x, y));
        }
    }

    private static void ConnectSections(Vector2Int baseConnectSectionPos, Vector2Int connectSectionPos)
    {
        SectionData baseSection = _dungeonData.SectionDataArray[baseConnectSectionPos.x, baseConnectSectionPos.y];
        SectionData connectSection = _dungeonData.SectionDataArray[connectSectionPos.x, connectSectionPos.y];
        SectionBuilder.CreateSectionLinkLoad(baseSection, connectSection, baseConnectSectionPos, connectSectionPos);
    }
}