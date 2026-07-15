using Domain.Common.Interface;
using System.Collections.Generic;

namespace Domain.InGame.Quest
{
    public class QuestSelector
    {
        public QuestSelector(IQuestDataRepositry questDataRepositry)
        {
            _questDataRepositry = questDataRepositry;
        }

        private IQuestDataRepositry _questDataRepositry;

        /// <summary> Playerに対して現在選択可能なクエストのデータを選別して渡す </summary>
        public List<IQuestData> GetCanSelectQuestData()
        {
            

            return null;
        }
    }
}
