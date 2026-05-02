using System;
using UnityEngine;

namespace Domain
{
    [Serializable]
    public class DungeonRoomData
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