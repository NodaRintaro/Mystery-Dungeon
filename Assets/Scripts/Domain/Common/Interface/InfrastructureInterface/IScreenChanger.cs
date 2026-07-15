using Cysharp.Threading.Tasks;

namespace Domain.Common.Interface
{
    /// <summary> 表示画面切り替え機能 </summary>
    public interface IScreenChanger
    {
        /// <summary> 表示画面の切り替え </summary>
        /// <param name="nextScreenType"> 次に表示される画面の種類 </param>
        UniTask ChangeScreen(ScreenType nextScreenType);
    }
}
