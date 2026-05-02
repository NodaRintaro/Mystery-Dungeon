using UnityEngine;

namespace Domain
{
    public interface ICharacterData : IOnGridObject
    {
        /// <summary> このキャラクターの行動可能フラグ </summary>
        public bool CanAction { get; }

        /// <summary> キャラクター現在向いている方向 </summary>
        public CharacterDirectionType CharacterDirection { get; }

        /// <summary> キャラクターの現在のステータス </summary>
        public CharacterStatus CurrentCharacterStatus { get; }

        /// <summary> キャラクターの成長曲線 </summary>
        public CharacterGrowthData GrowthData { get; }

        /// <summary> 所持スキル </summary>
        public SkillSlot CharacterSkillSlot { get; }

        /// <summary> キャラクターの向きを設定する </summary>
        /// <param name="characterDirection">設定する向き</param>
        public void SetDirection(CharacterDirectionType characterDirection);
    }
}