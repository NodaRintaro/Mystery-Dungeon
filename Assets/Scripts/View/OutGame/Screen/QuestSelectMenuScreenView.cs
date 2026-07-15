using UnityEngine;
using UnityEngine.UI;

using Domain.Common;
using View.OutGame.QuestSelect;

namespace View.OutGame.Screen
{
    public class QuestSelectMenuScreenView : ScreenView
    {
        [Header("クエストの選択ボタンUI")]
        [SerializeField] private QuestSelectButtonUI _questSelectUI;

        [Header("Home画面に遷移するButton")]
        [SerializeField, Tooltip("Home画面に遷移するButton")] private Button _backHomeMenuButton = null;

        public override ScreenType ScreenType => ScreenType.QuestSelectMenu;

        private void Start()
        {
            _backHomeMenuButton.onClick.AddListener(() => HandleChangeScreen(ScreenType.HomeMenu));
        }
    }
}
