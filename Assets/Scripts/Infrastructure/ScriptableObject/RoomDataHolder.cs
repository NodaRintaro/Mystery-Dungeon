using Domain;
using System;
using System.Linq;
using UnityEngine;

namespace Infrastructure
{
    [CreateAssetMenu(fileName = "RoomDataHolder", menuName = "ScriptableObjects/RoomData")]
    public class RoomDataHolder : ScriptableObject
    {
        [SerializeField] private DungeonRoomData[] _roomDataArray = new DungeonRoomData[0];
        public DungeonRoomData[] RoomDataArray => _roomDataArray;

        public void AddRoomData(DungeonRoomData data)
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
}




