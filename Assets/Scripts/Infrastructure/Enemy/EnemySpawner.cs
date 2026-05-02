using UnityEngine;

using Domain;

namespace Infrastructure
{
    public class EnemySpawner : MonoBehaviour, IFactry<CharacterData>
    {
        private DungeonData _dungeonData = null;

        public DungeonData CurrentDungeon { get; }

        public void Init(DungeonData dungeonData)
        {
            _dungeonData = dungeonData;
        }

        public CharacterData Spawn(int spawnObjId, Vector3 spawnPosition)
        {
            //GameObject enemyInstance = Instantiate(spawnObj);
            //enemyInstance.transform.position = new Vector3(spawnPosition.x * DungeonGenerator.GridSize, _spawnPosY, spawnPosition.y * DungeonGenerator.GridSize);
            return null;
        }
    }
}





