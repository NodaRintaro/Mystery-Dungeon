using UnityEngine;

[System.Serializable]
public class AreaData
{
    [Header("エリアの座標データ")]
    [SerializeField] private Vector2Int _leftTopPos;
    [SerializeField] private Vector2Int _leftBottomPos;
    [SerializeField] private Vector2Int _rightTopPos;
    [SerializeField] private Vector2Int _rightBottomPos;
    
    [Header("部屋の座標データ")]
    [SerializeField] private Vector2Int _roomLeftTopPos;
    [SerializeField] private Vector2Int _roomLeftBottomPos;
    [SerializeField] private Vector2Int _roomRightTopPos;
    [SerializeField] private Vector2Int _roomRightBottomPos;
    
    [SerializeField, Header("生成された部屋のデータ")] 
    private RoomData _roomData; 
    
    public Vector2Int AreaTopLeftPos => _leftTopPos;
    public Vector2Int AreaBottomLeftPos => _leftBottomPos;
    public Vector2Int AreaTopRightPos => _rightTopPos;
    public Vector2Int AreaBottomRightPos => _rightBottomPos;
    
    public Vector2Int RoomTopLeftPos => _roomLeftTopPos;
    public Vector2Int RoomBottomLeftPos => _roomLeftBottomPos;
    public Vector2Int RoomTopRightPos => _roomRightTopPos;
    public Vector2Int RoomBottomRightPos => _roomRightBottomPos;
    
    public RoomData RoomData => _roomData;
    
    /// <summary> エリアの座標データを登録 </summary>
    /// <param name="leftTopPos">左辺の頂点座標</param>
    /// <param name="leftBottomPos">左辺の低点座標</param>
    /// <param name="rightTopPos">右辺の頂点座標</param>
    /// <param name="rightBottomPos">右辺の低点座標</param>
    public void SetAreaPos(Vector2Int leftTopPos, Vector2Int leftBottomPos, Vector2Int rightTopPos, Vector2Int rightBottomPos)
    {
        _leftTopPos = leftTopPos;
        _leftBottomPos = leftBottomPos;
        _rightTopPos = rightTopPos;
        _rightBottomPos = rightBottomPos;
    }

    /// <summary> 部屋の座標データを登録 </summary>
    /// <param name="leftTopPos">左辺の頂点座標</param>
    /// <param name="leftBottomPos">左辺の低点座標</param>
    /// <param name="rightTopPos">右辺の頂点座標</param>
    /// <param name="rightBottomPos">右辺の低点座標</param>
    public void SetRoomData(Vector2Int leftTopPos, Vector2Int leftBottomPos, Vector2Int rightTopPos, Vector2Int rightBottomPos, RoomData roomData)
    {
        _roomLeftTopPos = leftTopPos;
        _roomLeftBottomPos = leftBottomPos;
        _roomRightTopPos = rightTopPos;
        _roomRightBottomPos = rightBottomPos;
        _roomData = roomData;
    }
}
