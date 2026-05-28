using System.Collections.Generic;
using UnityEngine;
using System;

namespace Layer.Domain
{
    public enum OutGameScreenType
    {
        Title,
        HomeMenu,
        SelectQuestMenu,
    }

    /// <summary> アウトゲームの画面切り替えクラス </summary>
    public class OutGameScreenChanger
    {
        public OutGameScreenChanger(
            IScreen title,
            IScreen homeMenu,
            IScreen selectQuestMenu)
        {
            _screenDictionary.Add(OutGameScreenType.Title, title);
            _screenDictionary.Add(OutGameScreenType.HomeMenu, homeMenu);
            _screenDictionary.Add(OutGameScreenType.SelectQuestMenu, selectQuestMenu);
        }

        // 画面の種類と画面オブジェクトを紐づける辞書
        private readonly Dictionary<OutGameScreenType, IScreen> _screenDictionary = new Dictionary<OutGameScreenType, IScreen>();

        /// <summary> 画面を切り替える </summary>
        public void ChangeScreen(OutGameScreenType screenType)
        {


            foreach (var screen in _screenDictionary)
            {
                if(screenType == screen.Key)
                {
                    screen.Value.Show();
                }
                else if(screen.Value.IsVisible)
                {
                    screen.Value.Hide();
                }
            }
        }
    }
}
