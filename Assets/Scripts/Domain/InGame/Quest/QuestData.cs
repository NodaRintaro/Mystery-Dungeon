
using Domain.Common.Interface;
using Domain.InGame.Dungeon;

namespace Domain.InGame.Quest
{
    public class QuestData : IQuestData
    {
        public QuestData(int questId, string questName, string questDescription, int dungeonFloorNum, 
            IQuestClearCondition clearCondition, DungeonEnvironmentType dungeonEnvironmentType, int[] questDungeonCharactersID,
            DungeonInformation[] dungeonConditions)
        {
            _questId = questId;
            _questName = questName;
            _questDescription = questDescription;
            _dungeonFloorNum = dungeonFloorNum;
            _clearCondition = clearCondition;
            _environmentType = dungeonEnvironmentType;
            _questDungeonCharactersID = questDungeonCharactersID;
            _dungeonConditions = dungeonConditions;
        }

        /// <summary> QuestDataのID </summary>
        private int _questId;
        /// <summary> クエストの名前 </summary>
        private string _questName;
        /// <summary> クエストの説明 </summary>
        private string _questDescription;
        /// <summary> クエストの発行されたダンジョンの階数 </summary>
        private int _dungeonFloorNum;
        /// <summary> クリア条件 </summary>
        private IQuestClearCondition _clearCondition;
        /// <summary> ダンジョンの環境 </summary>
        private DungeonEnvironmentType _environmentType;
        /// <summary> クエストが発行されたダンジョンに生息するキャラクターのID </summary>
        private int[] _questDungeonCharactersID;
        /// <summary> クエストが発行されたダンジョンの各フロアの生成状態 </summary>
        private DungeonInformation[] _dungeonConditions;

        /// <summary> QuestDataのID </summary>
        public int ID => _questId;
        /// <summary> クエストの名前 </summary>
        public string Name => _questName;
        /// <summary> クエストの説明 </summary>
        public string QuestDescription => _questDescription;
        /// <summary> クエストの発行されたダンジョンの階数 </summary>
        public int DungeonFloorNum => _dungeonFloorNum;
        /// <summary> クリア条件 </summary>
        public IQuestClearCondition ClearCondition => _clearCondition;
        /// <summary> ダンジョンの環境 </summary>
        public DungeonEnvironmentType EnvironmentType => _environmentType;
        /// <summary> クエストが発行されたダンジョンに生息するキャラクターのID </summary>
        public int[] QuestDungeonCharactersID => _questDungeonCharactersID;
        /// <summary> クエストが発行されたダンジョンの各フロアの生成状態 </summary>
        public DungeonInformation[] DungeonConditions => _dungeonConditions;
    }
}
