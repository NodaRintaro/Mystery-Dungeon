using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

using Domain.Common;

namespace View.OutGame.Screen 
{ 
    public class TitleScreenView : ScreenView
    {
        [Header("ホーム画面に遷移するボタン")]
        [SerializeField, Tooltip("ホーム画面に遷移するボタン")] private Button _gameStartButton = null;
        [SerializeField, Tooltip("ボタンのキャンバスグループ")] private CanvasGroup _gameStartButtonCanvasGroup = null;

        [Header("ボタン表示を行うまでの遅延フレーム数")]
        [SerializeField] private int _delayTime = 100;

        public override ScreenType ScreenType => ScreenType.Title;

        private void Start()
        {
            // ボタンの表示を隠す
            _gameStartButtonCanvasGroup.alpha = 0;

            DisplayingGameStartButton().Forget();
        }

        /// <summary> ゲームスタートボタンの表示するまでの処理 </summary>
        private async UniTask DisplayingGameStartButton()
        {
            await UniTask.Delay(_delayTime);

            // DOFadeで使用する値
            float endValue = 1;
            float duration = 1;

            await _gameStartButtonCanvasGroup.DOFade(endValue, duration);

            _gameStartButton.onClick.AddListener(() => HandleChangeScreen(ScreenType.HomeMenu));
        }

    }
}
