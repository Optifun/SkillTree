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

        protected override void OnScreenShown()
        {
            _earnButton.onClick.AddListener(Presenter.EarnXPPoints);
            _acclaimButton.onClick.AddListener(Presenter.AcclaimSelectedSkill);
            _forgetButton.onClick.AddListener(Presenter.ForgetSelectedSkill);
            _forgetAllButton.onClick.AddListener(Presenter.ForgetAllSkills);
        }

        protected override void OnScreenHidden()
        {
        }
    }
}