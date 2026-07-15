using UniRx;
using UnityEngine;

namespace Domain.Common.Interface
{
    /// <summary> Questの達成条件インターフェース </summary>
    public interface IQuestClearCondition
    {
        public IReadOnlyReactiveProperty<bool> IsQuestClear { get; }
    }

}