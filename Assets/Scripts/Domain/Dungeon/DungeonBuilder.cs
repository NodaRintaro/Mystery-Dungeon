using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Linq;
using UnityEngine;

namespace Layer.Domain
{
    /// <summary>
    /// ダンジョン生成を担当するクラスです。
    /// セクション分割アルゴリズムに基づき、部屋と通路を生成します。
    /// </summary>
    public class DungeonBuilder
    {
        private DungeonData _dungeonData = null;
        private DungeonRoomData[] _canBuildData = null;

        private SectionBuilder _sectionBuilder = new();

        private const int _cantBuildRoomSpace = 4;

        /// <summary> ダンジョンの構築 </summary>
        /// <param name="sectionSize">セクションのサイズ（一辺のマス数）</param>
        /// <param name="horizontalSectionNum">横方向のセクション数</param>
        /// <param name="VerticalSectionNum">縦方向のセクション数</param>
        /// <param name="minRoomNum">最小の部屋数</param>
        /// <param name="maxRoomNum">最大の部屋数</param>
        /// <param name="addRoadNum">追加の通路数</param>
        /// <param name="roomDataPath">部屋データのパス</param>
        /// <returns>生成されたダンジョンデータ</returns>
        public async UniTask<DungeonData> BuildDungeon(int sectionSize, int horizontalSectionNum, int VerticalSectionNum, int minRoomNum, int maxRoomNum, int addRoadNum, string roomDataPath)
        {
            _dungeonData = new DungeonData();
            int maxRoomSize = sectionSize - _cantBuildRoomSpace;

            // 構築可能な部屋データを読み込み
            _canBuildData = await GetCanBuildRoomData(roomDataPath, maxRoomSize);

            // ダンジョンデータの初期化（セクション配列の作成）
            _dungeonData.InitDungeonData(horizontalSectionNum, VerticalSectionNum);

            // 各セクションの初期データを生成
            for (int x = 0; x < horizontalSectionNum; x++)
            {
                for (int y = 0; y < VerticalSectionNum; y++)
                {
                    _dungeonData.SetSectionData(_sectionBuilder.GetNewSection(sectionSize), x, y);
                }
            }

            // 部屋を配置するセクションを決定
            SelectBuildRoomSection(minRoomNum, maxRoomNum);

            // 決定したセクションに部屋、あるいは中継点を構築
            BuildRooms();

            // セクション間を接続する通路を構築
            BuildRoads(addRoadNum);

            return _dungeonData;
        }

        /// <summary> 指定されたパスから、サイズ制限に合う部屋データを取得します。</summary>
        private UniTask<DungeonRoomData[]> GetCanBuildRoomData(string roomDataPath, int maxRoomSize)
        {
            // Todo: AssetsLoaderなどを使用してデータを読み込む処理
            DungeonRoomData[] roomData = null;
            List<DungeonRoomData> canBuildRoomDataList = new List<DungeonRoomData>();

            // ここでは仮の実装。本来はロード処理が入る
            if (roomData != null)
            {
                foreach (var room in roomData)
                {
                    if (room.Height <= maxRoomSize && room.Width <= maxRoomSize)
                    {
                        canBuildRoomDataList.Add(room);
                    }
                }
            }

            return UniTask.FromResult(canBuildRoomDataList.ToArray());
        }

        /// <summary> 部屋を作成するセクションをランダムに選定します。 </summary>
        private void SelectBuildRoomSection(int minRoomNum, int maxRoomNum)
        {
            const int weight = 1;
            List<RandomPickItem<SectionData>> randomPickSection = new List<RandomPickItem<SectionData>>();      
            foreach (var item in _dungeonData.DungeonSectionsData)
            {
                RandomPickItem<SectionData> sectionData = new RandomPickItem<SectionData>(item, weight);        
                randomPickSection.Add(sectionData);
            }

            int targetRoomCount = UnityEngine.Random.Range(minRoomNum, maxRoomNum);
            for (int roomCount = 0; roomCount < targetRoomCount; roomCount++)  
            {
                if (randomPickSection.Count == 0) break;
                SectionData section = RandomPickItem.SelectRandomItem(randomPickSection, true);
                section.IsBuildRoom = true;
            }
        }

        /// <summary> 各セクションに部屋、あるいは中継点を生成します。 </summary>
        private void BuildRooms()
        {
            foreach (var data in _dungeonData.DungeonSectionsData)
            {
                if (data.IsBuildRoom)
                {
                    _sectionBuilder.BuildRoom(data, _canBuildData[UnityEngine.Random.Range(0, _canBuildData.Length)]);
                }
                else
                {
                    _sectionBuilder.BuildRelay(data);
                }
            }
        }

        /// <summary> セクション間を接続し、通路を生成します。 </summary>
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

