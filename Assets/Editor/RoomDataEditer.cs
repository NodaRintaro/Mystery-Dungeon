using UnityEngine;
using UnityEditor;
using Domain;
using Infrastructure;

[CustomEditor(typeof(RoomDataHolder))]
public class RoomDataEditer : Editor
{
    //スクロールしている場所の位置
    private Vector2 scrollPos;
    private int _scrollHeight = 500;
    
    //空白の大きさ
    private int _spaceSize = 10;
    
    //表示するグリッドの大きさ
    private int _displayGridSize = 25;
    
    //Gridの表示サイズの最低値と最高値
    private int _minDisplayGridSize = 1;
    private int _maxDisplayGridSize = 50;

    private int _selectedRoomIndex = 0;

    //追加のColor
    Color _brown = new Color(0.6f, 0.4f, 0.2f, 1.0f);

    public override void OnInspectorGUI()
    {
        RoomDataHolder holder = (RoomDataHolder)target;

        if (holder.RoomDataArray == null || holder.RoomDataArray.Length == 0)
        {
            EditorGUILayout.HelpBox("RoomDataArray is empty.", MessageType.Info);
            return;
        }

        EditorGUILayout.Space(_spaceSize);

        // 部屋の選択
        string[] options = new string[holder.RoomDataArray.Length];
        for (int i = 0; i < options.Length; i++)
        {
            options[i] = $"Room {i} ({holder.RoomDataArray[i].Width}x{holder.RoomDataArray[i].Height})";
        }
        _selectedRoomIndex = EditorGUILayout.Popup("Select Room", _selectedRoomIndex, options);
        _selectedRoomIndex = Mathf.Clamp(_selectedRoomIndex, 0, holder.RoomDataArray.Length - 1);

        EditorGUILayout.Space(_spaceSize);

        if (GUILayout.Button("Remove Selected Room"))
        {
            holder.RemoveRoomDataAt(_selectedRoomIndex);
            EditorUtility.SetDirty(holder);
            return;
        }

        EditorGUILayout.Space(_spaceSize);

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(_scrollHeight));
        
        RoomData roomData = holder.RoomDataArray[_selectedRoomIndex];
        
        //nullの場合Dataを初期化
        if (roomData.GridRoomData == null || roomData.GridRoomData.Length == 0)
        {
            roomData.InitRoomData(new TileType[roomData.Width * roomData.Height]);
            EditorUtility.SetDirty(holder);
        }
        
        EditorGUILayout.LabelField("横のタイルの長さ:" + roomData.Width);
        EditorGUILayout.LabelField("縦のタイルの長さ:" + roomData.Height);
        EditorGUILayout.LabelField("出現重み:" + roomData.RoomWeight);

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
            case TileType.Ground: return _brown;
            case TileType.Wall: return Color.green;
            default: return Color.magenta;
        }
    }
    
    /// <summary>clickされた際にTileTypeを変える処理</summary>
    private TileType ChangeTileType(TileType type)
    {
        int next = ((int)type + 1) % System.Enum.GetValues(typeof(TileType)).Length;
        return (TileType)next;
    }
}
