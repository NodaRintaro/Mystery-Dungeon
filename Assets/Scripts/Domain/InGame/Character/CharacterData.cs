using Domain.InGame;
using Domain.Common.Interface;
using Domain.InGame.Skill;
using System;
using UniRx;
using UnityEngine;

namespace Domain.InGame.Character
{
    [Serializable]
    /// <summary> キャラクターのデータ </summary>
    public struct CharacterData : ICharacterData
    {
        /// <summary> キャラクターデータのコンストラクタ </summary>
        /// <param name="characterGridSize">キャラクターの占有するGridのサイズ</param>
        /// <param name="characterLevel">キャラクターのレベル</param>
        /// <param name="characterStatus">キャラクターのステータス</param>
        /// <param name="growthRates">キャラクターの成長曲線</param>
        public CharacterData(int id, string name, 
            Vector3 characterPosition, int characterGridSize, int characterLevel, 
            SkillSlot skillSlot, CharacterStatus characterStatus, CharacterGrowthRates growthRates)
        {
            _id = id;
            _characterName = name;
            _characterPosition = new(characterPosition);
            _characterGridSize = characterGridSize;
            _growthData = new CharacterGrowthData(characterLevel, growthRates);
            _currentCharacterStatus = characterStatus;
            _characterSkillSlot = skillSlot;
            _canAction = false;
            _characterDirection = new(CharacterDirectionType.Front);
        }

        [SerializeField, Tooltip("キャラクターのID")]
        private int _id;

        [SerializeField, Tooltip("キャラクターの名前")]
        private string _characterName;

        [SerializeField, Tooltip("キャラクターのステータス")]
        private readonly CharacterStatus _currentCharacterStatus;

        [SerializeField, Tooltip("キャラクターの成長曲線")]
        private readonly CharacterGrowthData _growthData;

        [SerializeField, Tooltip("所持スキル")]
        private SkillSlot _characterSkillSlot;

        /// <summary> キャラクターの向いている方向 </summary>
        private ReactiveProperty<CharacterDirectionType> _characterDirection;

        /// <summary> キャラクターの座標 </summary>
        private ReactiveProperty<Vector3> _characterPosition;

        /// <summary> このキャラクターの行動可能フラグ </summary>
        private bool _canAction;

        /// <summary> キャラクターの占有するGridのサイズ </summary>
        private readonly int _characterGridSize;

        #region 参照用プロパティ
        public int ID => _id;
        public string Name => _characterName;
        public CharacterStatus CurrentCharacterStatus => _currentCharacterStatus;
        public CharacterGrowthData GrowthData => _growthData;
        public SkillSlot CharacterSkillSlot => _characterSkillSlot;
        public bool CanAction => _canAction;
        public IReadOnlyReactiveProperty<CharacterDirectionType> CharacterDirection => _characterDirection;
        public IReadOnlyReactiveProperty<Vector3> GridPosition => _characterPosition;
        public int GridSize => _characterGridSize;
        #endregion

        public void SetDirection(CharacterDirectionType characterDirection) => _characterDirection.Value = characterDirection;
        public void SetPosition(Vector3 position) => _characterPosition.Value = position;
    }

    #region キャラクターの成長データ
    /// <summary> キャラクターの成長データ </summary>
    public struct CharacterGrowthData
    {
        public CharacterGrowthData(int characterLevel, CharacterGrowthRates growthRates)
        {
            _characterLevel = new(characterLevel);
            _characterXP = new(0);
            _nextLevelXP = 0;
            _characterGrowthRates = growthRates;
        }

        /// <summary> キャラクターのレベル </summary>
        private ReactiveProperty<int> _characterLevel;
        /// <summary> キャラクターの経験値 </summary>
        private ReactiveProperty<int> _characterXP;
        /// <summary> 次のレベルまでの経験値 </summary>
        private int _nextLevelXP;
        /// <summary> キャラクターの成長曲線 </summary>
        private CharacterGrowthRates _characterGrowthRates;

        #region 参照用プロパティ
        public IReactiveProperty<int> CharacterLevel => _characterLevel;
        public IReactiveProperty<int> CharacterXP => _characterXP;
        public int NextLevelXP => _nextLevelXP;
        #endregion

