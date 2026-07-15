using UnityEngine;
using Cysharp.Threading.Tasks;
using System;

namespace Domain.Common.Interface
{
    /// <summary> 画面の表示機能インターフェース </summary>
    public interface IScreenDisplayFunction
    {
        /// <summary> 継承先のスクリーンの種類 </summary>
        ScreenType ScreenType { get; }
        /// <summary> 画面が表示されているか </summary>
        bool IsVisible { get; }
        /// <summary> 画面を表示する </summary>
        UniTask Show();
        /// <summary> 画面を隠す </summary>
        UniTask Hide();
    }
}