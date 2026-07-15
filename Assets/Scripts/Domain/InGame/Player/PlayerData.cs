using UnityEngine;
using System;

using Domain.Common;
using Domain.Common.Interface;
using Domain.Common.System;

namespace Domain.InGame.Player
{
    [Serializable]
    public struct PlayerData : IPlayerData
    {
        public PlayerData(string name, int jobValue, int equippedWeaponsID)
        {
            _playerName = name;
            _playerJob = default;
            _playerJob = EnumUtility.GetValue<PlayerJobType>(jobValue);
            _equippedWeaponsID = equippedWeaponsID;
        }

        [SerializeField, Header("プレイヤーの名前")]
        private string _playerName;

        [SerializeField, Header("プレイヤーの役職")]
        private PlayerJobType _playerJob;

        [SerializeField, Header("装備している武器のID")]
        private int _equippedWeaponsID;

        public string Name => _playerName;
        public PlayerJobType PlayerJob => _playerJob;
        public int EquippedWeaponsID => _equippedWeaponsID;

        public void Load(IPlayerData playerData)
        {
            _playerName = playerData.Name;
            _playerJob = playerData.PlayerJob;
            _equippedWeaponsID = playerData.EquippedWeaponsID;
        }
    }
}
