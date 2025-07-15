using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable,CreateAssetMenu(fileName = "RoomData", menuName = "ScriptableObjects/RoomData")]
public class RoomData : ScriptableObject
{
    /// <summary>部屋の横の長さ</summary>
    [SerializeField] private int _width = 5;
    /// <summary>部屋の縦の長さ</summary>
    [SerializeField] private int _height = 5;
    /// <summary>部屋のデータ</summary>
    [SerializeField] private TileType[] _gridRoomData;
    public int Width => _width;
    
    public int Height => _height;
    
    public void SetWidth(int width) => _width = width;
    
    public void SetHeight(int height) => _height = height;
    
    public TileType[] GridRoomData => _gridRoomData;
    
    public void InitRoomData(TileType[] newRoomData)
    {
        _gridRoomData = new TileType[_width * _height];
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                _gridRoomData[y * _width + x] = newRoomData[y * _width + x];
            }
        }
    }
}