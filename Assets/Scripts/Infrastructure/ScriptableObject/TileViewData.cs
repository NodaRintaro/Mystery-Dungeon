using Domain;
using System;
using UnityEngine;



namespace Infrastructure
{
    [CreateAssetMenu(fileName = "Tile", menuName = "ScriptableObjects/TileViewData")]
    public class TileViewDataHolder : ScriptableObject
    {
        [SerializeField] private TileViewData[] _tileViewDataArray;

        public TileViewData[] TileViewDataArray => _tileViewDataArray;
    }

    /// <summary> タイルの見た目のデータ </summary>
    [Serializable]
    public class TileViewData
    {
        [Header("Tileのオブジェクト")]
        [SerializeField] private GameObject _tileObject;

        [Header("Tile種類")]
        [SerializeField] TileType _tileType;

        public GameObject TileObject => _tileObject;
        public TileType TileType => _tileType;
    }
}




