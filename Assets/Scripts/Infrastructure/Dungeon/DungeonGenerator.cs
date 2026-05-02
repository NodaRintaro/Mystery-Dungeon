using Domain;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Infrastructure
{
    /// <summary> Dungeon生成を行うクラス </summary>
    public class DungeonGenerator : MonoBehaviour
    {
        // 1グリッドの大きさ
        [Header("1グリッドの大きさ")]
        [SerializeField]
        private int _gridSize = 2;

        //各セクションの大きさ
        [Header("各セクションの大きさ")]
        [SerializeField] private int _sectionRange = 10;

        //ダンジョンに生成する部屋の個数の最大値と最小値
        [Header("ダンジョンに生成する部屋の最小値")]
        [SerializeField] private int _minRoomNum = 5;
        [Header("ダンジョンに生成する部屋の最大値")]
        [SerializeField] private int _maxRoomNum = 10;

        //生成するダンジョン全体のセクションの個数
        [Header("セクションの横の個数")]
        [SerializeField] private int _horizontalSectionsNum = 5;
        [Header("セクションの縦の個数")]
        [SerializeField] private int _verticalSectionsNum = 5;

        //生成するDungeonのYのポジション
        [Header("生成するDungeonのYのポジション")]
        [SerializeField] private int _groundheight = 0;

        //ダンジョンに追加で敷く道の本数
        [Header("ダンジョンに追加で敷く道の本数")]
        [SerializeField] private int _addRoadNum = 5;

        //部屋のデータの保存先のパス
        [Header("部屋のデータの保存先のパス")]
        [SerializeField] private string _roomDataPath = "DefaultDungeonRoom";

        [Header("Mapのデータパス")]
        [SerializeField] private string _mapDataPath = "Default";

        [Header("生成したObjectを保管するPoolClass")]
        [SerializeField] private DungeonTileObjectPool _tileObjectPool = new();

        private readonly DungeonBuilder _stageBuilder = new();



        #region ゲッター
        public int GridSize => _gridSize;
        public int SectionRange => _sectionRange;
        public int MinRoomNum => _minRoomNum;
        public int MaxRoomNum => _maxRoomNum;
        public int HorizontalSectionsNum => _horizontalSectionsNum;
        public int VerticalSectionsNum => _verticalSectionsNum;
        public int GroundHeight => _groundheight;
        public int addRoadNum => _addRoadNum;
        #endregion

        #region セッター
        public void SetSectionRange(int sectionRange) => _sectionRange = sectionRange;
        public void SetMinRoomNum(int minRoomNum) => _minRoomNum = minRoomNum;
        public void SetMaxRoomNum(int maxRoomNum) => _maxRoomNum = maxRoomNum;
        public void SetHorizontalSectionsNum(int horizontalSectionsNum) => _horizontalSectionsNum = horizontalSectionsNum;
        public void SetVerticalSectionsNum(int verticalSectionsNum) => _verticalSectionsNum = verticalSectionsNum;
        public void SetaddRoadNum(int addRoadNum) => _addRoadNum = addRoadNum;
        #endregion

        /// <summary> ダンジョンの生成を行う関数 </summary>
        public async UniTask<DungeonData> GenerateDungeon()
        {
            LoadStageTilePrefab();
            DungeonData dungeonData = await CreateStageData();

            for (int x = 0; x < _horizontalSectionsNum; x++)
            {
                for (int y = 0; y < _verticalSectionsNum; y++)
                {
                    Vector2Int generatePoints = new Vector2Int(x, y);
                    GenerateSection(dungeonData.DungeonSectionsData[x, y], generatePoints);
                }
            }

            return dungeonData;
        }

        /// <summary> ダンジョンを構成するタイルのPrefabをロードしてTileObjectPoolに保存する関数 </summary>
        private void LoadStageTilePrefab()
        {
            if (!ServiceLocator.TryGet(out AssetsLoader assetsLoader)) return;

            TileViewData[] tileDataArr = null;

            foreach (var tileData in tileDataArr)
            {
                _tileObjectPool.AddTileDict(tileData);
                Debug.Log(tileData.TileObject.name);
            }
        }

        /// <summary> ダンジョンのデータを構築する関数 </summary>
        private async UniTask<DungeonData> CreateStageData()
        {
            return await _stageBuilder.StageDungeon(_sectionRange, _horizontalSectionsNum, _verticalSectionsNum, _minRoomNum, _maxRoomNum, _addRoadNum, _roomDataPath);
        }

        /// <summary> タイルの種類に応じたタイルオブジェクトを生成する関数 </summary>
        private GameObject SpawnTileObj(TileType tileType)
        {
            if (_tileObjectPool.TileObjectDict.ContainsKey(tileType))
            {
                GameObject tileObject = _tileObjectPool.SpawnObject(_tileObjectPool.TileObjectDict[tileType]);
                return tileObject;
            }
            else
            {
                GameObject tileObject = _tileObjectPool.SpawnObject(_tileObjectPool.TileObjectDict[TileType.Wall]);
                return tileObject;
            }
        }

        /// <summary> セクションのデータと生成するポイントに応じてセクションを生成する関数 </summary>
        private void GenerateSection(SectionData sectionData, Vector2Int generatePoints)
        {
            Vector2Int firstGeneratePos = new Vector2Int
            {
                x = (generatePoints.x * _sectionRange) + _sectionRange,
                y = (generatePoints.y * _sectionRange) + _sectionRange
            };

            int gridPosCount = 0;

            foreach (var grid in sectionData.GridDataArr)
            {
                GameObject gridTileObj = SpawnTileObj(grid.TileType);

                if (gridTileObj != null)
                {
                    gridTileObj.transform.position = GetGenerateTilePosition(gridPosCount, firstGeneratePos);
                    gridTileObj.transform.parent = _tileObjectPool.DungeonTileParent.transform;
                }
                gridPosCount++;
            }
        }

        /// <summary> セクションのデータと生成するポイントに応じてタイルの生成位置を計算する関数 </summary>
        private Vector3Int GetGenerateTilePosition(int sectionsGridPos, Vector2Int leftBottomPos)
        {
            Vector3Int generatePos;

            if (sectionsGridPos < _sectionRange)
            {
                generatePos = new Vector3Int
                {
                    x = sectionsGridPos + leftBottomPos.x,
                    y = _groundheight,
                    z = leftBottomPos.y
                };
            }
            else
            {
                generatePos = new Vector3Int
                {
                    x = (sectionsGridPos % _sectionRange) + leftBottomPos.x,
                    y = _groundheight,
                    z = (sectionsGridPos / _sectionRange) + leftBottomPos.y
                };
            }

            generatePos.x *= GridSize;
            generatePos.z *= GridSize;
            return generatePos;
        }

        /// <summary> ダンジョンの削除を行う関数 </summary>
        private void DeleteDungeon()
        {
            _tileObjectPool.ReleaseAllObjects();
        }
    }
}





