using UnityEngine;

namespace Layer.Domain.SkillData
{
    /// <summary> スキルデータ </summary>
    public class SkillData : ISkillData
    {
        /// <summary> スキルデータ </summary>
        /// <param name="skillID">  </param>
        /// <param name="skillName">  </param>
        /// <param name="skillDescription">  </param>
        public SkillData(int skillID, string skillName, string skillDescription)
        {
            _skillID = skillID;
            _skillName = skillName;
            _skillDescription = skillDescription;
        }

        [SerializeField, Tooltip("スキルのID")] 
        private int _skillID;

        [SerializeField, Tooltip("スキルの名前")] 
        private string _skillName;

        [SerializeField, Tooltip("スキルの説明")] 
        private string _skillDescription;

        public int ID => _skillID;
        public string Name => _skillName;
        public string Description => _skillDescription;
    }
}
