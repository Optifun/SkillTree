using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using SkillTree.Data;
using SkillTree.Settings;
using SkillTree.UI.Core;

namespace SkillTree.UI.Screens
{
    public class SkillGraphPresenter : BasePresenter
    {
        public IReadOnlyList<SkillNodePresenter> Skills => _skills;
        public ReadOnlyReactiveProperty<int> ExperiencePoints => _gameState.Experience.ExperiencePoints;
        public ReadOnlyReactiveProperty<SkillNodePresenter?> SelectedSkill => _selectedSkill;
        public Guid SelectedSkillId => _selectedSkill.Value?.Id ?? Guid.Empty;

        private readonly List<SkillNodePresenter> _skills;
        private readonly GameState _gameState;
        private readonly SkillGraphProgress _skillGraphProgress;
        private readonly ReactiveProperty<SkillNodePresenter?> _selectedSkill = new();

        public SkillGraphPresenter(GameState gameState, SkillGraphProgress skillGraphProgress)
        {
            _skillGraphProgress = skillGraphProgress;
            _gameState = gameState;
            _skills = _skillGraphProgress.Nodes.Select(s => new SkillNodePresenter(s)).ToList();
        }

        public void EarnXPPoints()
        {
            _gameState.Experience.Add(GameSettings.ExperienceGainAmount);
        }

        public void AcclaimSelectedSkill()
        {
            if (false == _skillGraphProgress.TryEarnSkill(SelectedSkillId))
            {
                throw new InvalidOperationException($"Can't earn skill[{SelectedSkillId}]");
            }

            var skill = GetSkill(SelectedSkillId);
            _gameState.Experience.Decrease(skill.Data.EarnCost);
        }

        public void ForgetSelectedSkill()
        {
            if (false == _skillGraphProgress.TryForgetSkill(SelectedSkillId))
            {
                throw new InvalidOperationException($"Can't forget skill[{SelectedSkillId}]");
            }

            var skill = GetSkill(SelectedSkillId);
            _gameState.Experience.Add(skill.Data.EarnCost);
        }

        public void ForgetAllSkills()
        {
            _skillGraphProgress.ForgetAll();
        }

        public bool CanAcclaimSkill(Guid skillId)
        {
            return CanAcclaimSkill(_skillGraphProgress.Get(skillId));
        }

        public bool CanAcclaimSkill(SkillNode node)
        {
            SkillNode skillNode = node;
            bool canEarn = _skillGraphProgress.CanEarn(skillNode);
            bool enoughExperience = _gameState.Experience.ExperiencePoints.CurrentValue >= skillNode.Data.EarnCost;
            return canEarn && enoughExperience;
        }

        public bool CanForgetSkill(Guid skillId)
        {
            return _skillGraphProgress.CanForget(skillId);
        }

        public void SelectSkill(Guid skillId)
        {
            _selectedSkill.Value = GetSkill(skillId);
        }

        private SkillNodePresenter GetSkill(Guid skillId)
        {
            return _skills.First(s => s.Id == skillId);
        }
    }
}