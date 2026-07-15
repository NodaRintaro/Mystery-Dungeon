using UnityEngine;

namespace Domain.Common.Interface
{
    public interface ISkillData
    {
        public int ID { get; }

        public string Name { get; }

        public string Description { get; }
    }
}
