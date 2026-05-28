using System;
using UnityEngine;


namespace Layer.Infrastructure
{
    /// <summary> キャラクターの見た目のデータを保管するクラス </summary>
    [CreateAssetMenu(fileName = "CharacterPrefabData", menuName = "ScriptableObjects/Character/Prefab", order = 1)]
    public class CharacterPrefabHolder : ScriptableObject
    {
        [Header("キャラクターのPrefabが格納されている配列")]
        [SerializeField] private CharacterPrefab[] _characterPrefabHolder;

        /// <summary> キャラクターIDに対応するGameObjectを取得する </summary>
        public GameObject GetPrefab(int characterID)
        {
            foreach (var data in _characterPrefabHolder)
            {
                if (data.CharacterID == characterID)
                {
                    return data.CharacterPrefabObject;
                }
            }
            return null;
        }

        #region CharacterPrefab
        /// <summary> キャラクターのPrefab情報を格納する構造体 </summary>
        [Serializable]
        public struct CharacterPrefab
        {
            [Header("キャラクターID")]
            [SerializeField] private int _characterID;

            [Header("キャラクターのPrefab")]
            [SerializeField] private GameObject _characterPrefabObject;

            /// <summary> キャラクターのID </summary>
            public int CharacterID => _characterID;

            /// <summary> キャラクターのPrefab </summary>
            public GameObject CharacterPrefabObject => _characterPrefabObject;
        }
        #endregion
    }
}


