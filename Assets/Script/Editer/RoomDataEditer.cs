using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomData))]
public class RoomDataEditer : Editor
{
    private Vector2 scrollPos;
    private int _scrollHeight = 500;
    
    private int _spaceSize = 10;
    private int _displayGridSize = 25;
    
    //Gridの表示サイズの最低値と最高値
    private int _minDisplayGridSize = 1;
    private int _maxDisplayGridSize = 50;
        
    public override void OnInspectorGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(_scrollHeight));
        
        RoomData roomData = (RoomData)target;
        
        //nullの場合Dataを初期化
        if (roomData.GridRoomData == null)
        {
            roomData.InitRoomData(new TileType[roomData.Width * roomData.Height]);
            EditorUtility.SetDirty(roomData);
        }
        
        EditorGUILayout.LabelField("横のタイルの長さ:" + roomData.Width);
        EditorGUILayout.LabelField("縦のタイルの長さ:" + roomData.Height);

        EditorGUILayout.Space(_spaceSize);
        
        TileTypeGUI(roomData, _displayGridSize);
        
        _displayGridSize = EditorGUILayout.IntSlider("タイルの表示サイズ", _displayGridSize, _minDisplayGridSize, _maxDisplayGridSize);
        
        EditorGUILayout.EndScrollView();
    }

    private void TileTypeGUI(RoomData roomData, int displaySize)
    {
        TileType currentTile;
        Color tileColor;
        
        for (int y = 0; y < roomData.Height; y++)
        {
            //配列の要素数を横に表示する
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < roomData.Width; x++)
            {
                currentTile = roomData.GridRoomData[x + roomData.Width * y];
                tileColor = GetTileColor(currentTile);
                GUI.backgroundColor = tileColor;
                GUILayout.Button(currentTile.ToString().Substring(0, 1), GUILayout.Width(_displayGridSize), GUILayout.Height(_displayGridSize));
                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndHorizontal();
        }
    }
    
    /// <summary>グリッド上の表記をわかり安くする</summary>
    private Color GetTileColor(TileType type)
    {
        switch (type)
        {
            case TileType.Empty: return Color.white;
            case TileType.Walkable: return Color.green;
            case TileType.UnWalkable: return Color.black;
            default: return Color.magenta;
        }
    }
    
    /// <summary>clickされた際にTileTypeを変える機能</summary>
    private TileType ChangeTileType(TileType type)
    {
        int next = ((int)type + 1) % System.Enum.GetValues(typeof(TileType)).Length;
        return (TileType)next;
    }
}
