using System;
using UnityEngine;

/// <summary> キャラクターの見た目のデータを保管するクラス </summary>
[CreateAssetMenu(fileName = "NewCharacterPrefabData", menuName = "ScriptableObjects/CharacterPrefabData", order = 1)]
public class CharacterVisualDataHolder : ScriptableObject
{
    [SerializeField] private CharacterVisualData[] _characterVisualDataArray;

    /// <summary> キャラクターIDに対応する見た目のデータを取得する </summary>
    public CharacterVisualData GetCharacterVisualData(int characterID)
    {
        foreach (var data in _characterVisualDataArray)
        {
            if (data.CharacterID == characterID)
            {
                return data;
            }
        }
        return null;
    }
}

[Serializable]
public class CharacterVisualData
{
    [SerializeField] private int _characterID;
    [SerializeField] private GameObject _characterPrefab;
    [SerializeField] private RuntimeAnimatorController _animatorController;

    public int CharacterID => _characterID;
    public GameObject CharacterPrefab => _characterPrefab;
    public RuntimeAnimatorController AnimatorController => _animatorController;
}



