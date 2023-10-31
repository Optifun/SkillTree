using System;
using SkillTree.Data;

namespace SkillTree.UI.Screens
{
    public class SkillGraphModel
    {
        public event Action<Guid> SelectedSkillChanged;
        public Guid SelectedSkill { get; private set; }
        private GameState _gameState;

        public SkillGraphModel(GameState gameState)
        {
            _gameState = gameState;
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
        }

        public void AcclaimSkill(Guid skillId)
        {
        }

        public void ForgetSkill(Guid skillId)
        {
        }

        public void ForgetAllSkills()
        {
        }
    }
}