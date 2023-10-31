using SkillTree.UI.Core;

namespace SkillTree.UI.Screens
{
    public class SkillGraphPresenter : BasePresenter
    {
        private readonly SkillGraphModel _model;

        public SkillGraphPresenter(SkillGraphModel model)
        {
            _model = model;
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
    }
}