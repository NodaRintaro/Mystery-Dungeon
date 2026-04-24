using JetBrains.Annotations;
using System.Collections.Generic;

/// <summary> ターン制コマンドバトルシステム </summary>
public class TurnBasedCommandBattleSystem
{
    private Dictionary<ICharacterData, uint> _characterList = new();

    private uint _baseActiveValue = 10000;

    /// <summary> 行動対象を追加する </summary>
    /// <param name="character"> 行動対象 </param>
    public void AddCharacter(ICharacterData character)
    {
        _characterList.Add(character, _baseActiveValue);
    }

    /// <summary> 次の行動者を選択 </summary>
    public void ChangeTurn()
    {

    }

    /// <summary> 次の行動者を速度を元に計算 </summary>
    public void CalculateNextActionCharacter()
    {

    }
}
