using System;
using System.Collections.Generic;
using R3;
using SkillTree.StaticData.Skills;

namespace SkillTree.Data
{
    public interface ISkill
    {
        Guid Id { get; }
        ReadOnlyReactiveProperty<bool> IsEarned { get; }
        SkillDefinition Data { get; }
        IEnumerable<ISkill> Nodes { get; }
    }
}