using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

namespace Domain
{
    #region ダンジョン全体の生成クラス
    /// <summary> ランダムな地形のダンジョンを自動生成するためのロジッククラス </summary>
    public class DungeonBuilder
    {
        private DungeonData _dungeonData = null;
        private DungeonRoomData[] _canBuildData = null;

        private SectionBuilder _sectionBuilder = new();

        private const int _cantBuildRoomSpace = 4;

        /// <summary> Dungeonの自動生成ロジック </summary>
        /// <param name="sectionSize"> 生成するダンジョンのセクションの大きさ </param>
        /// <param name="horizontalSectionNum"> 横に生成するセクションの個数 </param>
        /// <param name="VerticalSectionNum"> 縦に生成するセクションの個数 </param>
        /// <param name="minRoomNum"> 生成する部屋の最小個数 </param>
        /// <param name="maxRoomNum"> 生成する部屋の最大個数 </param>
        /// <param name="roomDataPath"> 部屋のDataのPath </param>
        /// <returns> DungeonData </returns>
        public async UniTask<DungeonData> StageDungeon(int sectionSize, int horizontalSectionNum, int VerticalSectionNum, int minRoomNum, int maxRoomNum, int addRoadNum, string roomDataPath)
        {
            _dungeonData = new DungeonData();
            int maxRoomSize = sectionSize - _cantBuildRoomSpace;

            //部屋のデータを取得
            _canBuildData = await GetCanBuildRoomData(roomDataPath, maxRoomSize);

            //ダンジョンのデータを初期化
            _dungeonData.InitDungeonData(horizontalSectionNum, VerticalSectionNum);

            //各セクションのGridDataを構築
            for (int x = 0; x < horizontalSectionNum; x++)
            {
                for (int y = 0; y < VerticalSectionNum; y++)
                {
                    _dungeonData.SetSectionData(_sectionBuilder.GetNewSection(sectionSize), x, y);
                }
            }

            //部屋を作るセクションを選択
            SelectBuildRoomSection(minRoomNum, maxRoomNum);

            //実際にセクション内部に部屋を作る
            BuildRooms();

            //部屋全体を繋げる道を作る
            BuildRoads(addRoadNum);

            return _dungeonData;
        }

        /// <summary> 生成可能な部屋のデータを取得する処理 </summary>
        /// <param name="roomDataPath">部屋のデータが保存されている場所のパス</param>
        /// <param name="maxRoomSize"> 生成可能な部屋の最大サイズ </param>
        private UniTask<DungeonRoomData[]> GetCanBuildRoomData(string roomDataPath, int maxRoomSize)
        {
            //Todo: AssetsLoaderを使う
            DungeonRoomData[] roomData = null;
            List<DungeonRoomData> canBuildRoomDataList = new List<DungeonRoomData>();

            foreach (var room in roomData)
            {
                if (room.Height <= maxRoomSize && room.Width <= maxRoomSize)
                {
                    canBuildRoomDataList.Add(room);
                }
            }

            Debug.Log(canBuildRoomDataList.Count);
            return UniTask.FromResult(canBuildRoomDataList.ToArray());
        }

        /// <summary> ランダムに各セクションから部屋を作るセクションを決める </summary>
        /// <param name="minRoomNum"> 作る部屋の最小個数 </param>
        /// <param name="maxRoomNum"> 作る部屋の最大個数 </param>
        private void SelectBuildRoomSection(int minRoomNum, int maxRoomNum)
        {
            const int weight = 1;
            List<RandomPickItem<SectionData>> randomPickSection = new List<RandomPickItem<SectionData>>();
            foreach (var item in _dungeonData.DungeonSectionsData)
            {
                RandomPickItem<SectionData> sectionData = new RandomPickItem<SectionData>(item, weight);
                randomPickSection.Add(sectionData);
            }

            for (int roomCount = 0; roomCount < UnityEngine.Random.Range(minRoomNum, maxRoomNum); roomCount++)
            {
                SectionData section = RandomPickItem.SelectRandomItem(randomPickSection, true);
                section.IsBuildRoom = true;
            }
        }

        /// <summary> 各セクションに部屋か中継地点を配置する </summary>
        private void BuildRooms()
        {
            foreach (var data in _dungeonData.DungeonSectionsData)
            {
                if (data.IsBuildRoom)
                {
                    _sectionBuilder.BuildRoom(data, _canBuildData[UnityEngine.Random.Range(0, _canBuildData.Length - 1)]);
                }
                else
                {
                    _sectionBuilder.BuildRelay(data);
                }
            }
        }

