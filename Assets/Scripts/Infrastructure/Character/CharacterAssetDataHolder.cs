using Domain;
using System;
using UnityEngine;


namespace Infrastructure
{
    /// <summary> キャラクターの見た目のデータを保管するクラス </summary>
    [CreateAssetMenu(fileName = "NewCharacterPrefabData", menuName = "ScriptableObjects/CharacterPrefabData", order = 1)]
    public class CharacterAssetDataHolder : ScriptableObject
    {
        [SerializeField] private CharacterAssetData[] _characterViewDataArray;

        /// <summary> キャラクターIDに対応する見た目のデータを取得する </summary>
        public CharacterAssetData GetCharacterViewData(int characterID)
        {
            foreach (var data in _characterViewDataArray)
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
    public class CharacterAssetData
    {
        [SerializeField] private int _characterID;
        [SerializeField] private GameObject _characterPrefab;
        [SerializeField] private RuntimeAnimatorController _animatorController;

        public int CharacterID => _characterID;
        public GameObject CharacterPrefab => _characterPrefab;
        public RuntimeAnimatorController AnimatorController => _animatorController;
    }
}