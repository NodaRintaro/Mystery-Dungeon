using UnityEngine;
using UnityEngine.UI;
using Layer.Domain;

namespace Layer.View
{
    public class TitleScreenView : ScreenView
    {
        [Header("Home画面に遷移するButton")]
        [SerializeField, Tooltip("Home画面に遷移するButton")] private Button HomeMenuButton = null;

        private void Start()
        {
            HomeMenuButton.onClick.AddListener(() => _controller.HandleScreenChange(OutGameScreenType.HomeMenu));
        }
    }
}
