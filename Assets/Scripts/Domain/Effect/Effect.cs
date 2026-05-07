﻿

namespace Layer.Domain
{
    public struct DamageEffect : IEffect
    {
        public DamageEffect(ICharacterData targetCharacter, int damageValue)
        {
            _targetCharacter = targetCharacter;
            _damageValue = damageValue;
        }

        private int _damageValue;

        private ICharacterData _targetCharacter;

        public ICharacterData TargetCharacter => _targetCharacter;


    }
}
