using UnityEngine;
using Domain;
using Infrastructure;


namespace Infrastructure
{
public interface IFactry<T>
{
    protected const int _spawnPosY = 1;

    public DungeonData DungeonData {  get; }

    /// <summary> ‰Šú‰» </summary>
    public void Init(DungeonData dungeonData);

    /// <summary> ƒXƒ|[ƒ“ </summary>
    public T Spawn(int spawnObjId, Vector2Int spawnPosition);
}
}




