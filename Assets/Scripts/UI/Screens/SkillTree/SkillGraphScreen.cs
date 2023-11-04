using System;
using SkillTree.Data;
using SkillTree.UI.Core;
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

        protected override void OnScreenShown()
        {
            _earnButton.onClick.AddListener(Presenter.EarnXPPoints);
            _acclaimButton.onClick.AddListener(Presenter.AcclaimSelectedSkill);
            _forgetButton.onClick.AddListener(Presenter.ForgetSelectedSkill);
            _forgetAllButton.onClick.AddListener(Presenter.ForgetAllSkills);
            Presenter.SelectedSkillChanged += OnSelectedSkillChanged;
            Presenter.ExperienceChanged += OnExperienceChanged;
            RedrawButtons(_lastSelectedId);
        }

        protected override void OnScreenHidden()
        {
            _earnButton.onClick.RemoveListener(Presenter.EarnXPPoints);
            _acclaimButton.onClick.RemoveListener(Presenter.AcclaimSelectedSkill);
            _forgetButton.onClick.RemoveListener(Presenter.ForgetSelectedSkill);
            _forgetAllButton.onClick.RemoveListener(Presenter.ForgetAllSkills);
            Presenter.SelectedSkillChanged -= OnSelectedSkillChanged;
            Presenter.ExperienceChanged -= OnExperienceChanged; 
        }

        private void OnSelectedSkillChanged(Guid skillId)
        {
            _lastSelectedId = skillId;
            RedrawButtons(skillId);
        }

        private void OnExperienceChanged(int experiencePoints)
        {
            _experiencePointsText.text = experiencePoints.ToString();
            RedrawButtons(_lastSelectedId);
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
            ISkill skill = Presenter.GetSkill(skillId);
            _skillCostText.text = skill.Data.EarnCost.ToString();
            _acclaimButton.interactable = Presenter.CanAcclaimSkill(skillId);
            _forgetButton.interactable = Presenter.CanForgetSkill(skillId);
        }
    }
}