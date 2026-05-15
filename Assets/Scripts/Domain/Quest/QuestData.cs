using UnityEngine;

namespace Layer.Domain
{
    public class QuestData : IQuestData
    {
        [SerializeField, Tooltip("クエストのID")] 
        private int _questId;

        [SerializeField, Tooltip("クエストの名前")] 
        private string _questName;

        [SerializeField, Tooltip("クエストの説明")] 
        private string _questDescription;

        [SerializeField, Tooltip("クエストの発行されたダンジョンの階数")] 
        private int _dungeonFloorNum;

        [SerializeField, Tooltip("クエストのクリア条件")] 
        private IQuestClearCondition _clearCondition;

        [SerializeField, Tooltip("ダンジョンに生息するキャラクター達のID")] 
        private int[] _questDungeonCharactersID;

        [SerializeField, Tooltip("クエストが発行されたダンジョンの各フロアの生成状態")] 
        private DungeonCondition[] _dungeonConditions;

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
        /// <summary> クエストが発行されたダンジョンに生息するキャラクターのID </summary>
        public int[] QuestDungeonCharactersID => _questDungeonCharactersID;
        /// <summary> クエストが発行されたダンジョンの各フロアの生成状態 </summary>
        public DungeonCondition[] DungeonConditions => _dungeonConditions;
    }
}
