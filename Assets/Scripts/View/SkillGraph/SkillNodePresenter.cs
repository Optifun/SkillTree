using System;
using System.Collections.Generic;
using R3;
using SkillTree.Data;
using SkillTree.StaticData.Skills;

namespace SkillTree.UI.Screens
{
    public class SkillNodePresenter : ISkill
    {
        public Guid Id => _skill.Id;
        public SkillDefinition Data => _skill.Data;
        public IEnumerable<ISkill> Nodes => _skill.Nodes;
        public ReadOnlyReactiveProperty<bool> IsEarned => _skill.IsEarned;

        private readonly ISkill _skill;

        public SkillNodePresenter(ISkill skill)
        {
            _skill = skill;
        }
    }
}