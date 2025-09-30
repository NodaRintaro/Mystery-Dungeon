using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
            x = (Random.Range(_cantBuildRoomSpace, sectionData.GridWidth - (_cantBuildRoomSpace + roomData.Width))),
            y = (Random.Range(_cantBuildRoomSpace, sectionData.GridWidth - (_cantBuildRoomSpace + roomData.Height)))
        };

        int topPosX = roomData.Width + randomStartPos.x - 1;
        int topPosY = roomData.Height + randomStartPos.y - 1;

        for (int x = randomStartPos.x; x < roomData.Width + randomStartPos.x; x++) 
        { 
            for(int y = randomStartPos.y; y < roomData.Height + randomStartPos.y; y++)
            {
                int roomGridPos = (y - randomStartPos.y) * roomData.Width + (x - randomStartPos.x);

                sectionData.GridDataArr[x + (y * sectionData.GridWidth)].SetGridData(roomData.GridRoomData[roomGridPos]);

                if((x == randomStartPos.x || y == randomStartPos.y) && sectionData.GetGridData(x, y).TileType == TileType.Ground)
                {
                    CreateJoint(sectionData, new Vector2Int(x,y), JointType.Relay);
                }
                else if((x == topPosX || y == topPosY) && sectionData.GetGridData(x, y).TileType == TileType.Ground)
                {
                    CreateJoint(sectionData, new Vector2Int(x, y), JointType.Relay);
                }
            }
        }
    }

    public static void BuildRelay(SectionData sectionData)
    {
        Vector2Int jointPos = new Vector2Int
        {
            x = Random.Range(_cantBuildRoomSpace, sectionData.GridWidth - _cantBuildRoomSpace),
            y = Random.Range(_cantBuildRoomSpace, sectionData.GridWidth - _cantBuildRoomSpace)
        };

        CreateJoint(sectionData, jointPos, JointType.Relay);
    }

    public static void CreateSectionLinkLoad(SectionData baseSection, SectionData connectSection, Vector2Int baseSectionPos, Vector2Int connectSectionPos)
    {
        Joint baseSectionExit;
        Joint connectSectionExit;

        switch (baseSectionPos)
        {
            case var x when x.x < connectSectionPos.x:
                baseSectionExit = MakeExit(baseSection, ConnectDirection.Right);
                connectSectionExit = MakeExit(connectSection, ConnectDirection.Left);
                ConnectExitVertical(connectSection, baseSectionExit, connectSectionExit);
                    break;

            case var x when x.x > connectSectionPos.x:
                baseSectionExit = MakeExit(baseSection, ConnectDirection.Left);
                connectSectionExit = MakeExit(connectSection, ConnectDirection.Right);
                ConnectExitVertical(connectSection, baseSectionExit, connectSectionExit);
                break;

            case var y when y.y < connectSectionPos.y:
                baseSectionExit = MakeExit(baseSection, ConnectDirection.Top);
                connectSectionExit = MakeExit(connectSection, ConnectDirection.Bottom);
                ConnectExitHorizontal(connectSection, baseSectionExit, connectSectionExit);
                break;

            case var y when y.y > connectSectionPos.y:
                baseSectionExit = MakeExit(baseSection, ConnectDirection.Bottom);
                connectSectionExit = MakeExit(connectSection, ConnectDirection.Top);
                ConnectExitHorizontal(connectSection, baseSectionExit, connectSectionExit);
                break;
        }
    }

    private static Joint CreateJoint(SectionData sectionData, Vector2Int jointPos, JointType jointType)
    {
        if(sectionData.GetGridData(jointPos.x, jointPos.y).TileType != TileType.Ground)
        {
            sectionData.GridDataArr[jointPos.x + (jointPos.y * sectionData.GridWidth)].SetGridData(TileType.Ground);
        }

        Joint joint = new Joint{ JointPos = jointPos };
        sectionData.AddJointDict(jointType, joint);

        return joint;
    }

    private static Joint MakeExit(SectionData sectionData ,ConnectDirection direction)
    {
        List<Joint> pickEntranceList = new();
        Joint entranceJoint = default;
        Joint exitJoint = default;
        int entranceDirectionPos = 0;
        Vector2Int exitJointPos;

        switch (direction)
        {
            case ConnectDirection.Left:
                sectionData.AddConnectDirection(ConnectDirection.Left);
                entranceDirectionPos = sectionData.JointDataDict[JointType.Relay].Min(joint => joint.JointPos.x);

                foreach (Joint joint in sectionData.JointDataDict[JointType.Relay])
                {
                    if(joint.JointPos.x == entranceDirectionPos)
                    {
                        pickEntranceList.Add(joint);
                    }
                }

                entranceJoint = pickEntranceList[Random.Range(0, pickEntranceList.Count - 1)];
                exitJointPos = new Vector2Int(0, entranceJoint.JointPos.y);
                exitJoint = CreateJoint(sectionData, exitJointPos, JointType.SectionExit);

                BuildLoadHorizontal(sectionData, exitJointPos.x, entranceDirectionPos, exitJointPos.y);

                break; 
            case ConnectDirection.Right:
                sectionData.AddConnectDirection(ConnectDirection.Right);
                entranceDirectionPos = sectionData.JointDataDict[JointType.Relay].Max(joint => joint.JointPos.x);

                foreach (Joint joint in sectionData.JointDataDict[JointType.Relay])
                {
                    if (joint.JointPos.x == entranceDirectionPos)
                    {
                        pickEntranceList.Add(joint);
                    }
                }

                entranceJoint = pickEntranceList[Random.Range(0, pickEntranceList.Count - 1)];
                exitJointPos = new Vector2Int(sectionData.GridWidth - 1, entranceJoint.JointPos.y);
                exitJoint = CreateJoint(sectionData, exitJointPos, JointType.SectionExit);

                BuildLoadHorizontal(sectionData, entranceDirectionPos, exitJointPos.x, exitJointPos.y);

                break;
            case ConnectDirection.Top:
                sectionData.AddConnectDirection(ConnectDirection.Top);
                entranceDirectionPos = sectionData.JointDataDict[JointType.Relay].Max(joint => joint.JointPos.y);

                foreach (Joint joint in sectionData.JointDataDict[JointType.Relay])
                {
                    if (joint.JointPos.y == entranceDirectionPos)
                    {
                        pickEntranceList.Add(joint);
                    }
                }

                entranceJoint = pickEntranceList[Random.Range(0, pickEntranceList.Count - 1)];
                exitJointPos = new Vector2Int(entranceJoint.JointPos.x, sectionData.GridWidth - 1);
                exitJoint = CreateJoint(sectionData, exitJointPos, JointType.SectionExit);

                BuildLoadVertical(sectionData, entranceDirectionPos, exitJointPos.y, exitJointPos.x);

                break;
            case ConnectDirection.Bottom:
                sectionData.AddConnectDirection(ConnectDirection.Bottom);
                entranceDirectionPos = sectionData.JointDataDict[JointType.Relay].Min(joint => joint.JointPos.y);

                foreach (Joint joint in sectionData.JointDataDict[JointType.Relay])
                {
                    if (joint.JointPos.y == entranceDirectionPos)
                    {
                        pickEntranceList.Add(joint);
                    }
                }

                entranceJoint = pickEntranceList[Random.Range(0, pickEntranceList.Count - 1)];
                exitJointPos = new Vector2Int(entranceJoint.JointPos.x, 0);
                exitJoint = CreateJoint(sectionData, exitJointPos, JointType.SectionExit);

                BuildLoadVertical(sectionData, exitJointPos.y, entranceDirectionPos, exitJointPos.x);

                break;
        }

        return exitJoint;
    }

    private static void ConnectExitHorizontal(SectionData sectionData, Joint startConnectJoint, Joint endConnectJoint)
    {
        if (startConnectJoint.JointPos.x < endConnectJoint.JointPos.x)
        {
            BuildLoadHorizontal(sectionData, startConnectJoint.JointPos.x, endConnectJoint.JointPos.x, endConnectJoint.JointPos.y);
        }
        else
        {
            BuildLoadHorizontal(sectionData, endConnectJoint.JointPos.x, startConnectJoint.JointPos.x, endConnectJoint.JointPos.y);
        }
    }

    private static void ConnectExitVertical(SectionData sectionData, Joint startConnectJoint, Joint endConnectJoint)
    {
        if (startConnectJoint.JointPos.y < endConnectJoint.JointPos.y)
        {
            BuildLoadVertical(sectionData, startConnectJoint.JointPos.y, endConnectJoint.JointPos.y, endConnectJoint.JointPos.x);
        }
        else
        {
            BuildLoadVertical(sectionData, endConnectJoint.JointPos.y, startConnectJoint.JointPos.y, endConnectJoint.JointPos.x);
        }
    }

    private static void BuildLoadHorizontal(SectionData sectionData, int startPosX, int endPosX, int posY)
    {
        for (int x = startPosX; x <= endPosX; x++)
        {
            sectionData.GridDataArr[x + (posY * sectionData.GridWidth)].SetGridData(TileType.Ground);
        }
    }

    private static void BuildLoadVertical(SectionData sectionData, int startPosY, int endPosY, int PosX)
    {
        for (int y = startPosY; y <= endPosY; y++)
        {
            sectionData.GridDataArr[PosX + (y * sectionData.GridWidth)].SetGridData(TileType.Ground);
        }
    }
}