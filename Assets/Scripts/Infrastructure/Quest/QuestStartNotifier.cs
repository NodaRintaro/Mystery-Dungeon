using System;
using UnityEngine;

namespace InGame.Infrastructure
{
    /// <summary> クエスト開始の通知を行うクラス </summary>
    public class QuestStartNotifier : MonoBehaviour
    {
        public event Action OnQuestStart;

        public event Action OnQuestEnd;

        private void OnEnable()
        {

        }

        public void NotifyQuestStart() => OnQuestStart?.Invoke();

        public void NotifyQuestEnd() => OnQuestEnd?.Invoke();
    }
}
