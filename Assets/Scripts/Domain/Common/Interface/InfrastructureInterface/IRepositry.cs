using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Domain.Common.Interface
{
    public interface IRepositry<T> 
    {
        public T GetData(int id);
    }

    public interface IQuestDataRepositry : IRepositry<IQuestData> { }

    public interface IPlayerDataRepositry : IRepositry<IPlayerData> { }
}
