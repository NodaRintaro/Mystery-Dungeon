using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

using Domain.Common;

namespace View.OutGame.Screen
{
    public class HomeMenuScreenView : ScreenView
    {
        [Header("クエスト選択画面に遷移するボタン")]
        [SerializeField, Tooltip("クエスト選択に遷移するボタン")] private Button _questSelectDisplayButton;
        [SerializeField, Tooltip("ボタンのイメージ")] private Image _questSelectButtonImage;

        [Header("プレイヤー設定画面へ遷移するボタン")]
        [SerializeField] private Button _playerSettingsDisplayButton;
        [SerializeField, Tooltip("ボタンのイメージ")] private Image _playerSettingsButtonImage;

        // Fade後のアルファ値の最終版値
        private readonly float _fadeOutEndValue = 1;
        private Color _questSelectButtonImageDefaultColor = default;
        private Color _playerSettingsButtonImageDefaultColor = default;

        public override ScreenType ScreenType => ScreenType.HomeMenu;

        private void Awake()
        {
            _questSelectButtonImageDefaultColor = _questSelectButtonImage.color;
            _playerSettingsButtonImageDefaultColor = _playerSettingsButtonImage.color;
        }

        private async void Start()
        {
            _questSelectDisplayButton.onClick.AddListener(() => OnClickedQuestSelectScreenDisplayButton().Forget());
            _playerSettingsDisplayButton.onClick.AddListener(() => OnClickPlayerSettingsScreenDisplayButton().Forget());
        }

        private void OnEnable()
        {
            // ボタンの色をもとの戻す
            _questSelectButtonImage.color = _questSelectButtonImageDefaultColor;
            _playerSettingsButtonImage.color = _playerSettingsButtonImageDefaultColor;
        }

        /// <summary> クエスト選択画面へ遷移するボタンがクリックされた際のメソッド </summary>
        private async UniTask OnClickedQuestSelectScreenDisplayButton()
        {
            float duration = 1;

            await _questSelectButtonImage.DOColor(Color.black, duration);

            HandleChangeScreen(ScreenType.QuestSelectMenu);
        }

        /// <summary> プレイヤー設定画面へ遷移するボタンがクリックされた際のメソッド </summary>
        private async UniTask OnClickPlayerSettingsScreenDisplayButton()
        {
            float duration = 1;

            await _playerSettingsButtonImage.DOColor(Color.white, duration);

            HandleChangeScreen(ScreenType.PlayerSettings);
        }
    }
}
