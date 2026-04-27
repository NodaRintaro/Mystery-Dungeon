using UnityEngine;
using Domain;


namespace Domain
{
public interface ICharacterData
{
    public bool CanAction { get; }
    public CharacterDirectionType CharacterDirection { get; }
    public Vector3 GridPosition { get; }
    public CharacterStatus CurrentCharacterStatus { get; }
    public CharacterGrowthRates GrowthRates { get; }
    public void SetDirection(CharacterDirectionType characterDirection);
    public void SetPosition(Vector3 position);
}
}



