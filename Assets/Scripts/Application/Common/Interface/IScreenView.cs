using Domain.Common;
using System;
using UnityEngine;

namespace Application.Common.Interface
{
    public interface IScreenView
    {
        public event Action<ScreenType> OnChangeScreen;
    }
}
