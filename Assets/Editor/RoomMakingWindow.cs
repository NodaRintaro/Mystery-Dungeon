using UnityEditor;
using UnityEngine;
using Domain;
using Infrastructure;

[System.Serializable]
public class RoomMakingWindow : EditorWindow
{
    private TileType[] _gridRoomData;
    
    private Vector2 _scrollPos;
    private int _scrollHeight = 600;
    
    private int _width = 5;
    private int _height = 5;
    
    private int _spaceSize = 20;
    private int _displayGridSize = 25;
    
    //Gridの表示サイズの最低値と最高値
    private int _minDisplayGridSize = 1;
    private int _maxDisplayGridSize = 50;

    private RoomDataHolder _roomDataHolder;
    private int _roomWeight = 5;
    
    private bool _isInit = false;
    
    [MenuItem("Tools/CreateRoom")]
    public static void ShowWindow()
    {
        GetWindow<RoomMakingWindow>();
    }
    
    private void OnGUI()
    {
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, GUILayout.Height(_scrollHeight));
        
        //初期化前に行うべき設定
        if (!_isInit)
        {
            EditorGUILayout.LabelField("作る部屋の大きさを設定");
            _width = EditorGUILayout.IntField("横の大きさ:", _width);
            _height = EditorGUILayout.IntField("縦の大きさ:", _height);
            _roomWeight = EditorGUILayout.IntField("出現重み:", _roomWeight);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("保存先のRoomDataHolderを指定");
            _roomDataHolder = (RoomDataHolder)EditorGUILayout.ObjectField(_roomDataHolder, typeof(RoomDataHolder), false);
        }
        
        //1行空ける
        EditorGUILayout.Space(_spaceSize);
        
        //ボタンを押すと初期化されるようにする
        if (GUILayout.Button("Initialize Grid"))
        {
            InitGrid();
        }

        //ボタンを押すと1つ前の画面に戻って部屋の大きさを設定できるようになる
        if (GUILayout.Button("Delete Grid"))
        {
            _gridRoomData = null;
            _isInit = false;
        }
        
        EditorGUILayout.Space(_spaceSize);
        
        if(_isInit)
        {
            RoomMakeGUI();
        }
        
        EditorGUILayout.EndScrollView();
    }

    #region Gridの表示処理
    /// <summary>Gridの表示</summary>
    /// <param name="roomData">部屋のデータクラス</param>
    private void RoomMakeGUI()
    {
        if (_roomDataHolder != null)
        {
            EditorGUILayout.LabelField("保存先:" + _roomDataHolder.name);
        }
        else
        {
            EditorGUILayout.HelpBox("RoomDataHolderをアサインしてください", MessageType.Warning);
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("現在の部屋の大きさ");
        EditorGUILayout.LabelField("横の長さ:" + _width + "マス");
        EditorGUILayout.LabelField("縦の長さ:" + _height + "マス");
            
        EditorGUILayout.Space();
        
        for (int y = 0; y < _height; y++)
        {
            //配列の要素数を横に表示する
            EditorGUILayout.BeginHorizontal();
            for (int x = 0; x < _width; x++)
            {
                TileType currentTile = GetTile(x, y);

                Color color = GetColor(currentTile);
                GUI.backgroundColor = color;

                if (GUILayout.Button(currentTile.ToString().Substring(0, 1), GUILayout.Width(_displayGridSize), GUILayout.Height(_displayGridSize)))
                {
                    TileType nextType = ChangeTileType(currentTile);
                    SetTile(x, y, nextType);
                    //EditorUtility.SetDirty(_gridRoomData);
                }

                GUI.backgroundColor = Color.white;
            }
            EditorGUILayout.EndHorizontal();
        }
        
        if (GUILayout.Button("Add to RoomDataHolder"))
        {
            SaveRoomData();
        }
        
        _displayGridSize = EditorGUILayout.IntSlider("タイルの表示サイズ", _displayGridSize, _minDisplayGridSize, _maxDisplayGridSize);
    }
    #endregion

    #region Gridの初期化
    /// <summary>Gridの初期化</summary>
    private void InitGrid()
    {
        _gridRoomData = new TileType[_width * _height];
        for (int i = 0; i < _width; i++)
        for (int j = 0; j < _height; j++)
            _gridRoomData[i + _width * j] = TileType.Ground;
        _isInit = true;
    }
    #endregion

    #region Dataの保存
    /// <summary>部屋のデータを保存</summary>
    private void SaveRoomData()
    {
        if (_roomDataHolder == null)
        {
            Debug.LogError("RoomDataHolder is not assigned!");
            return;
        }

        RoomData newRoomData = new RoomData();

        //Dataを保存
        newRoomData.SetHeight(_height);
        newRoomData.SetWidth(_width);
        newRoomData.SetRoomWeight(_roomWeight);
        newRoomData.InitRoomData(_gridRoomData);
        
        _roomDataHolder.AddRoomData(newRoomData);
        EditorUtility.SetDirty(_roomDataHolder);
        AssetDatabase.SaveAssets();
        
        Debug.Log("RoomData added to " + _roomDataHolder.name);
    }
    #endregion
	
    /// <summary>clickされた際にTileTypeを変える機能</summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private TileType ChangeTileType(TileType type)
    {
        int next = ((int)type + 1) % System.Enum.GetValues(typeof(TileType)).Length;
        return (TileType)next;
    }

    /// <summary>グリッド上の表記をわかり安くする</summary>
    /// <param name="type"></param>
    /// <returns></returns>
    private Color GetColor(TileType type)
    {
        switch (type)
        {
            case TileType.Ground: return Color.green;
            case TileType.Wall: return Color.black;
            default: return Color.magenta;
        }
    }
    
    /// <summary>指定したタイルの情報を取得</summary>
    public TileType GetTile(int x, int y)
    {
        return _gridRoomData[TilePos(x,y)];
    }
    
    /// <summary>指定したタイルのタイプを変更</summary>
    public void SetTile(int tilePosX, int tilePosY, TileType tileType)
    {
        _gridRoomData[TilePos(tilePosX,tilePosY)] = tileType;
    }

    private int TilePos(int x, int y)
    {
        return x + y * _width;
    }
}