        /// <summary> 各部屋と中継地点がつながるように道を生成 </summary>
        /// <param name="addRoadNum"> つなぎ終わった後にダンジョンを難解化するために付け足す道の本数 </param>
        private void BuildRoads(int addRoadNum)
        {
            Vector2Int connectSectionIndex = new Vector2Int(0, 0);
            Vector2Int saveCurrentSectionIndex = connectSectionIndex;
            _dungeonData.DungeonSectionsData[connectSectionIndex.x, connectSectionIndex.y].IsConnect = true;

            while (TrySearchCanConnectSection(ref connectSectionIndex, false))
            {
                _dungeonData.DungeonSectionsData[connectSectionIndex.x, connectSectionIndex.y].IsConnect = true;
                ConnectSections(saveCurrentSectionIndex, connectSectionIndex);
                saveCurrentSectionIndex = connectSectionIndex;
            }

            ConnectAllSections();
            ConnectAllRoads();

            //追加で何本か道を敷いておく
            RandomAddRoads(addRoadNum);
        }

        /// <summary> すべてのセクションをつなげる処理 </summary>
        private void ConnectAllSections()
        {
            int widthLength = _dungeonData.DungeonSectionsData.GetLength(0);
            int heightLength = _dungeonData.DungeonSectionsData.GetLength(1);

            for (int x = 0; x < widthLength; x++)
            {
                for (int y = 0; y < heightLength; y++)
                {
                    Vector2Int checkConnectSectionIndex = new Vector2Int(x, y);

                    if (!_dungeonData.DungeonSectionsData[x, y].IsConnect)
                    {
                        BuildRoadUnconnectSections(checkConnectSectionIndex);
                    }
                }
            }
        }

        /// <summary> すべての道をつなげる処理 </summary>
        private void ConnectAllRoads()
        {
            const int minRoadNum = 2;
            int widthLength = _dungeonData.DungeonSectionsData.GetLength(0);
            int heightLength = _dungeonData.DungeonSectionsData.GetLength(1);

            for (int x = 0; x < widthLength; x++)
            {
                for (int y = 0; y < heightLength; y++)
                {
                    Vector2Int sectionIndex = new Vector2Int(x, y);

                    if (!_dungeonData.DungeonSectionsData[x, y].IsBuildRoom && GetConnectRoadNum(sectionIndex) <= minRoadNum)
                    {
                        Vector2Int saveSectionIndex = sectionIndex;
                        TrySearchCanConnectSection(ref sectionIndex, true);
                        ConnectSections(saveSectionIndex, sectionIndex);
                    }
                }
            }
        }

        /// <summary> ランダムに追加で新しい道を生成する処理 </summary>
        private void RandomAddRoads(int addRoadNum)
        {
            int widthLength = _dungeonData.DungeonSectionsData.GetLength(0);
            int heightLength = _dungeonData.DungeonSectionsData.GetLength(1);

            for (int addRoadCount = 0; addRoadCount < addRoadNum; addRoadCount++)
            {
                Vector2Int addRoadSectionIndex =
                    new Vector2Int
                    {
                        x = Random.Range(0, widthLength),
                        y = Random.Range(0, heightLength)
                    };

                Vector2Int saveIndex = addRoadSectionIndex;
                TrySearchCanConnectSection(ref addRoadSectionIndex, true);
                ConnectSections(addRoadSectionIndex, saveIndex);
            }
        }

        /// <summary> 接続のされていないセクションに新たに道を作る </summary>
        /// <param name="connectSectionIndex"> 道を作るスタート地点 </param>
        private void BuildRoadUnconnectSections(Vector2Int connectSectionIndex)
        {
            Vector2Int saveConnectIndex = connectSectionIndex;
            _dungeonData.DungeonSectionsData[connectSectionIndex.x, connectSectionIndex.y].IsConnect = true;

            if (TrySearchCanConnectSection(ref connectSectionIndex, true))
            {
                ConnectSections(saveConnectIndex, connectSectionIndex);
            }
            else
            {
                if (TrySearchCanConnectSection(ref connectSectionIndex))
                {
                    ConnectSections(saveConnectIndex, connectSectionIndex);
                    BuildRoadUnconnectSections(connectSectionIndex);
                }
            }
        }

