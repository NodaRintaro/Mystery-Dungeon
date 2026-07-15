using Domain.InGame.Character;
using UnityEngine;



namespace Domain.InGame.CommandBattle
{
    public class DamageCalculator
    {
        /// <summary>  </summary>
        /// <param name="targetStatus"></param>
        /// <param name="attackerStatus"></param>
        /// <param name="damageEffect"></param>
        /// <returns> TotalDamage </returns>
        public int CalculateTotalDamage(CharacterStatus targetStatus, CharacterStatus attackerStatus)
        {
            return 0;
        }

        /// <summary>  </summary>
        /// <param name="critical"></param>
        public bool TryCriticalHit(int critical)
        {
            return false;
        }
    }
}

