using UnityEngine;

namespace Domain.Common.Interface
{
    public interface ISaveData<T>
    {
        public void Load(T saveData);
    }
}