using Character.Action;
using UnityEngine;

/// <summary>
/// ユーザーからの入力によって発生する処理部分の機構
/// </summary>
public class PlayerApplication 
{ 
    public PlayerApplication(ICharacterData characterData, DungeonData dungeonData, Transform characterTransform)
    {
        _characterMovement = new(characterData, dungeonData, characterTransform);
        _characterAttack = new();
    }

    private CharacterMovement _characterMovement;

    private CharacterAttack _characterAttack;
}