        /// <summary> 道による接続が可能なセクションを探しなければfalseを返す </summary>
        private bool TrySearchCanConnectSection(ref Vector2Int connectSectionIndex, bool isConnectOther = false)
        {
            bool found = false;
            List<Vector2Int> canConnectSections = GetCanConnectSections(connectSectionIndex, isConnectOther);

            if (canConnectSections.Count > 0)
            {
                connectSectionIndex = canConnectSections[Random.Range(0, canConnectSections.Count)];
                found = true;
            }

            return found;
        }

        /// <summary> 道の接続が可能なセクションを探索 </summary>
        private List<Vector2Int> GetCanConnectSections(Vector2Int connectSectionIndex, bool isConnectOther)
        {
            const int shiftArrPos = 1;
            List<Vector2Int> canConnectSections = new();
            SectionData sectionData = _dungeonData.DungeonSectionsData[connectSectionIndex.x, connectSectionIndex.y];

            foreach (var connectDirection in sectionData.IsConnectDirectionDict)
            {
                Vector2Int connectSection = default;

                if (!connectDirection.Value)
                {
                    switch (connectDirection.Key)
                    {
                        case ConnectDirection.Top:
                            if(TryGetValidSection(out connectSection, connectSectionIndex.x, connectSectionIndex.y + shiftArrPos, isConnectOther))
                            {
                                canConnectSections.Add(connectSection);
                            }
                            break;
                        case ConnectDirection.Bottom:
                            if (TryGetValidSection(out connectSection, connectSectionIndex.x, connectSectionIndex.y - shiftArrPos, isConnectOther))
                            {
                                canConnectSections.Add(connectSection);
                            }
                            break;
                        case ConnectDirection.Left:
                            if (TryGetValidSection(out connectSection, connectSectionIndex.x - shiftArrPos, connectSectionIndex.y, isConnectOther))
                            {
                                canConnectSections.Add(connectSection);
                            }
                            break;
                        case ConnectDirection.Right:
                            if (TryGetValidSection(out connectSection, connectSectionIndex.x + shiftArrPos, connectSectionIndex.y, isConnectOther))
                            {
                                canConnectSections.Add(connectSection);
                            }
                            break;
                    }
                }
            }

            return canConnectSections;
        }

        /// <summary> 現在接続しているセクションに対して、上下左右でいくつ道で繋がっているセクションがあるかを数える </summary>
        /// <param name="checkSectionIndex">調べたいセクションの要素番号</param>
        /// <returns> つながっている道の本数 </returns>
        private int GetConnectRoadNum(Vector2Int checkSectionIndex)
        {
            int roadNum = 0;
            SectionData sectionData = _dungeonData.DungeonSectionsData[checkSectionIndex.x, checkSectionIndex.y];

            foreach (var connectDirection in sectionData.IsConnectDirectionDict)
            {
                if (connectDirection.Value)
                {
                    roadNum++;
                }
            }

            return roadNum;
        }

        /// <summary> 接続の有効なセクションか調べる </summary>
        private bool TryGetValidSection(out Vector2Int validSection, int x, int y, bool isConnectOtherSection)
        {
            List<Vector2Int> validSectionlist = new List<Vector2Int>();

            int widthLength = _dungeonData.DungeonSectionsData.GetLength(0);
            int heightLength = _dungeonData.DungeonSectionsData.GetLength(1);

            if (x >= 0 && x < widthLength && y >= 0 && y < heightLength && _dungeonData.DungeonSectionsData[x, y].IsConnect == isConnectOtherSection)
            {
                validSection = new Vector2Int(x, y);
                return true;
            }

            validSection = default;
            return default;
        }

        /// <summary> セクション同士をつなぐ処理 </summary>
        private void ConnectSections(Vector2Int baseConnectSectionPos, Vector2Int connectSectionPos)
        {
            SectionData baseSection = _dungeonData.DungeonSectionsData[baseConnectSectionPos.x, baseConnectSectionPos.y];
            SectionData connectSection = _dungeonData.DungeonSectionsData[connectSectionPos.x, connectSectionPos.y];
            _sectionBuilder.BuildRoad(baseSection, connectSection, baseConnectSectionPos, connectSectionPos);
        }
    }
    #endregion

