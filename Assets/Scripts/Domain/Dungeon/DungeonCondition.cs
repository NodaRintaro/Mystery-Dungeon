using System;
using UnityEngine;

namespace Layer.Domain
{
    [Serializable]
    public class DungeonCondition
    {
        // 各セクションのサイズ（一辺の長さ）
        [Header("各セクションのサイズ")]
        [SerializeField] private int _sectionRange = 10;

        // 生成する部屋の最小・最大数
        [Header("部屋の最小生成数")]
        [SerializeField] private int _minRoomNum = 5;
        [Header("部屋の最大生成数")]
        [SerializeField] private int _maxRoomNum = 10;

        // ダンジョンのセクション分割数（横・縦）
        [Header("横方向のセクション数")]
        [SerializeField] private int _horizontalSectionsNum = 5;
        [Header("縦方向のセクション数")]
        [SerializeField] private int _verticalSectionsNum = 5;

        // 基本の連結以外に追加で生成する通路の数
        [Header("追加の通路数")]
        [SerializeField] private int _addRoadNum = 5;

        // 部屋データおよびマップデータのロードパス
        [Header("部屋データのパス")]
        [SerializeField] private string _roomDataPath = "DefaultDungeonRoom";

        #region ゲッター
        public int SectionRange => _sectionRange;
        public int MinRoomNum => _minRoomNum;
        public int MaxRoomNum => _maxRoomNum;
        public int HorizontalSectionsNum => _horizontalSectionsNum;
        public int VerticalSectionsNum => _verticalSectionsNum;
        public int AddRoadNum => _addRoadNum;
        public string RoomDataPath => _roomDataPath;
        #endregion
    }
}
