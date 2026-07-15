using Application.Common.Interface;
using Cysharp.Threading.Tasks;
using Domain.Common;
using Domain.Common.Interface;

namespace Application.OutGame.Screen
{
    public class ScreenPresenter
    {
        public ScreenPresenter(IScreenChanger screenChanger, IScreenView[] screenViews)
        {
            _screenChanger = screenChanger;
            Init(screenViews);
        }

        private IScreenChanger _screenChanger = null;

        /// <summary> 初期化 </summary>
        private void Init(IScreenView[] screenViews)
        {
            foreach (var view in screenViews)
            {
                view.OnChangeScreen += HandleChangeScreen;
            }
        }

        /// <summary> Screen切り替えイベント発火時のメソッド </summary>
        /// <param name="screenType"> 切り替える画面の種類 </param>
        private void HandleChangeScreen(ScreenType screenType)
        {
            if (_screenChanger != null) _screenChanger.ChangeScreen(screenType).Forget();
        }
    }
}
