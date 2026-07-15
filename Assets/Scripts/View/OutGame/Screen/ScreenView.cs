using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

using Domain.Common.Interface;
using Application.OutGame.Screen;
using Domain.Common;
using Application.Common.Interface;

namespace View.OutGame.Screen
{
    [Serializable]
    public abstract class ScreenView : MonoBehaviour, IScreenDisplayFunction, IScreenView
    {
        public event Action<ScreenType> OnChangeScreen;

        [SerializeField] protected GameObject _screenObject;

        protected ScreenPresenter _presenter;

        protected bool _isVisible = false;

        public abstract ScreenType ScreenType { get; }

        public bool IsVisible => _isVisible;

        public void Init(ScreenPresenter screenPresenter)
        {
            _presenter = screenPresenter;
        }

        public async virtual UniTask Show()
        {
            _screenObject.SetActive(true);
            _isVisible = true;
        }

        public async virtual UniTask Hide()
        {
            _screenObject.SetActive(false);
            _isVisible = false;
        }

        protected void HandleChangeScreen(ScreenType nextScreen)
        {
            OnChangeScreen?.Invoke(nextScreen);
        }
    }
}
