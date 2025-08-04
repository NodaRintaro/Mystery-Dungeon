using System;
using UnityEngine;

/// <summary>�}�b�v�̃f�[�^��ۑ�����N���X</summary>
#region MapDataClass
[Serializable]
public class MapData
{
    private int _width, _height;

    /// <summary>�}�b�v�f�[�^�̔z��A2�����z�񂾂�Serialize�s�\�Ȃ���1�����z��ŕۑ�</summary>
    private TileType[] _gridMapData;

    public int Width => _width;
    public int Height => _height;

    public TileType[] GridMapData => _gridMapData;

    public void InitData(int width, int height)
    {
        _gridMapData = new TileType[width * height];
        _width = width;
        _height = height;
        Debug.Log("�z��̒���" + GridMapData.Length);
    }

    public void SetTileType(int tilePosX, int tilePosY, TileType tileType)
    {
        _gridMapData[tilePosX + _width * tilePosY] = tileType;
    }

    public TileType GetTileType(int tilePosX, int tilePosY)
    {
        return _gridMapData[tilePosX + _width * tilePosY];
    }
}
#endregion