            // ランダムに道路を追加
            RandomAddRoads(addRoadNum);
        }

        /// <summary> 接続されていないセクションをすべて接続 </summary>
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

        /// <summary> すべての通路の接続 </summary>
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

        /// <summary> 指定された数だけ、ランダムに通路を追加 </summary>
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

        /// <summary> 未接続のセクションから接続可能なセクションを探索し、通路を構築 </summary>
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

        /// <summary> 隣接するセクションの中から接続可能なものを探索 </summary>
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

        /// <summary> 接続可能なセクションのリストを取得 </summary>
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

        /// <summary> 指定されたセクションに接続されている通路の数を取得 </summary>
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

        /// <summary> 指定された座標が有効なセクションであり、接続条件を満たしているか確認 </summary>
        private bool TryGetValidSection(out Vector2Int validSection, int x, int y, bool isConnectOtherSection)  
        {
            int widthLength = _dungeonData.DungeonSectionsData.GetLength(0);
            int heightLength = _dungeonData.DungeonSectionsData.GetLength(1);

            if (x >= 0 && x < widthLength && y >= 0 && y < heightLength && _dungeonData.DungeonSectionsData[x, y].IsConnect == isConnectOtherSection)
            {
                validSection = new Vector2Int(x, y);
                return true;
            }

            validSection = default;
            return false;
        }

        /// <summary> 2つのセクション間に通路を構築 </summary>
        private void ConnectSections(Vector2Int baseConnectSectionPos, Vector2Int connectSectionPos)
        {
            SectionData baseSection = _dungeonData.DungeonSectionsData[baseConnectSectionPos.x, baseConnectSectionPos.y];
            SectionData connectSection = _dungeonData.DungeonSectionsData[connectSectionPos.x, connectSectionPos.y];
            _sectionBuilder.BuildRoad(baseSection, connectSection, baseConnectSectionPos, connectSectionPos);   
        }

        #region セクションビルダー
        /// <summary> セクション内の具体的なタイル配置（部屋、中継点、通路）を担当する内部クラス </summary>
        public class SectionBuilder
        {
            const int _cantBuildRoomSpace = 2;

            /// <summary> 新しいセクションデータを生成 </summary>
            public SectionData GetNewSection(int sectionSize)
            {
                SectionData sectionData = new SectionData();
                sectionData.InitSectionData(sectionSize);
                return sectionData;
            }

            /// <summary> セクション内に部屋を構築 </summary>
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

            /// <summary> セクション内に中継点（通路の曲がり角など）を構築 </summary>
            public void BuildRelay(SectionData sectionData)
            {
                Vector2Int jointPos = new Vector2Int
                {
                    x = Random.Range(_cantBuildRoomSpace, sectionData.GridWidth - _cantBuildRoomSpace),
                    y = Random.Range(_cantBuildRoomSpace, sectionData.GridWidth - _cantBuildRoomSpace)
                };

                CreateJoint(sectionData, jointPos, JointType.TurningPoint);
            }

            /// <summary> セクション間に通路を構築 </summary>
            public void BuildRoad(SectionData baseSection, SectionData connectSection, Vector2Int baseSectionPos, Vector2Int connectSectionPos)
            {
                Joint baseSectionExit;
                Joint connectSectionExit;

                switch (baseSectionPos)
                {
                    case var pos when pos.x < connectSectionPos.x:
                        baseSectionExit = MakeExit(baseSection, ConnectDirection.Right);
                        connectSectionExit = MakeExit(connectSection, ConnectDirection.Left);
                        ConnectExitVertical(connectSection, baseSectionExit, connectSectionExit);
                        break;

                    case var pos when pos.x > connectSectionPos.x:
                        baseSectionExit = MakeExit(baseSection, ConnectDirection.Left);
                        connectSectionExit = MakeExit(connectSection, ConnectDirection.Right);
                        ConnectExitVertical(connectSection, baseSectionExit, connectSectionExit);
                        break;

                    case var pos when pos.y < connectSectionPos.y:
                        baseSectionExit = MakeExit(baseSection, ConnectDirection.Top);
                        connectSectionExit = MakeExit(connectSection, ConnectDirection.Bottom);
                        ConnectExitHorizontal(connectSection, baseSectionExit, connectSectionExit);
                        break;

                    case var pos when pos.y > connectSectionPos.y:
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

                        entranceJoint = pickEntranceList[Random.Range(0, pickEntranceList.Count)];
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

                        entranceJoint = pickEntranceList[Random.Range(0, pickEntranceList.Count)];
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

                        entranceJoint = pickEntranceList[Random.Range(0, pickEntranceList.Count)];
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

                        entranceJoint = pickEntranceList[Random.Range(0, pickEntranceList.Count)];
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
}
