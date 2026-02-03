using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class DungeonTilePool
{
    [SerializeField]
    private GameObject _tileHolderObj = null;

    [SerializeField]
    private ObjectPool _dungeonTilePool = new();

    private Dictionary<TileType, GameObject> _tileObjectDict = new Dictionary<TileType, GameObject>();

    public GameObject TileHolderObj => _tileHolderObj;

    public ObjectPool TileObjectPool => _dungeonTilePool;

    public Dictionary<TileType, GameObject> TileObjectDict => _tileObjectDict;

    public void AddTileDict(TileData tileData)
    {
        _tileObjectDict.Add(tileData.TileType,tileData.TileObject);
    }
}