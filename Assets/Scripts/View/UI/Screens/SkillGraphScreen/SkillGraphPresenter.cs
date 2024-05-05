using System;
using System.Collections.Generic;
using System.Linq;
using SkillTree.Data;
using SkillTree.Settings;
using SkillTree.UI.Core;

namespace SkillTree.UI.Screens
{
    public class SkillGraphPresenter : BasePresenter
    {
        public event Action<Guid> SelectedSkillChanged;
        public event Action<int> ExperienceChanged;

        public IReadOnlyList<SkillNodePresenter> Skills => _skills;
        public int ExperiencePoints => _gameState.Experience.ExperiencePoints;
        public Guid SelectedSkill { get; private set; }

        private readonly List<SkillNodePresenter> _skills;
        private readonly GameState _gameState;
        private readonly SkillGraphProgress _skillGraphProgress;

        public SkillGraphPresenter(GameState gameState, SkillGraphProgress skillGraphProgress)
        {
            _skillGraphProgress = skillGraphProgress;
            _gameState = gameState;
            _skills = _skillGraphProgress.Nodes.Select(s => new SkillNodePresenter(s)).ToList();
        }

        public override void Initialize()
        {
            base.Initialize();
            Subscribe();
        }

        protected override void OnDispose()
        {
            base.OnDispose();
            Unsubscribe();
        }

        public void EarnXPPoints()
        {
            _gameState.Experience.Add(GameSettings.ExperienceGainAmount);
        }

        public void AcclaimSelectedSkill()
        {
            if (false == _skillGraphProgress.TryEarnSkill(SelectedSkill))
            {
                throw new InvalidOperationException($"Can't earn skill[{SelectedSkill}]");
            }
        }

        public void ForgetSelectedSkill()
        {
            if (false == _skillGraphProgress.TryForgetSkill(SelectedSkill))
            {
                throw new InvalidOperationException($"Can't forget skill[{SelectedSkill}]");
            }
        }

        public void ForgetAllSkills()
        {
            _skillGraphProgress.ForgetAll();
        }

        public SkillNodePresenter GetSkill(Guid skillId)
        {
            return _skills.First(s => s.Id == skillId);
        }

        public bool CanAcclaimSkill(Guid skillId)
        {
            return CanAcclaimSkill(_skillGraphProgress.Get(skillId));
        }

        public bool CanAcclaimSkill(SkillNode node)
        {
            SkillNode skillNode = node;
            bool canEarn = _skillGraphProgress.CanEarn(skillNode);
            bool enoughExperience = _gameState.Experience.ExperiencePoints >= skillNode.Data.EarnCost;
            return canEarn && enoughExperience;
        }

        public bool CanForgetSkill(Guid skillId)
        {
            return _skillGraphProgress.CanForget(skillId);
        }

        public void SelectSkill(Guid skillId)
        {
            if (SelectedSkill != skillId)
            {
                SelectedSkill = skillId;
                SelectedSkillChanged?.Invoke(skillId);
            }
        }

        private void Subscribe()
        {
            _gameState.Experience.ExperienceChanged += OnExperienceChanged;
        }

        private void Unsubscribe()
        {
            _gameState.Experience.ExperienceChanged -= OnExperienceChanged;
        }

        private void OnExperienceChanged(int xp)
        {
            ExperienceChanged?.Invoke(xp);
        }
    }
}