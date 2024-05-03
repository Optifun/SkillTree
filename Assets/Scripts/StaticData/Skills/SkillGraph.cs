using System;
using System.Collections.Generic;

namespace SkillTree.StaticData.Skills
{
    public class SkillGraph
    {
        public string Name { get; set; }
        public Guid BaseSkill { get; set; }
        public List<SkillDefinition> Skills { get; set; }
        public List<SkillConnection> Connections { get; set; }
    }
}