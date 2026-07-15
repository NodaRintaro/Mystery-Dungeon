using System;
using UnityEngine;

namespace Domain.InGame.Dungeon
{
    [Serializable]
    public struct DungeonInformation
    {
        /// <summary> Dungeonの階数 </summary>
        public int CurrentFloor;

        /// <summary> 1つのセクションの大きさ </summary>
        public int SectionRange;

        /// <summary> 生成する部屋の最小サイズ </summary>
        public int MinRoomNum;

        /// <summary> 生成する部屋の最大サイズ </summary>
        public int MaxRoomNum;

        /// <summary> 横に生成するセクションの数多い </summary>
        public int HorizontalSectionsNum;

        /// <summary> 縦に生成するセクションの数多い </summary>
        public int VerticalSectionsNum;

        /// <summary> ダンジョンに余分に追加する通路の本数 </summary>
        public int AddRoadNum;

        /// <summary> 生成する部屋の種類 </summary>
        public string RoomDataPath;
    }
}
