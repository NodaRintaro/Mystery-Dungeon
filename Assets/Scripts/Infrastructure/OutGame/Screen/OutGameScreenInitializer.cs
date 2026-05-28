using Layer.Application;
using Layer.View;
using Layer.Domain;
using UnityEngine;

namespace Layer.Infrastructure
{
    public class OutGameScreenInitializer : MonoBehaviour
    {
        [Header("初期画面")]
        [SerializeField, Tooltip("最初に表示する画面")] private OutGameScreenType _startScreen = OutGameScreenType.Title;

        [Header("各種ScreenViewがアタッチされたGameObject")]
        [SerializeField, Tooltip("ScreenViewのGameObject")] private GameObject _screenView = null;

        private OutGameScreenController _screenController = null;
        private OutGameScreenChanger _screenChanger = null;

        private void Start()
        {
            if(_screenView.TryGetComponent<TitleScreenView>(out var titleScreen) 
                && _screenView.TryGetComponent<HomeMenuScreenView>(out var homeScreen) 
                && _screenView.TryGetComponent<QuestSelectMenuScreenView>(out var selectQuestScreen))
            {
                _screenChanger = new OutGameScreenChanger(titleScreen, homeScreen, selectQuestScreen);

                // 画面切り替えコントローラーの初期化
                _screenController = new OutGameScreenController(_screenChanger);

                titleScreen.Init(_screenController);
                homeScreen.Init(_screenController);
                selectQuestScreen.Init(_screenController);

                titleScreen.Hide();
                homeScreen.Hide();
                selectQuestScreen.Hide();
            }

            _screenChanger.ChangeScreen(_startScreen);
        }
    }
}
