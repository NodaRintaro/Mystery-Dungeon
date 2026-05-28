using Layer.Domain;

namespace Layer.Application
{
    public class OutGameScreenController
    {
        public OutGameScreenController(OutGameScreenChanger screenChanger)
        {
            _screenChanger = screenChanger;
        }

        private OutGameScreenChanger _screenChanger = null;



        /// <summary> Screen切り替えイベント発火時のメソッド </summary>
        /// <param name="screenType"> 切り替える画面の種類 </param>
        public void HandleScreenChange(OutGameScreenType screenType)
        {


            _screenChanger.ChangeScreen(screenType);
        }
    }
}
