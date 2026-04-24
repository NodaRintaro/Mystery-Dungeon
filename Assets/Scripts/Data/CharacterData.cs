using UnityEngine;
using System;

namespace Data.Character
{
    /// <summary>
    /// キャラクターのデータ
    /// </summary>
    [Serializable]
    public class CharacterData : ICharacterData
    {
        /// <summary> コンストラクタ </summary>
        public CharacterData(CharacterStatus characterStatus, CharacterGrowthRates growthRates)
        {
            _growthRates = growthRates;
            _currentCharacterStatus = characterStatus;
        }

        [SerializeField, Header("キャラクターの向き")]
        private CharacterDirectionType _characterDirection = CharacterDirectionType.Front;

        /// <summary> キャラクターのステータス </summary>
        [SerializeField, Header("キャラクターのステータス")]
        private readonly CharacterStatus _currentCharacterStatus;

        /// <summary> キャラクターの成長曲線 </summary>
        [SerializeField, Header("キャラクターの成長曲線")]
        private readonly CharacterGrowthRates _growthRates;

        [SerializeField, Header("所持スキル")]
        private SkillSlot _characterSkillSlot = new SkillSlot();

        /// <summary> キャラクターの座標 </summary>
        private Vector3 _characterPosition = new Vector3();

        /// <summary> このキャラクターの行動可能フラグ </summary>
        private bool _canAction = false;

        public bool CanAction => _canAction;
        public CharacterDirectionType CharacterDirection => _characterDirection;
        public Vector3 GridPosition => _characterPosition;
        public CharacterStatus CurrentCharacterStatus => _currentCharacterStatus;
        public CharacterGrowthRates GrowthRates => _growthRates;
        public SkillSlot CharacterSkillSlot => _characterSkillSlot;

        public void SetDirection(CharacterDirectionType characterDirection) => _characterDirection = characterDirection;
        public void SetPosition(Vector3 position) => _characterPosition = position;
    }

    #region キャラクターステータス
    /// <summary> キャラクターのステータス </summary>
    [Serializable]
    public class CharacterStatus
    {
        public CharacterStatus(int characterID, int characterLevel, int nextLevelXP, int characterXP, int maxHp, int maxMp, int atk, int matk, int def, int mdef, int speed, int criticalRate, int criticalDMG)
        {
            _characterID = characterID;
            _characterLevel = characterLevel;
            _nextLevelXP = nextLevelXP;
            _characterXP = characterXP;
            _maxHp = maxHp;
            _hp = maxHp;
            _maxMp = maxMp;
            _mp = maxMp;
            _atk = atk;
            _matk = matk;
            _def = def;
            _mdef = mdef;
            _speed = speed;
            _criticalRate = criticalRate;
            _criticalDMG = criticalDMG;
        }

        /// <summary> キャラクターのID </summary>
        private readonly int _characterID;
        /// <summary> キャラクターのレベル </summary>
        private int _characterLevel;
        /// <summary> 次のレベルまでの経験値 </summary>
        private int _nextLevelXP;
        /// <summary> キャラクターの経験値 </summary>
        private int _characterXP;
        /// <summary> 体力の最大値 </summary>
        private int _maxHp;
        /// <summary> 体力 </summary>
        private int _hp;
        /// <summary> 魔力の最大値 </summary>
        private int _maxMp;
        /// <summary> 魔力 </summary>
        private int _mp;
        /// <summary> 物理攻撃力 </summary>
        private int _atk;
        /// <summary> 魔術攻撃力 </summary>
        private int _matk;
        /// <summary> 物理防御力 </summary>
        private int _def;
        /// <summary> 魔術防御力 </summary>
        private int _mdef;
        /// <summary> 行動速度 </summary>
        private int _speed;
        /// <summary> 会心率 </summary>
        private int _criticalRate;
        /// <summary> 会心ダメージ </summary>
        private int _criticalDMG;

