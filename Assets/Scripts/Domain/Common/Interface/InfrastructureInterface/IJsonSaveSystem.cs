using UnityEngine;

namespace Domain.Common.Interface
{
    public interface IJsonSaveSystem<T> where T : ISaveData<T>
    {
        /// <summary> データをセーブする </summary>
        public void Save(string path);

        /// <summary> セーブデータをロードする </summary>
        public T Load(string path);
    }
}