using System;
using System.Collections.Generic;

namespace SkillTree.StaticData.Skills
{
    public class SkillGraph
    {
        public string Name;
        public Guid BaseSkill;
        public List<SkillDefinition> Skills;
        public List<SkillConnection> Connections;
    }
}