﻿using UnityEngine;
using System;
using System.Collections.Generic;

namespace Layer.Infrastructure
{
    [Serializable]
    public class DungeonTileObjectPool : ObjectPool
    {
        [SerializeField]
        private GameObject _dungeonTileParent = null;

        private Dictionary<TileType, GameObject> _tileObjectDict = new Dictionary<TileType, GameObject>();

        public GameObject DungeonTileParent => _dungeonTileParent;

        public Dictionary<TileType, GameObject> TileObjectDict => _tileObjectDict;

        public void AddTileDict(TileViewData tileData)
        {
            _tileObjectDict.Add(tileData.TileType, tileData.TileObject);
        }
    }
}







