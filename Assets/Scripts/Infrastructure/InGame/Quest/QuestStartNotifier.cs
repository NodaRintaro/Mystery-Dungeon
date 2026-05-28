using System;
using UnityEngine;

namespace Layer.Infrastructure
{
    /// <summary> クエスト開始の通知を行うクラス </summary>
    public class QuestStartNotifier : MonoBehaviour
    {
        public event Action OnQuestStart;

        public event Action OnQuestEnd;

        private void OnEnable()
        {
            if(ServiceLocator.TryGet<QuestSaveData>(out var questSaveData))
            {
                OnQuestStart?.Invoke();
            }
        }

        public void NotifyQuestStart() => OnQuestStart?.Invoke();

        public void NotifyQuestEnd() => OnQuestEnd?.Invoke();
    }
}
