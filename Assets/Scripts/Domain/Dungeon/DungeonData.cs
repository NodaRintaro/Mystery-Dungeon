using System.Collections.Generic;
using Domain;

namespace Domain
{
    public class DungeonData
    {
        public DungeonData(StageData dungeonData, int gridSize)
        {
            _stageData = dungeonData;
            _dungeonGridSize = gridSize;
        }

        private int _dungeonGridSize;

        private StageData _stageData;

        private List<ICharacterData> _activeCharcterDataList = new();

        public int DungeonGridSize => _dungeonGridSize;
        public StageData CurrentDungeon => _stageData;
        public List<ICharacterData> ActiveCharcterDataList => _activeCharcterDataList;
    }
}