        #region 参照用プロパティ
        public int CharacterID => _characterID;
        public int CharacterLevel => _characterLevel;
        public int NextLevelXP => _nextLevelXP;
        public int CharacterXP => _characterXP;
        public int MaxHp => _maxHp;
        public int HP => _hp;
        public int MaxMp => _maxMp;
        public int MP => _mp;
        public int ATK => _atk;
        public int MATK => _matk;
        public int DEF => _def;
        public int MDEF => _mdef;
        public int SPEED => _speed;
        public int CriticalRate => _criticalRate;
        public int CriticalDMG => _criticalDMG;

        #endregion

        #region 各ステータスの計算用関数
        public void AddCharacterLevel(int level) => _characterLevel += level;
        public void AddNextLevelupXP(int xp) => _nextLevelXP += xp;
        public void AddCharacterXP(int xp) => _characterXP += xp;
        public void AddMaxHP(int hp) => _maxHp = hp;
        public void AddHP(int hp) => _hp += hp;
        public void Damage(int hp) => _hp -= hp;
        public void AddMaxMP(int mp) => _maxMp += mp;
        public void AddMP(int mp) => _mp += mp;
        public void ConsumeMP(int mp) => _mp -= mp;
        public void AddATK(int atk) => _atk += atk;
        public void AddMATK(int matk) => _matk += matk;
        public void AddDEF(int def) => _def += def;
        public void AddMDEF(int mdef) => _mdef += mdef;
        public void AddSpeed(int speed) => _speed += speed;
        public void AddCriticalRate(int criticalRate) => _criticalRate += criticalRate;
        public void AddCriticalDMG(int criticalDMG) => _criticalDMG += criticalDMG;

        #endregion
    }
    #endregion

    #region キャラクターの成長曲線
    public class CharacterGrowthRates
    {
        public CharacterGrowthRates(int hpGrowthRate, int mpGrowthRate, int atkGrowthRate, int matkGrowthRate, int defGrowthRate, int mdefGrowthRate, int speedGrowthRate)
        {
            _hpGrowthRate = hpGrowthRate;
            _mpGrowthRate = mpGrowthRate;
            _atkGrowthRate = atkGrowthRate;
            _matkGrowthRate = matkGrowthRate;
            _defGrowthRate = defGrowthRate;
            _mdefGrowthRate = mdefGrowthRate;
            _speedGrowthRate = speedGrowthRate;
        }

        /// <summary> レベルアップ時のHPの成長率 </summary>
        private float _hpGrowthRate;
        /// <summary> レベルアップ時のMPの成長率 </summary>
        private float _mpGrowthRate;
        /// <summary> レベルアップ時のATKの成長率 </summary>
        private float _atkGrowthRate;
        /// <summary> レベルアップ時のATKの成長率 </summary>
        private float _matkGrowthRate;
        /// <summary> レベルアップ時のDEFの成長率 </summary>
        private float _defGrowthRate;
        /// <summary> レベルアップ時のMDEFの成長率 </summary>
        private float _mdefGrowthRate;
        /// <summary> レベルアップ時のSPEEDの成長率 </summary>
        private float _speedGrowthRate;

        public float HpGrowthRate => _hpGrowthRate;
        public float MpGrowthRate => _mpGrowthRate;
        public float AtkGrowthRate => _atkGrowthRate;
        public float MatkGrowthRate => _matkGrowthRate;
        public float DefGrowthRate => _defGrowthRate;
        public float MdefGrowthRate => _mdefGrowthRate;
        public float SpeedGrowthRate => _speedGrowthRate;

        #region 成長率の増加用関数
        public void AddHpGrowthRate(float hpGrowthRate) => _hpGrowthRate += hpGrowthRate;
        public void AddMpGrowthRate(float mpGrowthRate) => _mpGrowthRate += mpGrowthRate;
        public void AddAtkGrowthRate(float atkGrowthRate) => _atkGrowthRate += atkGrowthRate;
        public void AddMatkGrowthRate(float matkGrowthRate) => _matkGrowthRate += matkGrowthRate;
        public void AddDefGrowthRate(float defGrowthRate) => _defGrowthRate += defGrowthRate;
        public void AddMdefGrowthRate(float mdefGrowthRate) => _mdefGrowthRate += mdefGrowthRate;
        public void AddSpeedGrowthRate(float speedGrowthRate) => _speedGrowthRate += speedGrowthRate;
        #endregion
    }
    #endregion
}