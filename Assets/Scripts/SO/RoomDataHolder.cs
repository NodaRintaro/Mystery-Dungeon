using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike.Dungeon
{
    [CreateAssetMenu(fileName = "RoomDataHolder", menuName = "ScriptableObjects/RoomData")]
    public class RoomDataHolder : ScriptableObject
    {
        [SerializeField] private RoomData[] _roomDataArray = new RoomData[0];
        public RoomData[] RoomDataArray => _roomDataArray;

        public void AddRoomData(RoomData data)
        {
            Array.Resize(ref _roomDataArray, _roomDataArray.Length + 1);
            _roomDataArray[_roomDataArray.Length - 1] = data;
        }

        public void RemoveRoomDataAt(int index)
        {
            if (index < 0 || index >= _roomDataArray.Length) return;

            var list = _roomDataArray.ToList();
            list.RemoveAt(index);
            _roomDataArray = list.ToArray();
        }
    }

    [Serializable]
    public class RoomData
    {
        /// <summary>部屋の横の長さ</summary>
        [SerializeField] private int _width = 5;
        /// <summary>部屋の縦の長さ</summary>
        [SerializeField] private int _height = 5;
        /// <summary>部屋の出現重み</summary>
        [SerializeField] private int _roomWeight = 5;
        /// <summary>部屋のデータ</summary>
        [SerializeField] private TileType[] _gridRoomData;

        public int Width => _width;
        public int Height => _height;
        public int RoomWeight => _roomWeight;
        public void SetWidth(int width) => _width = width;
        public void SetHeight(int height) => _height = height;
        public void SetRoomWeight(int weight) => _roomWeight = weight;
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
}