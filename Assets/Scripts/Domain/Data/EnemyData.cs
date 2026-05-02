using UnityEngine;

namespace Domain
{
    public class EnemyData : CharacterData
    {
        public EnemyData(Vector3 characterPosition, int characterGridSize, int characterLevel,
            SkillSlot skillSlot, CharacterStatus characterStatus, CharacterGrowthRates growthRates)
            : base(characterPosition, characterGridSize, characterLevel, skillSlot, characterStatus, growthRates)
        {

        }
    }
}