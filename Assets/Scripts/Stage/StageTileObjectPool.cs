using UnityEngine;
using System;
using System.Collections.Generic;

namespace Roguelike.Dungeon
{
    [Serializable]
    public class StageTileObjectPool : ObjectPool
    {
        [SerializeField]
        private GameObject _dungeonTileParent = null;

        private Dictionary<TileType, GameObject> _tileObjectDict = new Dictionary<TileType, GameObject>();

        public GameObject DungeonTileParent => _dungeonTileParent;

        public Dictionary<TileType, GameObject> TileObjectDict => _tileObjectDict;

        public void AddTileDict(TileData tileData)
        {
            _tileObjectDict.Add(tileData.TileType, tileData.TileObject);
        }
    }
}