using Cysharp.Threading.Tasks;
using Layer.Domain;
using Layer.Infrastructure;
using UnityEngine;

namespace Layer.Infrastructure
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
