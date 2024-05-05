using System;
using SkillTree.UI.Core;
using SkillTree.View;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkillTree.UI.Screens
{
    public class SkillGraphScreen : BaseScreen<SkillGraphPresenter>
    {
        [SerializeField] private TMP_Text _experiencePointsText;
        [SerializeField] private TMP_Text _skillCostText;
        [SerializeField] private Button _earnButton;
        [SerializeField] private Button _acclaimButton;
        [SerializeField] private Button _forgetButton;
        [SerializeField] private Button _forgetAllButton;
        private Guid _lastSelectedId;
        private SkillTreeView _skillTreeView;

        public void Initialize(SkillTreeView skillTreeView)
        {
            _skillTreeView = skillTreeView;
        }

        protected override void OnScreenShown()
        {
            Subscribe();
            RedrawButtons(_lastSelectedId);
            _skillTreeView.DisplayTree(Presenter.Skills);
        }

        protected override void OnScreenHidden()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            _earnButton.onClick.AddListener(Presenter.EarnXPPoints);
            _acclaimButton.onClick.AddListener(Presenter.AcclaimSelectedSkill);
            _forgetButton.onClick.AddListener(Presenter.ForgetSelectedSkill);
            _forgetAllButton.onClick.AddListener(Presenter.ForgetAllSkills);
            Presenter.SelectedSkillChanged += OnSelectedSkillChanged;
            Presenter.ExperienceChanged += OnExperienceChanged;
            _skillTreeView.SkillClicked += OnSkillClicked;
        }

        private void Unsubscribe()
        {
            _earnButton.onClick.RemoveListener(Presenter.EarnXPPoints);
            _acclaimButton.onClick.RemoveListener(Presenter.AcclaimSelectedSkill);
            _forgetButton.onClick.RemoveListener(Presenter.ForgetSelectedSkill);
            _forgetAllButton.onClick.RemoveListener(Presenter.ForgetAllSkills);
            Presenter.SelectedSkillChanged -= OnSelectedSkillChanged;
            Presenter.ExperienceChanged -= OnExperienceChanged;
        }

        private void OnSkillClicked(object sender, Guid skillId)
        {
            Presenter.SelectSkill(skillId);
        }

        private void OnSelectedSkillChanged(Guid skillId)
        {
            RedrawButtons(skillId);
            _skillTreeView.SelectSkill(skillId);
        }

        private void OnExperienceChanged(int experiencePoints)
        {
            _experiencePointsText.text = experiencePoints.ToString();
            RedrawButtons(Presenter.SelectedSkill);
        }

        private void RedrawButtons(Guid skillId)
        {
            if (skillId == default)
            {
                _skillCostText.text = string.Empty;
                _acclaimButton.interactable = false;
                _forgetButton.interactable = false;
                return;
            }
            SkillNodePresenter skill = Presenter.GetSkill(skillId);
            _skillCostText.text = skill.Data.EarnCost.ToString();
            _acclaimButton.interactable = Presenter.CanAcclaimSkill(skillId);
            _forgetButton.interactable = Presenter.CanForgetSkill(skillId);
        }
    }
}