namespace Domain.Common.Interface
{
    public interface IPlayerData : ISaveData<IPlayerData>
    {
        /// <summary>
        /// プレイヤーネーム
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// プレイヤーの役職
        /// </summary>
        public PlayerJobType PlayerJob { get; }

        /// <summary>
        /// 装備している武器のID
        /// </summary>
        public int EquippedWeaponsID { get; }
    }
}
