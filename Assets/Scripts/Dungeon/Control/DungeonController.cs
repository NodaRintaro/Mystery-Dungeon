using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

/// <summary> ダンジョン生成を行うクラス </summary>
public class DungeonController : MonoBehaviour
{
    //Gridの大きさ
    [Header("Gridの大きさ")]
    [SerializeField] private int _gridSize = 2;

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

    //部屋のデータの保存先のパス
    [Header("部屋のデータの保存先のパス")]
    [SerializeField] private string _roomDataPath = "DefaultDungeonRoom";

    [Header("Mapのデータパス")]
    [SerializeField] private string _mapDataPath = "Default";

    [Header("生成したObjectを管理するViewClass")]
    [SerializeField] private DungeonObjectHolder _dungeonObjectHolder = new();

    /// <summary> 生成したダンジョンのデータ </summary>
    private DungeonData _dungeonData = null;


    public int SectionRange => _sectionRange;
    public int MinRoomNum => _minRoomNum;
    public int MaxRoomNum => _maxRoomNum;
    public int HorizontalSectionsNum => _horizontalSectionsNum;
    public int VerticalSectionsNum => _verticalSectionsNum;
    public DungeonData DungeonData => _dungeonData;

    public void SetSectionRange(int sectionRange) => _sectionRange = sectionRange;
    public void SetMinRoomNum(int minRoomNum) => _minRoomNum = minRoomNum;
    public void SetMaxRoomNum(int maxRoomNum) => _maxRoomNum = maxRoomNum;
    public void SetHorizontalSectionsNum(int horizontalSectionsNum) => _horizontalSectionsNum = horizontalSectionsNum;
    public void SetVerticalSectionsNum(int verticalSectionsNum) => _verticalSectionsNum = verticalSectionsNum;

    public async void Awake()
    {
        GenerateDungeon();
    }

    public async void GenerateDungeon()
    {
        await LoadDungeonObjPrefab();
        await BuildDungeonData();

        for (int x = 0; x < _horizontalSectionsNum; x++)
        {
            for (int y = 0; y < _verticalSectionsNum; y++) 
            { 
                Vector2Int generatePoints = new Vector2Int(x, y);
                GenerateSection(_dungeonData.SectionDataArray[x, y], generatePoints);
            }
        }
    }

    private async UniTask LoadDungeonObjPrefab()
    {
        TileData[] tileDataArr = Resources.LoadAll<TileData>("MapTile/" + _mapDataPath);
        foreach(var tileData in tileDataArr)
        {
            _dungeonObjectHolder.AddTileDict(tileData);
            Debug.Log(tileData.TileObject.name);
        }
    }

    private async UniTask BuildDungeonData()
    {
        _dungeonData = await DungeonBuilder.DungeonBuild(_sectionRange, _horizontalSectionsNum, _verticalSectionsNum, _minRoomNum, _maxRoomNum, _roomDataPath);
    }

    private GameObject SpawnTileObj(TileType tileType)
    {
        if (_dungeonObjectHolder.TileObjectDict.ContainsKey(tileType))
        {
            GameObject tileObject = _dungeonObjectHolder.DungeonTilePool.SpawnObject(_dungeonObjectHolder.TileObjectDict[tileType]);
            return tileObject;
        }
        return null;
    }

    private void GenerateSection(SectionData sectionData, Vector2Int generatePoints)
    {
        Vector2Int firstGeneratePos = new Vector2Int
        {
            x = (generatePoints.x * _sectionRange) + _sectionRange,
            y = (generatePoints.y * _sectionRange) + _sectionRange
        };

        int gridPosCount = 0;

        foreach (var grid in sectionData.GridData)
        {
            GameObject gridTileObj = SpawnTileObj(grid.TileType);
            
            if (gridTileObj != null)
            {
                gridTileObj.transform.position = GenerateTilePosition(gridPosCount, firstGeneratePos);
                //gridTileObj.transform.parent = _dungeonObjectHolder.TileHolderObj.transform;
            }
            gridPosCount++;
        }
    }

    private Vector3Int GenerateTilePosition(int sectionsGridPos, Vector2Int leftBottomPos)
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
            Debug.Log("b");
        }
        else
        {
            generatePos = new Vector3Int
            {
                x = (sectionsGridPos % _sectionRange) + leftBottomPos.x,
                y = _groundheight,
                z = (sectionsGridPos / _sectionRange) + leftBottomPos.y
            };
            Debug.Log("a");
        }

        generatePos.x *= _gridSize;
        generatePos.z *= _gridSize;
        return generatePos;
    }
}

