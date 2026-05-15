using UnityEngine;

namespace Layer.Domain
{
    public interface ISkillData : IData
    {
        public string Description { get; }
    }
}
