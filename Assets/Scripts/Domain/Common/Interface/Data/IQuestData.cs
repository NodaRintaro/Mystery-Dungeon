using Domain.InGame.Dungeon;
using Domain.InGame;
using UniRx;

namespace Domain.Common.Interface
{
    /// <summary> クエストのデータインターフェース </summary>
    public interface IQuestData
    {
        /// <summary> クエストの説明 </summary>
        public string QuestDescription { get; }

        /// <summary> クエストの発行されたダンジョンの階数 </summary>
        public int DungeonFloorNum { get; }

        /// <summary> クリア条件 </summary>
        public IQuestClearCondition ClearCondition { get; }

        /// <summary> ダンジョンの環境 </summary>
        public DungeonEnvironmentType EnvironmentType { get; }

        /// <summary> クエストが発行されたダンジョンに生息するキャラクターのID </summary>
        public int[] QuestDungeonCharactersID { get; }

        /// <summary> クエストが発行されたダンジョンの各フロアの生成状態 </summary>
        public DungeonInformation[] DungeonConditions { get; }
    }
}
