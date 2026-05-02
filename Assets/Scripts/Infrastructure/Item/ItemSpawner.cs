using UnityEngine;
using Domain;

namespace Infrastructure
{
    public class ItemSpawner : MonoBehaviour, IFactry<ItemData>
    {
        private DungeonData _dungeonData = null;

        public DungeonData CurrentDungeon { get; }

        public void Init(DungeonData dungeonData)
        {
            _dungeonData = dungeonData;
        }

        public ItemData Spawn(int spawnObjId, Vector3 spawnPosition)
        {
            //GameObject enemyInstance = Instantiate(spawnObj);
            //enemyInstance.transform.position = new Vector3(spawnPosition.x * DungeonGenerator.GridSize, _spawnPosY, spawnPosition.y * DungeonGenerator.GridSize);
            return null;
        }
    }
}





