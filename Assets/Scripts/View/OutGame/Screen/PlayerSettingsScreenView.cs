using UnityEngine;
using UnityEngine.UI;

using Domain.Common;

namespace View.OutGame.Screen
{
    public class PlayerSettingsScreenView : ScreenView
    {
        [Header("Home画面に遷移するButton")]
        [SerializeField, Tooltip("Home画面に遷移するButton")] private Button _backHomeMenuButton = null;

        public override ScreenType ScreenType => ScreenType.PlayerSettings;

        private void Start()
        {
            _backHomeMenuButton.onClick.AddListener(() => HandleChangeScreen(ScreenType.HomeMenu));
        }
    }
}