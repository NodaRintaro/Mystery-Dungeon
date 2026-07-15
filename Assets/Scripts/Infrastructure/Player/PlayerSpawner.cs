using Domain.Common.Interface;
using Domain.InGame.Character;
using Domain.InGame.Dungeon;
using Infrastructure.Character;
using UnityEngine;


namespace InGame.Infrastructure
{
    public class PlayerSpawner : MonoBehaviour, ISpawner<CharacterData>
    {
        private CharacterDataRepositry _characterDataRepositry = null;

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
            return default;
        }
    }
}