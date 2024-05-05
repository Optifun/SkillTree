using System;
using System.Collections.Generic;
using SkillTree.Data;
using SkillTree.Data.Events;
using SkillTree.StaticData.Skills;

namespace SkillTree.UI.Screens
{
    public class SkillNodePresenter : ISkill
    {
        public event EventHandler<SkillNodeStateChangedArgs> StateChanged
        {
            add => _skill.StateChanged += value;
            remove => _skill.StateChanged -= value;
        }

        public Guid Id => _skill.Id;
        public bool Earned => _skill.Earned;
        public SkillDefinition Data => _skill.Data;

        public IEnumerable<ISkill> Nodes => _skill.Nodes;

        private readonly ISkill _skill;

        public SkillNodePresenter(ISkill skill)
        {
            _skill = skill;
        }
    }
}