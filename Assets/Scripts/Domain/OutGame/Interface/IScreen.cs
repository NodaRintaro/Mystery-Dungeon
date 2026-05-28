using UnityEngine;

public interface IScreen
{
    /// <summary> 画面が表示されているか </summary>
    bool IsVisible { get; }
    /// <summary> 画面を表示する </summary>
    void Show();
    /// <summary> 画面を隠す </summary>
    void Hide();
}
