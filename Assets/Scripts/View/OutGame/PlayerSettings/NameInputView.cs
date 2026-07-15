using System;
using TMPro;
using UnityEngine;

namespace View.OutGame.PlayerSettings
{
    [Serializable]
    public class NameInputView
    {
        [Header("記入欄のUI")]
        [SerializeField] private TMP_InputField _inputField;
        [SerializeField] private TMP_Text _nameDisplay;

        [Header("現在の名前")]
        [SerializeField] private string _playerName = string.Empty;

        private const int _minNameLength = 1;
        private const int _maxNameLength = 10;

        public string PlayerName => _playerName;

        public void LoadCurrentName(string currentName)
        {
            if (string.IsNullOrEmpty(currentName))
            {
                _inputField.placeholder.gameObject.SetActive(true);
            }
            else
            {
                _inputField.placeholder.gameObject.SetActive(false);
                _inputField.text = currentName;
            }
        }

        /// <summary> 名前記入時に呼ばれる処理 </summary>
        public void OnNameSubmit(string newName)
        {
            // 入力バリデーション（例：2〜10 文字）
            if (string.IsNullOrWhiteSpace(newName) 
                || newName.Length < _minNameLength 
                || newName.Length > _maxNameLength)
            {
                Debug.LogWarning($"名前は {_minNameLength}〜{_maxNameLength} 文字で入力してください");
                return;
            }

            _inputField.placeholder.gameObject.SetActive(false);
            _playerName = newName;
            newName = _inputField.text;
        }
    }
}