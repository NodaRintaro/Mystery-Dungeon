using System;
using UnityEngine;

namespace Layer.Domain
{
    [Serializable]
    public struct DungeonCondition
    {
        [Header("各セクションのサイズ")]
        [SerializeField, Tooltip("各セクションのサイズ")] private int _sectionRange;

        [Header("生成する部屋の最小・最大数")]
        [SerializeField, Tooltip("最小数")] private int _minRoomNum;
        [SerializeField, Tooltip("最大数")] private int _maxRoomNum;

        [Header(" ダンジョンのセクション分割数（横・縦）")]
        [SerializeField, Tooltip("横")] private int _horizontalSectionsNum;
        [SerializeField, Tooltip("縦")] private int _verticalSectionsNum;

        [Header("基本の連結以外に追加で生成する通路の数")]
        [SerializeField, Tooltip("追加で生成する通路の数")] private int _addRoadNum;

        #region ゲッター
        public int SectionRange => _sectionRange;
        public int MinRoomNum => _minRoomNum;
        public int MaxRoomNum => _maxRoomNum;
        public int HorizontalSectionsNum => _horizontalSectionsNum;
        public int VerticalSectionsNum => _verticalSectionsNum;
        public int AddRoadNum => _addRoadNum;
        #endregion
    }
}
