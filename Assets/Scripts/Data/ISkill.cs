using System;
using System.Collections.Generic;
using SkillTree.Data.Events;
using SkillTree.StaticData.Skills;

namespace SkillTree.Data
{
    public interface ISkill
    {
        event EventHandler<SkillNodeStateChangedArgs> StateChanged;
        Guid Id { get; }
        bool Earned { get; }
        SkillDefinition Data { get; }
        IEnumerable<ISkill> Nodes { get; }
    }
}