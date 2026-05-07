using Cysharp.Threading.Tasks;
using UnityEngine;
using Layer.Domain;

namespace Layer.Infrastructure
{
    /// <summary> ダンジョンの生成プロセスを制御し、実際にオブジェクトを配置するクラスです。 </summary>
    public class DungeonGenerator : MonoBehaviour
    {
        // 1グリッドあたりのサイズ
        [Header("1グリッドのサイズ")]
        [SerializeField]
        private int _gridSize = 2;

        // 各セクションのサイズ（一辺の長さ）
        [Header("各セクションのサイズ")]
        [SerializeField] private int _sectionRange = 10;

        // 生成する部屋の最小・最大数
        [Header("部屋の最小生成数")]
        [SerializeField] private int _minRoomNum = 5;
        [Header("部屋の最大生成数")]
        [SerializeField] private int _maxRoomNum = 10;

        // ダンジョンのセクション分割数（横・縦）
        [Header("横方向のセクション数")]
        [SerializeField] private int _horizontalSectionsNum = 5;
        [Header("縦方向のセクション数")]
        [SerializeField] private int _verticalSectionsNum = 5;

        // ダンジョンが生成される高さ（Y座標）
        [Header("ダンジョンの生成高度")]
        [SerializeField] private int _groundheight = 0;

        // 基本の連結以外に追加で生成する通路の数
        [Header("追加の通路数")]
        [SerializeField] private int _addRoadNum = 5;

        // 部屋データおよびマップデータのロードパス
        [Header("部屋データのパス")]
        [SerializeField] private string _roomDataPath = "DefaultDungeonRoom";

        [Header("マップデータのパス")]
        [SerializeField] private string _mapDataPath = "Default";

        // 生成されたタイルオブジェクトを管理するプールクラス
        [Header("オブジェクトプール管理クラス")]
        [SerializeField] private DungeonTileObjectPool _tileObjectPool = new();

        private DungeonBuilder _dungeonBuilder = new();

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

        /// <summary> ダンジョンの生成プロセスを実行します。 </summary>
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

        /// <summary> ステージで使用するタイルのPrefabをロードし、タイルオブジェクトプールに登録します。 </summary>
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

        /// <summary> ダンジョン生成に必要な構成データを構築します。 </summary>
        private async UniTask<DungeonData> CreateStageData()
        {
            return await _dungeonBuilder.BuildDungeon(_sectionRange, _horizontalSectionsNum, _verticalSectionsNum, _minRoomNum, _maxRoomNum, _addRoadNum, _roomDataPath);
        }

        /// <summary> 指定されたタイルの種類に応じて、プールからオブジェクトを生成します。 </summary>   
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

        /// <summary> セクションデータに基づき、タイルを配置してセクションを構築します。 </summary>
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

        /// <summary> グリッド内のインデックスから、ワールド座標でのタイル配置位置を算出します。 </summary>
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

        /// <summary> ダンジョンのオブジェクトを破棄し、プールをクリアします。 </summary>
        private void DeleteDungeon()
        {
            _tileObjectPool.ReleaseAllObjects();
        }
    }
}