    #region Map内の各セクション生成クラス
    /// <summary> Section内部の細かい設定をするロジック </summary>
    public class SectionBuilder
    {
        const int _cantBuildRoomSpace = 2;

        /// <summary> 新たにセクションを生成 </summary>
        /// <param name="sectionSize">セクションの大きさ</param>
        public SectionData GetNewSection(int sectionSize)
        {
            SectionData sectionData = new SectionData();
            sectionData.InitSectionData(sectionSize);
            return sectionData;
        }

        /// <summary> セクション内部に部屋を生成する処理 </summary>
        /// <param name="sectionData"> 部屋を生成するセクション </param>
        /// <param name="roomData"> 生成する部屋のデータ </param>
        public void BuildRoom(SectionData sectionData, DungeonRoomData roomData)
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
                for (int y = randomStartPos.y; y < roomData.Height + randomStartPos.y; y++)
                {
                    int roomGridPos = (y - randomStartPos.y) * roomData.Width + (x - randomStartPos.x);

                    sectionData.GridDataArr[x + (y * sectionData.GridWidth)].SetTileType(roomData.GridRoomData[roomGridPos]);

                    if ((x == randomStartPos.x || y == randomStartPos.y) && sectionData.GetGridData(x, y).TileType == TileType.Ground)
                    {
                        CreateJoint(sectionData, new Vector2Int(x, y), JointType.TurningPoint);
                    }
                    else if ((x == topPosX || y == topPosY) && sectionData.GetGridData(x, y).TileType == TileType.Ground)
                    {
                        CreateJoint(sectionData, new Vector2Int(x, y), JointType.TurningPoint);
                    }
                }
            }
        }

        /// <summary> セクション内部に道の分岐点を生成する処理 </summary>
        /// <param name="sectionData"> 分岐点を生成するセクション </param>
        public void BuildRelay(SectionData sectionData)
        {
            Vector2Int jointPos = new Vector2Int
            {
                x = Random.Range(_cantBuildRoomSpace, sectionData.GridWidth - _cantBuildRoomSpace),
                y = Random.Range(_cantBuildRoomSpace, sectionData.GridWidth - _cantBuildRoomSpace)
            };

            CreateJoint(sectionData, jointPos, JointType.TurningPoint);
        }

        /// <summary> 2つのセクション間に道を作りつなげる処理 </summary>
        /// <param name="baseSection"> 道を作り始めるセクション </param>
        /// <param name="connectSection"> 道をつなげたいセクション </param>
        /// <param name="baseSectionPos">  </param>
        /// <param name="connectSectionPos"></param>
        public void BuildRoad(SectionData baseSection, SectionData connectSection, Vector2Int baseSectionPos, Vector2Int connectSectionPos)
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

        private Joint CreateJoint(SectionData sectionData, Vector2Int jointPos, JointType jointType)
        {
            if (sectionData.GetGridData(jointPos.x, jointPos.y).TileType != TileType.Ground)
            {
                sectionData.GridDataArr[jointPos.x + (jointPos.y * sectionData.GridWidth)].SetTileType(TileType.Ground);
            }

            Joint joint = new Joint { JointPos = jointPos };
            sectionData.AddJoint(jointType, joint);

            return joint;
        }

        private Joint MakeExit(SectionData sectionData, ConnectDirection direction)
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
                    entranceDirectionPos = sectionData.JointDataDict[JointType.TurningPoint].Min(joint => joint.JointPos.x);

                    foreach (Joint joint in sectionData.JointDataDict[JointType.TurningPoint])
                    {
                        if (joint.JointPos.x == entranceDirectionPos)
                        {
                            pickEntranceList.Add(joint);
                        }
                    }

                    entranceJoint = pickEntranceList[Random.Range(0, pickEntranceList.Count - 1)];
                    exitJointPos = new Vector2Int(0, entranceJoint.JointPos.y);
                    exitJoint = CreateJoint(sectionData, exitJointPos, JointType.SectionExit);

                    BuildRoadHorizontal(sectionData, exitJointPos.x, entranceDirectionPos, exitJointPos.y);

                    break;
                case ConnectDirection.Right:
                    sectionData.AddConnectDirection(ConnectDirection.Right);
                    entranceDirectionPos = sectionData.JointDataDict[JointType.TurningPoint].Max(joint => joint.JointPos.x);

                    foreach (Joint joint in sectionData.JointDataDict[JointType.TurningPoint])
                    {
                        if (joint.JointPos.x == entranceDirectionPos)
                        {
                            pickEntranceList.Add(joint);
                        }
                    }

                    entranceJoint = pickEntranceList[Random.Range(0, pickEntranceList.Count - 1)];
                    exitJointPos = new Vector2Int(sectionData.GridWidth - 1, entranceJoint.JointPos.y);
                    exitJoint = CreateJoint(sectionData, exitJointPos, JointType.SectionExit);

                    BuildRoadHorizontal(sectionData, entranceDirectionPos, exitJointPos.x, exitJointPos.y);

                    break;
                case ConnectDirection.Top:
                    sectionData.AddConnectDirection(ConnectDirection.Top);
                    entranceDirectionPos = sectionData.JointDataDict[JointType.TurningPoint].Max(joint => joint.JointPos.y);

                    foreach (Joint joint in sectionData.JointDataDict[JointType.TurningPoint])
                    {
                        if (joint.JointPos.y == entranceDirectionPos)
                        {
                            pickEntranceList.Add(joint);
                        }
                    }

                    entranceJoint = pickEntranceList[Random.Range(0, pickEntranceList.Count - 1)];
                    exitJointPos = new Vector2Int(entranceJoint.JointPos.x, sectionData.GridWidth - 1);
                    exitJoint = CreateJoint(sectionData, exitJointPos, JointType.SectionExit);

                    BuildRoadVertical(sectionData, entranceDirectionPos, exitJointPos.y, exitJointPos.x);

                    break;
                case ConnectDirection.Bottom:
                    sectionData.AddConnectDirection(ConnectDirection.Bottom);
                    entranceDirectionPos = sectionData.JointDataDict[JointType.TurningPoint].Min(joint => joint.JointPos.y);

                    foreach (Joint joint in sectionData.JointDataDict[JointType.TurningPoint])
                    {
                        if (joint.JointPos.y == entranceDirectionPos)
                        {
                            pickEntranceList.Add(joint);
                        }
                    }

                    entranceJoint = pickEntranceList[Random.Range(0, pickEntranceList.Count - 1)];
                    exitJointPos = new Vector2Int(entranceJoint.JointPos.x, 0);
                    exitJoint = CreateJoint(sectionData, exitJointPos, JointType.SectionExit);

                    BuildRoadVertical(sectionData, exitJointPos.y, entranceDirectionPos, exitJointPos.x);

                    break;
            }

            return exitJoint;
        }

        private void ConnectExitHorizontal(SectionData sectionData, Joint startConnectJoint, Joint endConnectJoint)
        {
            if (startConnectJoint.JointPos.x < endConnectJoint.JointPos.x)
            {
                BuildRoadHorizontal(sectionData, startConnectJoint.JointPos.x, endConnectJoint.JointPos.x, endConnectJoint.JointPos.y);
            }
            else
            {
                BuildRoadHorizontal(sectionData, endConnectJoint.JointPos.x, startConnectJoint.JointPos.x, endConnectJoint.JointPos.y);
            }
        }

        private void ConnectExitVertical(SectionData sectionData, Joint startConnectJoint, Joint endConnectJoint)
        {
            if (startConnectJoint.JointPos.y < endConnectJoint.JointPos.y)
            {
                BuildRoadVertical(sectionData, startConnectJoint.JointPos.y, endConnectJoint.JointPos.y, endConnectJoint.JointPos.x);
            }
            else
            {
                BuildRoadVertical(sectionData, endConnectJoint.JointPos.y, startConnectJoint.JointPos.y, endConnectJoint.JointPos.x);
            }
        }

        private void BuildRoadHorizontal(SectionData sectionData, int startPosX, int endPosX, int posY)
        {
            for (int x = startPosX; x <= endPosX; x++)
            {
                sectionData.GridDataArr[x + (posY * sectionData.GridWidth)].SetTileType(TileType.Ground);
            }
        }

        private void BuildRoadVertical(SectionData sectionData, int startPosY, int endPosY, int PosX)
        {
            for (int y = startPosY; y <= endPosY; y++)
            {
                sectionData.GridDataArr[PosX + (y * sectionData.GridWidth)].SetTileType(TileType.Ground);
            }
        }
    }
    #endregion
}





