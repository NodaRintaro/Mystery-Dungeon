using UnityEngine;
using Layer.Domain;

namespace Layer.Infrastructure
{
    public interface IFactry<T>
    {
        public static DungeonData CurrentDungeon { get; }

        /// <summary> スポーン </summary>
        public T Spawn(int spawnObjId, Vector3 spawnPosition);
    }
}


