using UnityEngine;

namespace Domain.Common.Interface
{
    public interface IInGameInitializer
    {
        public void Init(IQuestData questData);
    }
}