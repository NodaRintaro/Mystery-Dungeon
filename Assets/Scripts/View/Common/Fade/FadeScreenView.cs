using Cysharp.Threading.Tasks;
using DG.Tweening;
using Domain.Common;
using UnityEngine;
using UnityEngine.UI;

using View.OutGame.Screen;

namespace View.Common.Screen
{
    public class FadeScreenView : ScreenView
    {
        [Header("FadeOut時の暗幕")]
        [SerializeField, Tooltip("FadeOut時の暗幕")] private Image _fadeImage;

        [Header("FadeOut,FadeInにかける時間")]
        [SerializeField] private float _fadeOutDuration;
        [SerializeField] private float _fadeInDuration;

        // Fade後のアルファ値の最終版値
        private readonly float _fadeOutEndValue = 1;
        private readonly float _fadeInEndValue = 0;

        public override ScreenType ScreenType => ScreenType.Fade;

        private void Awake()
        {
            // Showが呼ばれてないときは常に画面を隠しておく
            Color currentColor = _fadeImage.color;
            currentColor.a = _fadeInEndValue;
            _fadeImage.color = currentColor;
        }

        public override async UniTask Show()
        {
            base.Show().Forget();

            await _fadeImage.DOFade(_fadeOutEndValue, _fadeOutDuration);
        }

        public override async UniTask Hide()
        {
            await _fadeImage.DOFade(_fadeInEndValue, _fadeOutDuration);

            base.Hide().Forget();
        }
    }
}
