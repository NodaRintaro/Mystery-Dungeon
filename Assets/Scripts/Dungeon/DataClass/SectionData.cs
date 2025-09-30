using UnityEngine;
using System.Collections.Generic;
using System;

public class SectionData
{
    /// <summary> Section内部の部屋の有無 </summary>
    private bool _isBuildRoom = false;

    /// <summary> このセクションがほかのセクションとつながっているかの判定 </summary>
    private bool _isConnect = false;

    /// <summary> Section内部のGridのデータ </summary>
    private GridData[] _gridDataArr;

    /// <summary> Sectionのどの方向に道が伸びているかの判定の保存先 </summary>
    private Dictionary<ConnectDirection, bool> _isConnectDirectionDict = new();

    /// <summary> Section内部のJointの保存先 </summary>
    private Dictionary<JointType, List<Joint>> _jointDataDict = new();

    /// <summary> GridDataの1列の長さ </summary>
    private int _gridWidth;

    public bool IsBuildRoom 
    { 
        get { return _isBuildRoom; }
        set { _isBuildRoom = value; }
    }

    public bool IsConnect
    {
        get { return _isConnect; }
        set { _isConnect = value; }
    }
    
    public Dictionary<ConnectDirection, bool> IsConnectDirectionDict => _isConnectDirectionDict;

    public Dictionary<JointType, List<Joint>> JointDataDict => _jointDataDict;

    public GridData[] GridDataArr => _gridDataArr;

    public int GridWidth => _gridWidth;

    /// <summary> SctionDataの初期化 </summary>
    public void InitSectionData(int arrIndex)
    {
        int totalIndexNum = arrIndex * arrIndex;
        _gridWidth = arrIndex;
        _gridDataArr = new GridData[totalIndexNum];
        
        for (int i = 0; i < Enum.GetValues(typeof(ConnectDirection)).Length; i++)
        {
            _isConnectDirectionDict.Add((ConnectDirection)i, false);
        }
    }

    /// <summary> GridDataの参照用関数 </summary>
    public GridData GetGridData(int x, int y)
    {
        int index = x + (_gridWidth * y);
        return _gridDataArr[index];
    }

    public void AddJointDict(JointType jointType, Joint joint)
    {
        if(!_jointDataDict.ContainsKey(jointType))
            _jointDataDict.Add(jointType, new List<Joint>());

        _jointDataDict[jointType].Add(joint);
    }

    public void AddConnectDirection(ConnectDirection connectDirection)
    {
        _isConnectDirectionDict[connectDirection] = true;
    }
}

public struct GridData
{
    private bool _isJoint;

    private TileType _tileType;

    public bool IsJoint
    {
        get { return _isJoint; }
        set { _isJoint = value; }
    }

    public TileType TileType => _tileType;

    public void SetGridData(TileType tileType)
    {
        _tileType = tileType;
    }
}

public class Joint
{
    private Vector2Int _jointPos;

    public Vector2Int JointPos
    {
        get { return _jointPos; }
        set { _jointPos = value; }
    }
}

public enum TileType
{
    Empty,
    Wall,
    Ground,
}

public enum JointType
{
    Normal,
    Relay,
    SectionExit
}

public enum ConnectDirection
{
    Top,
    Left,
    Right,
    Bottom
}