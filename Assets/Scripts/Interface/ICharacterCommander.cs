using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;
using Data.Character;
using System;

public interface ICharacterCommander
{
    public event Action TurnStart;

    public event Action TurnEnd;

    public bool IsMyTurn { get; }

    public ICharacterData CharacterData { get; }
}
