using UnityEngine;

/// <summary> Questの達成条件インターフェース </summary>
public interface IQuestClearCondition
{
    public QuestClearType ClearType { get; } // クリア条件のタイプ
}
