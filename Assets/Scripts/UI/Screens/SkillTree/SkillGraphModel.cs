using System;
using SkillTree.Data;
using SkillTree.Data.Events;
using SkillTree.Settings;

namespace SkillTree.UI.Screens
{
    public class SkillGraphModel
    {
        public event Action<Guid> SelectedSkillChanged;
        public event Action<int> ExperienceChanged;
        public Guid SelectedSkill { get; private set; }
        public ExperienceState ExperienceState => _gameState.Experience;
        private readonly GameState _gameState;
        private readonly SkillGraphProgress _skillGraphProgress;

        public SkillGraphModel(GameState gameState, SkillGraphProgress skillGraphProgress)
        {
            _skillGraphProgress = skillGraphProgress;
            _gameState = gameState;
            skillGraphProgress.SkillStateChanged += OnSkillStateChanged;
        }

        public void SetSelection(Guid id)
        {
            if (SelectedSkill != id)
            {
                SelectedSkill = id;
                SelectedSkillChanged?.Invoke(id);
            }
        }

        public void EarnXPPoints()
        {
            _gameState.Experience.Add(GameSettings.ExperienceGainAmount);
        }

        public void AcclaimSkill(Guid skillId)
        {
            SkillNode skillNode = _skillGraphProgress.Get(skillId);
            if (false == _skillGraphProgress.TryEarnSkill(skillId))
            {
                throw new InvalidOperationException($"Can't earn skill[{skillNode.Id}]");
            }
        }

        public void ForgetSkill(Guid skillId)
        {
            SkillNode skillNode = _skillGraphProgress.Get(skillId);
            if (false == _skillGraphProgress.TryForgetSkill(skillId))
            {
                throw new InvalidOperationException($"Can't forget skill[{skillNode.Id}]");
            }
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
            bool enoughExperience = _gameState.Experience.ExperiencePoints >= skillNode.Data.EarnCost;
            return canEarn && enoughExperience;
        }

        private void OnSkillStateChanged(object sender, SkillNodeStateChangedArgs e)
        {
            if (e.Earned)
            {
                _gameState.Experience.Add(-e.Skill.Data.EarnCost);
            }
            else
            {
                _gameState.Experience.Add(e.Skill.Data.EarnCost);
            }
        }

        public ISkill GetSkill(Guid skillId)
        {
            return _skillGraphProgress.Get(skillId);
        }

        public bool CanForgetSkill(Guid skillId)
        {
            return _skillGraphProgress.CanForget(skillId);
        }
    }
}