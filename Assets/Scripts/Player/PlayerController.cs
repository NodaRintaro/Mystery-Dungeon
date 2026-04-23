using Data.Character;
using UniRx;
using System;
using UnityEngine;
using Character.Action;

public class PlayerController : ICharacterCommander
{
    public PlayerController(ICharacterData characterData, Transform characterTransform)
    {
        _characterData = characterData;

        TurnStart += HandleStartMyTurn;
        TurnEnd += HandleEndMyTurn;
    }

    public event Action TurnStart;

    public event Action TurnEnd;

    private PlayerApplication _playerApplication;

    private ICharacterData _characterData;

    private bool _isMyTurn = false;

    public bool IsMyTurn => _isMyTurn;
    public ICharacterData CharacterData => _characterData;

    public void OnUpDate()
    {
        if(_isMyTurn)
        {

        }
    }

    public void HandleStartMyTurn()
    {
        _isMyTurn = true;
    }

    public void HandleEndMyTurn()
    {
        _isMyTurn = false;
    }
}
