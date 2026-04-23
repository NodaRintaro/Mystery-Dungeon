using System;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "ScriptableObjects/CharacterData", order = 1)]
public class CharacterAssetData : ScriptableObject
{
    [SerializeField] private int _characterID;
    [SerializeField] private GameObject _characterPrefab;
    [SerializeField] private RuntimeAnimatorController _animatorController;

    public int CharacterID => _characterID;
    public GameObject CharacterPrefab => _characterPrefab;
    public RuntimeAnimatorController AnimatorController => _animatorController;
}

