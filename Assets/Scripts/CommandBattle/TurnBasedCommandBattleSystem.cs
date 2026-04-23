using JetBrains.Annotations;
using System.Collections.Generic;

public class TurnBasedCommandBattleSystem
{
    private Dictionary<ICharacterCommander, uint> _commanderList = new();

    private uint _baseActiveValue = 10000;

    /// <summary> 行動対象を追加する </summary>
    /// <param name="commander"> 行動対象 </param>
    public void AddCommander(ICharacterCommander commander)
    {
        _commanderList.Add(commander, _baseActiveValue);
    }

    /// <summary> 次の行動者を選択 </summary>
    public void ChangeTurn()
    {
        ICharacterCommander commander;

        
    }

    public void CalculateNextTurnCommander()
    {

    }
}
