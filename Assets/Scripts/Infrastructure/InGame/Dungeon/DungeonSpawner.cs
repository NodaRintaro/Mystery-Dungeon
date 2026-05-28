using Cysharp.Threading.Tasks;
using UnityEngine;
using Layer.Domain;

namespace Layer.Infrastructure
{
    /// <summary> ダンジョンの生成プロセスを制御し、実際にオブジェクトを配置するクラスです。 </summary>
    public class DungeonSpawner : MonoBehaviour
    {
        // 1グリッドあたりのサイズ
        [Header("1グリッドのサイズ")]
        [SerializeField]
        private int _gridSize = 2;

        // ダンジョンが生成される高さ（Y座標）
        [Header("ダンジョンの生成高度")]
        [SerializeField] private int _groundheight = 0;

        // 生成されたタイルオブジェクトを管理するプールクラス
        [Header("生成したダンジョンタイルのオブジェクトプール")]
        [SerializeField] private DungeonTileObjectPool _tileObjectPool = new();

        // ダンジョンのデータ生成クラス
        private DungeonBuilder _dungeonBuilder = new();

        private DungeonDataRepositry _dungeonDataRepositry = new();

        /// <summary> ダンジョンの生成メソッド </summary>
        public async UniTask<DungeonData> Spawn(DungeonCondition dungeonCondition)
        {
            LoadStageTilePrefab();
            DungeonData dungeonData = await CreateStageData(dungeonCondition);

            for (int x = 0; x < dungeonCondition.HorizontalSectionsNum; x++)
            {
                for (int y = 0; y < dungeonCondition.VerticalSectionsNum; y++)
                {
                    Vector2Int generatePoints = new Vector2Int(x, y);
                    GenerateSection(dungeonData.DungeonSectionsData[x, y], generatePoints, dungeonCondition.SectionRange);
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
        private async UniTask<DungeonData> CreateStageData(DungeonCondition dungeonCondition)
        {
            return null;
            //return await _dungeonBuilder.BuildDungeon(dungeonCondition.SectionRange, dungeonCondition.HorizontalSectionsNum, dungeonCondition.VerticalSectionsNum, dungeonCondition.MinRoomNum, dungeonCondition.MaxRoomNum, dungeonCondition.AddRoadNum, dungeonCondition.TileDataPath);
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
        private void GenerateSection(SectionData sectionData, Vector2Int generatePoints, int sectionRange)
        {
            Vector2Int firstGeneratePos = new Vector2Int
            {
                x = (generatePoints.x * sectionRange) + sectionRange,
                y = (generatePoints.y * sectionRange) + sectionRange
            };

            int gridPosCount = 0;

            foreach (var grid in sectionData.GridDataArr)
            {
                GameObject gridTileObj = SpawnTileObj(grid.TileType);

                if (gridTileObj != null)
                {
                    gridTileObj.transform.position = GetGenerateTilePosition(gridPosCount, firstGeneratePos, sectionRange);   
                    gridTileObj.transform.parent = _tileObjectPool.DungeonTileParent.transform;
                }
                gridPosCount++;
            }
        }

        /// <summary> グリッド内のインデックスから、ワールド座標でのタイル配置位置を算出します。 </summary>
        private Vector3Int GetGenerateTilePosition(int sectionsGridPos, Vector2Int leftBottomPos, int sectionRange)
        {
            Vector3Int generatePos;

            if (sectionsGridPos < sectionRange)
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
                    x = (sectionsGridPos % sectionRange) + leftBottomPos.x,
                    y = _groundheight,
                    z = (sectionsGridPos / sectionRange) + leftBottomPos.y
                };
            }

            generatePos.x *= _gridSize;
            generatePos.z *= _gridSize;
            return generatePos;
        }
    }
}
