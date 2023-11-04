using System;
using System.Collections.Generic;
using SkillTree.Data;
using SkillTree.UI.Core;

namespace SkillTree.UI.Screens
{
    public class SkillGraphPresenter : BasePresenter
    {
        public event Action<int> ExperienceChanged;
        public event Action<Guid> SelectedSkillChanged;
        public IEnumerable<ISkill> Skills => _model.Skills;
        public Guid SelectedSkill => _model.SelectedSkill;
        private readonly SkillGraphModel _model;

        public SkillGraphPresenter(SkillGraphModel model)
        {
            _model = model;
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
            _model.EarnXPPoints();
        }

        public void AcclaimSelectedSkill()
        {
            _model.AcclaimSkill(_model.SelectedSkill);
        }

        public void ForgetSelectedSkill()
        {
            _model.ForgetSkill(_model.SelectedSkill);
        }

        public void ForgetAllSkills()
        {
            _model.ForgetAllSkills();
        }

        public ISkill GetSkill(Guid skillId)
        {
            return _model.GetSkill(skillId);
        }

        public bool CanAcclaimSkill(Guid skillId)
        {
            return _model.CanAcclaimSkill(skillId);
        }

        public bool CanForgetSkill(Guid skillId)
        {
            return _model.CanForgetSkill(skillId);
        }

        public void SelectSkill(Guid skillId)
        {
            _model.SetSelection(skillId);
        }

        private void Subscribe()
        {
            _model.SelectedSkillChanged += OnSelectedSkillChanged;
            _model.ExperienceState.ExperienceChanged += OnExperienceChanged;
        }

        private void Unsubscribe()
        {
            _model.SelectedSkillChanged -= OnSelectedSkillChanged;
            _model.ExperienceState.ExperienceChanged -= OnExperienceChanged;
        }

        private void OnSelectedSkillChanged(Guid skillId)
        {
            SelectedSkillChanged?.Invoke(skillId);
        }

        private void OnExperienceChanged(int xp)
        {
            ExperienceChanged?.Invoke(xp);
        }
    }
}