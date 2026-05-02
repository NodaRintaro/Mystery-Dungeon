using System;
using UnityEditor;
using UnityEngine;

namespace Domain
{
    [Serializable]
    /// <summary> キャラクターのデータ </summary>
    public class CharacterData : ICharacterData
    {
        /// <summary> キャラクターデータのコンストラクタ </summary>
        /// <param name="characterGridSize">キャラクターの占有するGridのサイズ</param>
        /// <param name="characterLevel">キャラクターのレベル</param>
        /// <param name="characterStatus">キャラクターのステータス</param>
        /// <param name="growthRates">キャラクターの成長曲線</param>
        public CharacterData(Vector3 characterPosition, int characterGridSize, int characterLevel, 
            SkillSlot skillSlot, CharacterStatus characterStatus, CharacterGrowthRates growthRates)
        {
            _growthData = new CharacterGrowthData(characterLevel, growthRates);
            _currentCharacterStatus = characterStatus;
            _characterSkillSlot = skillSlot;
            _characterPosition = characterPosition;
        }

        [SerializeField, Header("キャラクターの向き")]
        private CharacterDirectionType _characterDirection = CharacterDirectionType.Front;

        [SerializeField, Header("キャラクターのステータス")]
        private readonly CharacterStatus _currentCharacterStatus = null;

        [SerializeField, Header("キャラクターの成長曲線")]
        private readonly CharacterGrowthData _growthData = null;

        [SerializeField, Header("所持スキル")]
        private SkillSlot _characterSkillSlot = null;

        /// <summary> キャラクターの座標 </summary>
        private Vector3 _characterPosition = Vector3.zero;

        /// <summary> このキャラクターの行動可能フラグ </summary>
        private bool _canAction = false;

        /// <summary> キャラクターの占有するGridのサイズ </summary>
        private readonly int _characterGridSize = 0;

        #region 参照用プロパティ
        public bool CanAction => _canAction;
        public CharacterDirectionType CharacterDirection => _characterDirection;
        public Vector3 GridPosition => _characterPosition;
        public int GridSize => _characterGridSize;
        public CharacterStatus CurrentCharacterStatus => _currentCharacterStatus;
        public CharacterGrowthData GrowthData => _growthData;
        public SkillSlot CharacterSkillSlot => _characterSkillSlot;
        #endregion

        public void SetDirection(CharacterDirectionType characterDirection) => _characterDirection = characterDirection;
        public void SetPosition(Vector3 position) => _characterPosition = position;
    }

    #region キャラクターの成長データ
    /// <summary> キャラクターの成長データ </summary>
    public class CharacterGrowthData
    {
        public CharacterGrowthData(int characterLevel, CharacterGrowthRates characterGrowthRates)
        {
            _characterLevel = characterLevel;
            _characterGrowthRates = characterGrowthRates;
        }

        /// <summary> キャラクターのレベル </summary>
        private int _characterLevel;
        /// <summary> 次のレベルまでの経験値 </summary>
        private int _nextLevelXP;
        /// <summary> キャラクターの経験値 </summary>
        private int _characterXP;
        /// <summary> キャラクターの成長曲線 </summary>
        private CharacterGrowthRates _characterGrowthRates;

        #region 参照用プロパティ
        public int CharacterLevel => _characterLevel;
        public int NextLevelXP => _nextLevelXP;
        public int CharacterXP => _characterXP;
        #endregion

        /// <summary> キャラクターのレベルを増加させる </summary>
        /// <param name="addLevel"> 増加させるレベル </param>   
        public void AddCharacterLevel(int addLevel) => _characterLevel += addLevel;

        /// <summary> 次のレベルまでの経験値を増加させる </summary>
        /// <param name="addXP"> 増加させる経験値 </param>
        public void AddNextLevelupXP(int addXP) => _nextLevelXP += addXP;

        /// <summary> キャラクターの経験値を増加させる </summary>
        /// <param name="addXP"> 増加させる経験値 </param>
        public void AddCharacterXP(int addXP)  => _characterXP += addXP;

        public CharacterGrowthRates CharacterGrowthRates => _characterGrowthRates;
    }
    #endregion

    #region キャラクターステータス
    /// <summary> キャラクターのステータス </summary>
    [Serializable]
    public class CharacterStatus
    {
        public CharacterStatus(int characterID, string characterName, int maxHp, int maxMp, int atk, int matk, int def, int mdef, int speed, int criticalRate, int criticalDMG)
        {
            _characterID = characterID;
            _characterName = characterName;
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
        /// <summary> キャラクターの名前 </summary>
        private readonly string _characterName;
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
        public string CharacterName => _characterName;
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
        /// <summary> 最大HPを増加させる </summary>
        public void AddMaxHP(int hp) => _maxHp = hp;
        /// <summary> 現在のHPを増加させる </summary>
        public void AddHP(int hp) => _hp += hp;
        /// <summary> 現在のHPを減少させる </summary>
        public void Damage(int hp) => _hp -= hp;
        /// <summary> 最大MPを増加させる </summary>
        public void AddMaxMP(int mp) => _maxMp += mp;
        /// <summary> 現在のMPを増加させる </summary>
        public void AddMP(int mp) => _mp += mp;
        /// <summary> 現在のMPを減少させる </summary>
        public void ConsumeMP(int mp) => _mp -= mp;
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

    #region ステータスの表示用エディタ拡張
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(CharacterStatus))]
    public class CharacterStatusDrawer : PropertyDrawer
    {
        // 表示したいフィールド名（SerializeFieldがついた変数名）のリスト
        private readonly string[] _fieldNames = {
            "CharacterID",
            "CharacterName",
            "MaxHp",
            "HP",
            "MaxMp",
            "MP",
            "ATK",
            "MATK",
            "DEF",
            "MDEF",
            "SPEED",
            "CriticalRate",
            "CriticalDMG"
            };

        // インスペクター上のラベル表示名
        private readonly string[] _displayNames = {
            "Character ID",
            "Name",
            "Max HP",
            "Current HP",
            "Max MP",
            "Current MP",
            "ATK",
            "MATK",
            "DEF",
            "MDEF",
            "Speed",
            "Critical Rate (%)",
            "Critical DMG (%)"
            };

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            // 親ラベル（変数名）を表示
            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // インデントの深さを調整
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // --- 表示のみ（編集不可）の設定開始 ---
            EditorGUI.BeginDisabledGroup(true);

            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = 2f;
            Rect fieldRect = new Rect(position.x, position.y, position.width, lineHeight);

            for (int i = 0; i < _fieldNames.Length; i++)
            {
                SerializedProperty prop = property.FindPropertyRelative(_fieldNames[i]);

                if (prop != null)
                {
                    // 正確にフィールドを描画
                    EditorGUI.PropertyField(fieldRect, prop, new GUIContent(_displayNames[i]));
                    // 次の行へ移動
                    fieldRect.y += lineHeight + spacing;
                }
            }

            EditorGUI.EndDisabledGroup();
            // --- 表示のみの設定終了 ---

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // 全体の高さを計算（(行数 * 1行の高さ) + 行間の合計）
            float lineHeight = EditorGUIUtility.singleLineHeight;
            float spacing = 2f;
            return (lineHeight + spacing) * _fieldNames.Length;
        }
    }
#endif
    #endregion

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