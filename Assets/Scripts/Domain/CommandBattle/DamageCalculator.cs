﻿using UnityEngine;

namespace Layer.Domain
{
    public class DamageCalculator
    {
        /// <summary>  </summary>
        /// <param name="targetStatus"></param>
        /// <param name="attackerStatus"></param>
        /// <param name="damageEffect"></param>
        /// <returns> TotalDamage </returns>
        public int CalculateTotalDamage(CharacterStatus targetStatus, CharacterStatus attackerStatus, DamageEffect damageEffect)
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

