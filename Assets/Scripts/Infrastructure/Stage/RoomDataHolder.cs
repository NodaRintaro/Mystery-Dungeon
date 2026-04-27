using Domain;
using System;
using System.Linq;
using UnityEngine;

namespace Infrastructure
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
}




