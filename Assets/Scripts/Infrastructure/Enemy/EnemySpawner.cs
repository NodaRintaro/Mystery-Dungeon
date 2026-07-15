using Domain.Common.Interface;
using UnityEngine;

using View.InGame.Enemy;

namespace InGame.Infrastructure
{
    public class EnemySpawner : MonoBehaviour, ISpawner<EnemyCharacterView>
    {
        public EnemyCharacterView Spawn(int spawnObjId, Vector3 spawnPosition)
        {
            return null;
        }
    }
}








