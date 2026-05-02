using Domain;
using UnityEngine;

namespace Application
{
    /// <summary> ユーザーからの入力によって発生する処理部分の機構 </summary>
    public class CharacterApplication
    {
        public CharacterApplication(ICharacterData characterData, DungeonData dungeonData, Transform characterTransform)
        {
            _characterMovement = new(characterData, dungeonData, characterTransform);
            _characterAttack = new();
        }

        private CharacterMovement _characterMovement = null;

        private CharacterSkill _characterAttack = null;
    }
}





