namespace Layer.Domain
{
    /// <summary> クエストのデータインターフェース </summary>
    public interface IQuestData : IData
    {
        /// <summary> クエストの説明 </summary>
        public string QuestDescription { get; }

        /// <summary> クエストの発行されたダンジョンの階数 </summary>
        public int DungeonFloorNum { get; }

        /// <summary> クリア条件 </summary>
        public IQuestClearCondition ClearCondition { get; }

        /// <summary> クエストが発行されたダンジョンに生息するキャラクターのID </summary>
        public int[] QuestDungeonCharactersID { get; }

        /// <summary> クエストが発行されたダンジョンの各フロアの生成状態 </summary>
        public DungeonCondition[] DungeonConditions { get; }
    }
}
