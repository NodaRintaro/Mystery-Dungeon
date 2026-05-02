using Domain;
using UnityEngine;
using System;

namespace Domain
{
    public class CharacterMovement : IDisposable
    {
        public CharacterMovement(ICharacterData chracterData, DungeonData dungeonData, Transform transform)
        {
            _characterData = chracterData;
            _dungeonData = dungeonData;
        }

        private ICharacterData _characterData;
        private DungeonData _dungeonData;
        private Transform _transform;

        public Vector3 MovePosition(CharacterDirectionType characterDirectionType)
        {
            return default;
        }

        public void Dispose()
        {
                    
        }

        private void Walk(Vector3 movePosition)
        {

        }

        private void TurnRotate(CharacterDirectionType characterDirectionType)
        {

        }

        private void GetMovePosition(CharacterDirectionType characterDirectionType)
        {
            switch(characterDirectionType)
            {
                case CharacterDirectionType.Left:

                    break;
                case CharacterDirectionType.Right:
                    break;
                case CharacterDirectionType.Front:
                    break;
                case CharacterDirectionType.Back:
                    break;
            }
        }
    }
}





