using UnityEngine;

using Domain.InGame.Quest;

namespace InGame.Infrastructure
{
    public class QuestInitializer : MonoBehaviour
    {
        [Header("クエストデータ")]
        [SerializeField] private QuestData _questData;

        

        public void Init(QuestData questData)
        {
            _questData = questData;
        }
    }
}
