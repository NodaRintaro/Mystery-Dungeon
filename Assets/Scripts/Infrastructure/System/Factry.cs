using UnityEngine;
using Layer.Domain;

namespace Layer.Infrastructure
{
    public abstract class Factry<T> : IFactry<T>
    {
        private static DungeonData _dungeonData;

        public static DungeonData CurrentDungeon => _dungeonData;

        public static void SetDungeonData(DungeonData dungeonData)
        {
            _dungeonData = dungeonData;
        }

        public abstract T Spawn(int spawnObjId, Vector3 spawnPosition);
    }
}