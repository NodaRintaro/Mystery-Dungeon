using Layer.Application;
using System;
using UnityEngine;

namespace Layer.View
{
    [Serializable]
    public class ScreenView : MonoBehaviour, IScreen
    {
        [SerializeField] protected GameObject _screenObject;

        protected OutGameScreenController _controller;

        protected bool _isVisible = false;

        public bool IsVisible => _isVisible;

        public void Init(OutGameScreenController outGameScreenController)
        {
            _controller = outGameScreenController;
        }

        public virtual void Show()
        {
            _screenObject.SetActive(true);
            _isVisible = true;
        }

        public virtual void Hide()
        {
            _screenObject.SetActive(false);
            _isVisible = false;
        }
    }
}
