using UnityEngine;

/// <summary> Section内部の細かい設定をするロジック </summary>
public static class SectionBuilder
{
    const int _cantBuildRoomSpace = 2;

    public static SectionData InitSection(int sectionSize)
    {
        SectionData sectionData = new SectionData();
        sectionData.InitSectionData(sectionSize);
        return sectionData;
    }

    public static void BuildRoom(SectionData sectionData, RoomData roomData)
    {
        Vector2Int randomStartPos = new Vector2Int
        {
            x = (Random.Range(_cantBuildRoomSpace, sectionData.GridArrRange - (_cantBuildRoomSpace + roomData.Width))),
            y = (Random.Range(_cantBuildRoomSpace, sectionData.GridArrRange - (_cantBuildRoomSpace + roomData.Width)))
        };

        for (int x = randomStartPos.x; x < roomData.Width + randomStartPos.x; x++) 
        { 
            for(int y = randomStartPos.y; y < roomData.Height + randomStartPos.y; y++)
            {
                int sectionGridPos = y * sectionData.GridArrRange + x;
                int roomGridPos = (y - randomStartPos.y) * roomData.Width + (x - randomStartPos.x);

                sectionData.GridData[sectionGridPos].SetGridData(roomData.GridRoomData[roomGridPos]);
            }
        }
    }

    public static void BuildJoint(SectionData sectionData)
    {
        Joint joint = new Joint();

        Vector2Int jointPos = new Vector2Int
        {
            x = Random.Range(_cantBuildRoomSpace, sectionData.GridArrRange - _cantBuildRoomSpace),
            y = Random.Range(_cantBuildRoomSpace, sectionData.GridArrRange - _cantBuildRoomSpace)
        };

        joint.SetRelay(jointPos);
    }
}