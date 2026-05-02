using UnityEngine;

namespace Domain
{
    public class PlayerData : CharacterData
    {
        public PlayerData(Vector3 characterPosition, int characterGridSize, int characterLevel,
            SkillSlot skillSlot, CharacterStatus characterStatus, CharacterGrowthRates growthRates)
            : base(characterPosition, characterGridSize, characterLevel, skillSlot, characterStatus, growthRates)
        {

        }
    }
}