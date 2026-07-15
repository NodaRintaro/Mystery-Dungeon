using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace View.OutGame.QuestSelect
{
    [Serializable]
    public class QuestSelectButtonUI
    {
        [Header("クエスト選択を行うボタンのリスト")]
        [SerializeField, Tooltip("クエスト選択を行うボタンのリスト")] private List<Button> _selectButtonList = new();

        /// <summary> クエスト選択ボタンを表示する </summary>
        public void DisplaySelectButtons()
        {

        }

        /// <summary> 表示中のButtonを全て解放する </summary>
        public void ReleaseSelectButtons()
        {

        }
    }
}
