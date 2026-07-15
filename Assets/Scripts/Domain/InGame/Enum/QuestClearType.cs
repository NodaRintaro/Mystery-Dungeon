using UnityEngine;

namespace Domain.InGame
{
    /// <summary> クエストクリア条件のEnum </summary>
    public enum QuestClearType
    {
        None = 0, // 初期値
        DefeatEnemy, // 敵を撃破する
        CollectItem, // 特定のアイテムを回収する
    }
}