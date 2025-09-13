using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class SectionData
{
    /// <summary> Section内部の部屋の有無 </summary>
    private bool _isBuildRoom = false;

    /// <summary> Section内部のGridのデータ </summary>
    private GridData[] _gridData;



    private int _gridArrRange;

    public int GridArrRange => _gridArrRange;

    public bool IsBuildRoom 
    { 
        get { return _isBuildRoom; } 
        set { _isBuildRoom = value; }
    }

    public GridData[] GridData => _gridData;

    /// <summary> SctionDataの初期化 </summary>
    public void InitSectionData(int arrIndex)
    {
        int totalIndexNum = arrIndex * arrIndex;
        _gridArrRange = arrIndex;
        _gridData = new GridData[totalIndexNum];
    }
}

public struct GridData
{
    private TileType _tileType;
    public TileType TileType => _tileType;

    public void SetGridData(TileType tileType)
    {
        _tileType = tileType;
    }
}

public class Joint
{
    private Vector2Int _relayPos;

    public Vector2Int RelayPos => _relayPos;

    public void SetRelay(Vector2Int relayPos) => _relayPos = relayPos;
}

public enum TileType
{
    Empty,
    Wall,
    Ground,
}