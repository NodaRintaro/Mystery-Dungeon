using Domain.Common.Interface;
using System;
using UniRx;
using UnityEngine;

namespace Domain.InGame.Quest
{
    /// <summary> クエストの進行状況を管理するクラス </summary>
    public class QuestProgressManager
    {
        /// <summary> 現在進行中のQuestData </summary>
        private IQuestData _currentQuestData;

        /// <summary> 現在攻略中のダンジョンの階数 </summary>
        private ReactiveProperty<int> _currentDungeonFloor = new ReactiveProperty<int>(0);

        public IQuestData CurrentQuestData => _currentQuestData;
        public IReadOnlyReactiveProperty<int> CurrentDungeonFloor => _currentDungeonFloor;

        /// <summary> 現在のダンジョンの階層を変更する </summary>
        /// <param name="changeNum"> 次の階層の進む数 </param>
        public void ChangeFloorNum(int changeNum = 1)
        {
            if (changeNum == 0 || changeNum + _currentDungeonFloor.Value <= 0)
            {
                Debug.LogError($"ダンジョンの階数がマイナスの値に設定されました。");
                return;
            }

            if (changeNum + _currentDungeonFloor.Value > _currentQuestData.DungeonFloorNum)
            {

            }

            _currentDungeonFloor.Value += changeNum;
        }
    }
}
