using Cysharp.Threading.Tasks;
using RandomWeightPick;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
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
    public static async Task<DungeonData> DungeonBuild(int sectionSize, int horizontalSectionNum, int VerticalSectionNum, int minRoomNum, int maxRoomNum, string roomDataPath)
    {
        _dungeonData = new DungeonData();
        int maxRoomSize = sectionSize - _cantBuildRoomSpace;

        _canBuildData = await LoadCanBuildRoomData(roomDataPath, maxRoomSize);

        _dungeonData.InitDungeonData(horizontalSectionNum, VerticalSectionNum);

        for(int x = 0; x < horizontalSectionNum; x++)
        {
            for(int y = 0; y < VerticalSectionNum; y++)
            {
                _dungeonData.SetSectionData(SectionBuilder.InitSection(sectionSize) ,x, y);
            }
        }

        RandomPickBuildRoomSection(minRoomNum,maxRoomNum);
        BuildDungeonRooms();
        
        return _dungeonData;
    }

    private static Task<RoomData[]> LoadCanBuildRoomData(string roomDataPath, int maxRoomSize)
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
        return Task.FromResult(canBuildRoomDataList.ToArray());
    }

    private static void RandomPickBuildRoomSection(int minRoomNum, int maxRoomNum)
    {
        const int weight = 1;
        List<RandomPickItem<SectionData>> randomPickSection = new List<RandomPickItem<SectionData>>();
        foreach (var item in _dungeonData.SectionDataArray)
        {
            RandomPickItem<SectionData> sectionData = new RandomPickItem<SectionData>(item, weight);
            randomPickSection.Add(sectionData);
        }

        for(int roomCount = 0;  roomCount < Random.Range(minRoomNum,maxRoomNum); roomCount++)
        {
            SectionData section = RandomPickItem.SelectRandomItem(randomPickSection, true);
            section.IsBuildRoom = true;
        }
    }

    private static void BuildDungeonRooms()
    {
        foreach (var data in _dungeonData.SectionDataArray)
        {
            if (data.IsBuildRoom)
            {
                SectionBuilder.BuildRoom(data, _canBuildData[Random.Range(0, _canBuildData.Length - 1)]);
            }
            else
            {
                SectionBuilder.BuildJoint(data);
            }
        }
    }
}