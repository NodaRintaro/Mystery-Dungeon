﻿using UnityEngine;

namespace Layer.Domain
{
    public interface ISkillData
    {
        public int SkillID { get; }
        public string SkillName { get; }
        public string SkillDescription { get; }
    }
}