        /// <summary> キャラクターのレベルを増加させる </summary>
        /// <param name="addLevel"> 増加させるレベル </param>   
        public void AddCharacterLevel(int addLevel) => _characterLevel.Value += addLevel;

        /// <summary> 次のレベルまでの経験値を増加させる </summary>
        /// <param name="addXP"> 増加させる経験値 </param>
        public void AddNextLevelupXP(int addXP) => _nextLevelXP += addXP;

        /// <summary> キャラクターの経験値を増加させる </summary>
        /// <param name="addXP"> 増加させる経験値 </param>
        public void AddCharacterXP(int addXP)  => _characterXP.Value += addXP;

        public CharacterGrowthRates CharacterGrowthRates => _characterGrowthRates;
    }
    #endregion

    #region キャラクターステータス
    /// <summary> キャラクターのステータス </summary>
    [Serializable]
    public struct CharacterStatus
    {
        public CharacterStatus(int maxHp, int maxMp, int atk, int matk, int def, int mdef, int speed, int criticalRate, int criticalDMG)
        {
            _maxHp = maxHp;
            _currentHP = new(maxHp);
            _maxMp = maxMp;
            _currentMP = new(maxMp);
            _atk = atk;
            _matk = matk;
            _def = def;
            _mdef = mdef;
            _speed = speed;
            _criticalRate = criticalRate;
            _criticalDMG = criticalDMG;
        }

        /// <summary> 現在の体力 </summary>
        private ReactiveProperty<int> _currentHP;
        /// <summary> 現在の魔力 </summary>
        private ReactiveProperty<int> _currentMP;
        /// <summary> 体力の最大値 </summary>
        private int _maxHp;
        /// <summary> 魔力の最大値 </summary>
        private int _maxMp;
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
        public IReadOnlyReactiveProperty<int> CurrentHP => _currentHP;
        public IReadOnlyReactiveProperty<int> CurrentMP => _currentMP;
        public int MaxHp => _maxHp;
        public int MaxMp => _maxMp;
        public int ATK => _atk;
        public int MATK => _matk;
        public int DEF => _def;
        public int MDEF => _mdef;
        public int SPEED => _speed;
        public int CriticalRate => _criticalRate;
        public int CriticalDMG => _criticalDMG;

        #endregion

        #region 各ステータスの計算用関数
        /// <summary> 最大HPを増加させる </summary>
        public void AddMaxHP(int hp) => _maxHp = hp;
        /// <summary> 現在のHPを増加させる </summary>
        public void AddHP(int hp) => _currentHP.Value += hp;
        /// <summary> 現在のHPを減少させる </summary>
        public void Damage(int hp) => _currentHP.Value -= hp;
        /// <summary> 最大MPを増加させる </summary>
        public void AddMaxMP(int mp) => _maxMp += mp;
        /// <summary> 現在のMPを増加させる </summary>
        public void AddMP(int mp) => _currentMP.Value += mp;
        /// <summary> 現在のMPを減少させる </summary>
        public void ConsumeMP(int mp) => _currentMP.Value -= mp;
        /// <summary> 物理攻撃力を増加させる </summary>
        public void AddATK(int atk) => _atk += atk;
        /// <summary> 魔術攻撃力を増加させる </summary>
        public void AddMATK(int matk) => _matk += matk;
        /// <summary> 物理防御力を増加させる </summary>
        public void AddDEF(int def) => _def += def;
        /// <summary> 魔術防御力を増加させる </summary>
        public void AddMDEF(int mdef) => _mdef += mdef;
        /// <summary> 行動速度を増加させる </summary>
        public void AddSpeed(int speed) => _speed += speed;
        /// <summary> 会心率を増加させる </summary>
        public void AddCriticalRate(int criticalRate) => _criticalRate += criticalRate;
        /// <summary> 会心ダメージを増加させる </summary>
        public void AddCriticalDMG(int criticalDMG) => _criticalDMG += criticalDMG;
        #endregion
    }

    #endregion

    #region キャラクターの成長曲線
    public struct CharacterGrowthRates
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
