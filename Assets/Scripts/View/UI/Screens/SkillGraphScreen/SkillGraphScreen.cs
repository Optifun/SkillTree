using System;
using JetBrains.Annotations;
using SkillTree.Data;
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

        private SkillTreeView _skillTreeView;

        public void Initialize(SkillTreeView skillTreeView)
        {
            _skillTreeView = skillTreeView;
        }

        protected override void OnScreenShown()
        {
            Subscribe();
            RedrawButtons();
            _skillTreeView.DisplayTree(Presenter.Skills);
        }

        private void Subscribe()
        {
            Bind(_earnButton.onClick, Presenter.EarnXPPoints);
            Bind(_acclaimButton.onClick, Presenter.AcclaimSelectedSkill);
            Bind(_forgetButton.onClick, Presenter.ForgetSelectedSkill);
            Bind(_forgetAllButton.onClick, Presenter.ForgetAllSkills);
            Bind(_skillTreeView.SkillClicked, OnSkillClicked);

            Bind(Presenter.SelectedSkill, OnSelectedSkillChanged);
            Bind(Presenter.ExperiencePoints, OnExperienceChanged);
        }

        private void OnSkillClicked(Guid skillId)
        {
            Presenter.SelectSkill(skillId);
        }

        private void OnSelectedSkillChanged([CanBeNull] ISkill skill)
        {
            RedrawButtons();
            _skillTreeView.SelectSkill(skill?.Id);
        }

        private void OnExperienceChanged(int experiencePoints)
        {
            _experiencePointsText.text = experiencePoints.ToString();
            RedrawButtons();
        }

        private void RedrawButtons()
        {
            Guid selectedId = Presenter.SelectedSkillId;
            if (selectedId == default)
            {
                _skillCostText.text = string.Empty;
                _acclaimButton.interactable = false;
                _forgetButton.interactable = false;
                return;
            }

            var earnCost = Presenter.SelectedSkill.CurrentValue?.Data.EarnCost;
            _skillCostText.text = earnCost?.ToString() ?? string.Empty;
            
            _acclaimButton.interactable = Presenter.CanAcclaimSkill(selectedId);
            _forgetButton.interactable = Presenter.CanForgetSkill(selectedId);
        }
    }
}