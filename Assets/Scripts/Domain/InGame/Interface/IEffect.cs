﻿using UnityEngine;

namespace Layer.Domain
{
    public interface IEffect
    {
        public ICharacterData TargetCharacter { get; }
    }
}

