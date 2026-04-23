using Data.Character;
using Roguelike.Dungeon;
using System.Collections.Generic;

public class DungeonData
{
    public DungeonData(StageData dungeonData, int gridSize)
    {
        _stageData = dungeonData;
        _dungeonGridSize = gridSize;
    }

    private int _dungeonGridSize;

    private StageData _stageData;

    private List <CharacterData> _activeCharcterDataList = new();

    public int DungeonGridSize => _dungeonGridSize;
    public StageData CurrentDungeon => _stageData;
    public List<CharacterData> ActiveCharcterDataList => _activeCharcterDataList;
}
