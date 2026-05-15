using UnityEngine;

namespace Layer.Domain
{
    public class PlayerData : CharacterData
    {
        public PlayerData(
            int id, 
            string name, 
            Vector3 position, 
            int gridSize, 
            int level,
            SkillSlot skillSlot, 
            CharacterStatus status, 
            CharacterGrowthRates growthRates)
            : base(id, name, position, gridSize, level, skillSlot, status, growthRates)
        {

        }
    }
}
