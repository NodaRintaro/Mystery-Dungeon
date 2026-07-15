using System.Collections.Generic;
using Cysharp.Threading.Tasks;

using Domain.Common;
using Domain.Common.Interface;

namespace Domain.OutGame.Screen
{
    /// <summary> アウトゲームの画面切り替えクラス </summary>
    public class OutGameScreenChanger : IScreenChanger
    {
        public OutGameScreenChanger(IScreenDisplayFunction[] screens)
        {
            _screens = screens;

            // Screenの表示が一度に重複するのを防ぐため隠しておく
            foreach (var screen in _screens)
            {
                screen.Hide();
                
                if(screen.ScreenType == ScreenType.Fade)
                {
                    _fadeScreen = screen;
                }
            }
        }

        // 各種画面
        private readonly IScreenDisplayFunction[] _screens;

        private IScreenDisplayFunction _currentDisplyScreen = null;
        private readonly IScreenDisplayFunction _fadeScreen;

        /// <summary> 画面を切り替える </summary>
        public async UniTask ChangeScreen(ScreenType nextScreenType)
        {
            IScreenDisplayFunction nextScreen = null;

            // 次の表示画面が切り替え不可能な画面なら切り替え失敗とみなす
            if (nextScreenType == ScreenType.Fade 
                || nextScreenType == ScreenType.None) return;

            // 次の表示画面を探す
            foreach (var screen in _screens)
            {
                if(screen.ScreenType == nextScreenType)
                {
                    nextScreen = screen;
                }
            }
            
            // 現在のScreenTypeがNoneなら遷移先のScreenをノータイムで表示する
            if (_currentDisplyScreen == null)
            {
                nextScreen.Show().Forget();
                _currentDisplyScreen = nextScreen;
                return;
            }

            // Playerから画面切り替えの瞬間を隠しながら画面を遷移する
            await _fadeScreen.Show();
                
            await _currentDisplyScreen.Hide();

            await nextScreen.Show();

            await _fadeScreen.Hide();

            _currentDisplyScreen = nextScreen;
        }
    }
}
