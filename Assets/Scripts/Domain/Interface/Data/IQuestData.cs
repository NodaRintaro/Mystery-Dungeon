namespace Layer.Domain
{
    /// <summary> クエストのデータインターフェース </summary>
    public interface IQuestData
    {
        /// <summary> クエストの名前 </summary>
        public string QuestName { get; }

        /// <summary> クエストの説明 </summary>
        public string QuestDescription { get; }

        /// <summary> クリア条件 </summary>
        public IQuestClearConditions ClearConditions { get; }

        /// <summary> クエストが発行されたダンジョンに生息するキャラクターのデータ </summary>
        public ICharacterData[] QuestDungeonCharactersData { get; }
    }
}
