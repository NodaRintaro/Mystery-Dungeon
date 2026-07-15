using UnityEngine;
using Cysharp.Threading.Tasks;
using Domain.Common;
using Domain.OutGame.Screen;
using Application.OutGame.Screen;
using View.OutGame.Screen;
using View.Common.Screen;
using Domain.Common.Interface;
using Application.Common.Interface;

namespace InGame.Infrastructure
{
    public class OutGameScreenInitializer : MonoBehaviour
    {
        [Header("初期画面")]
        [SerializeField, Tooltip("最初に表示する画面")] private ScreenType _startScreen = ScreenType.Title;

        [Header("各種ScreenView")]
        [SerializeField] ScreenView[] _screenViews = null;

        private ScreenPresenter _screenPresenter = null;
        private IScreenChanger _screenChanger = null;

        private void Start()
        {
            // 画面切り替えシステム
            _screenChanger = new OutGameScreenChanger(_screenViews);

            // 画面切り替えコントローラーの初期化
            _screenPresenter = new ScreenPresenter(_screenChanger, _screenViews);

            // 各スクリーンの初期化
            foreach (var view in _screenViews)
            {
                view.Init(_screenPresenter);
            }

            // 最初の画面を表示
            _screenChanger.ChangeScreen(_startScreen).Forget();
        }
    }
}
