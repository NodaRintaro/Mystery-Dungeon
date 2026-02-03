using UnityEngine;

/// <summary>
/// キャラクターのベースデータ
/// </summary>
[System.Serializable]
public class CharacterBaseData
{
    [Header("キャラクターの名前")]
    [SerializeField]
    private string _characterName = null;

    [Header("キャラクターのレベル")]
    [SerializeField]
    private int _lv = 1;

    [Header("キャラクターの経験値")]
    [SerializeField]
    private int _xp = 0;

    [Header("次のレベルに行くために必要な経験値量の基礎値")]
    [SerializeField]
    private int _nextLvXp = 0;

    [Header("ステータス")]
    [SerializeField]
    private CharacterStatus _characterStatus;

    public string CharacterName => _characterName;
    public int Lv => _lv;
    public int Xp => _xp;
    public int NextLvXp => _nextLvXp;
    public CharacterStatus CharacterStatus => _characterStatus;

    /// <summary>
    /// ステータスの初期化
    /// </summary>
    /// <param name="hp">HP</param>
    /// <param name="mp">MP</param>
    /// <param name="atk">攻撃力</param>
    /// <param name="matk">魔術攻撃力</param>
    /// <param name="def">防御力</param>
    /// <param name="mdef">魔術防御力</param>
    /// <param name="speed">速度</param>
    /// <param name="dex">命中率</param>
    /// <param name="eva">回避率</param>
    /// <param name="criticalRate">会心率</param>
    /// <param name="criticalDMG">会心ダメージ</param>
    public void InitStatus(int hp, int mp, int atk, int matk, int def, int mdef, int speed, int dex, int eva, int criticalRate, int criticalDMG)
    {
        _characterStatus.SetMaxHP(hp);
        _characterStatus.SetHP(hp);
        _characterStatus.SetMaxMp(mp);
        _characterStatus.SetMP(mp);
        _characterStatus.SetATK(atk);
        _characterStatus.SetMATK(matk);
        _characterStatus.SetDEF(def);
        _characterStatus.SetMDEF(mdef);
        _characterStatus.SetSpeed(speed);
        _characterStatus.SetDEX(dex);
        _characterStatus.SetEVA(eva);
        _characterStatus.SetCriticalRate(criticalRate);
        _characterStatus.SetCriticalDMG(criticalDMG);
    }


}

/// <summary>
/// キャラクターのステータス
/// </summary>
[System.Serializable]
public class CharacterStatus
{
    /// <summary> 体力の最大値 </summary>
    [Header("体力の最大値")]
    [SerializeField]
    private int _maxHp;

    /// <summary> 体力 </summary>
    [Header("体力")]
    [SerializeField]
    private int _hp;

    /// <summary> 魔力の最大値 </summary>
    [Header("魔力の最大値")]
    [SerializeField]
    private int _maxMp;

    /// <summary> 魔力 </summary>
    [Header("魔力")]
    [SerializeField]
    private int _mp;

    /// <summary> 物理攻撃力 </summary>
    [Header("物理攻撃力")]
    [SerializeField]
    private int _atk;

    /// <summary> 魔術攻撃力 </summary>
    [Header("魔術攻撃力")]
    [SerializeField]
    private int _mAtk;

    /// <summary> 物理防御力 </summary>
    [Header("物理防御力")]
    [SerializeField]
    private int _def;

    /// <summary> 魔術防御力 </summary>
    [Header("魔術防御力")]
    [SerializeField]
    private int _mDef;

    /// <summary> 行動速度 </summary>
    [Header("行動速度")]
    [SerializeField]
    private int _speed;

    /// <summary> 命中率 </summary>
    [Header("命中率")]
    [SerializeField]
    private int _dex;

    /// <summary> 回避率 </summary>
    [Header("回避率")]
    [SerializeField]
    private int _eva;

    /// <summary> 会心率 </summary>
    [Header("会心率")]
    [SerializeField]
    private int _criticalRate;

    /// <summary> 会心ダメージ </summary>
    [Header("会心ダメージ")]
    [SerializeField]
    private int _criticalDMG;

    #region 参照用プロパティ

    public int MaxHp => _maxHp;
    public int HP => _hp;
    public int MaxMp => _maxMp;
    public int MP => _mp;
    public int ATK => _atk;
    public int MATK => _mAtk;
    public int DEF => _def;
    public int MDEF => _mDef;
    public int SPEED => _speed;
    public int DEX => _dex;
    public int EVA => _eva;
    public int CriticalRate => _criticalRate;
    public int CriticalDMG => _criticalDMG;

    #endregion

    #region 各ステータスのセッター

    public void SetMaxHP(int hp) => _maxHp = hp;
    public void SetHP(int hp) => _hp = hp;
    public void SetMaxMp(int mp) => _maxMp = mp;
    public void SetMP(int mp) => _mp = mp;
    public void SetATK(int atk) => _atk = atk;
    public void SetMATK(int matk) => _mAtk = matk;
    public void SetDEF(int def) => _def = def;
    public void SetMDEF(int mdef) => _mDef = mdef;
    public void SetSpeed(int speed) => _speed = speed;
    public void SetDEX(int dex) => _dex = dex;
    public void SetEVA(int eva) => _eva = eva;
    public void SetCriticalRate(int criticalRate) => _criticalRate = criticalRate;
    public void SetCriticalDMG(int criticalDMG) => _criticalDMG = criticalDMG;

    #endregion

    #region 各ステータスの計算用関数

    public void AddMaxHP(int hp) => _maxHp = hp;
    public void AddHP(int hp) => _hp += hp;
    public void AddMaxMP(int mp) => _maxMp += mp;
    public void AddMP(int mp) => _mp += mp;
    public void AddATK(int atk) => _atk += atk;
    public void AddMATK(int matk) => _mAtk += matk;
    public void AddDEF(int def) => _def += def;
    public void AddMDEF(int mdef) => _mDef += mdef;
    public void AddSpeed(int speed) => _speed += speed;
    public void AddDEX(int dex) => _dex += dex;
    public void AddEVA(int eva) => _eva += eva;
    public void AddCriticalRate(int criticalRate) => _criticalRate += criticalRate;
    public void AddCriticalDMG(int criticalDMG) => _criticalDMG += criticalDMG;

    #endregion
}

