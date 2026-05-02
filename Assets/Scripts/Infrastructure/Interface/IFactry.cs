using UnityEngine;
using Domain;
using Infrastructure;


namespace Infrastructure
{
    public interface IFactry<T>
    {
        public static DungeonData CurrentDungeon { get; }

        /// <summary> スポーン </summary>
        public T Spawn(int spawnObjId, Vector3 spawnPosition);
    }
}