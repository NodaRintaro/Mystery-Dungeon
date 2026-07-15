using UnityEngine;
using UnityEngine.UI;

namespace Domain.Common.Interface
{
    public interface ISpawner<T>
    {
        public T Spawn(int id, Vector3 spawnPos);
    }

    /// <summary> ButtonUIの生成を行うクラスのインターフェース </summary>
    public interface IButtonSpawner : ISpawner<Button> { }

    public interface ICharacterSpawner : ISpawner<ICharacterData> { }
}