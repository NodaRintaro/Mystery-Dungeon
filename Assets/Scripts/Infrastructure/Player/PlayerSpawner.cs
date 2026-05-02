using Domain;
using UnityEngine;


namespace Infrastructure
{
    public class PlayerSpawner : MonoBehaviour, IFactry<CharacterData>
    {
        private CharacterRepositry _characterDataRepositry = null;

        private DungeonData _dungeonData;

        private const int _playerID = 1;

        public DungeonData CurrentDungeon => _dungeonData;

        private void Start()
        {

        }

        public void Init(DungeonData dungeonData)
        {
            _dungeonData = dungeonData;
        }

        public CharacterData Spawn(int spawnObjId, Vector3 spawnPosition)
        {
            return null;
        }
    }
}





