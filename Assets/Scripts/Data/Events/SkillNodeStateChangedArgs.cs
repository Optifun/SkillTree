using System;

namespace SkillTree.Data.Events
{
    public class SkillNodeStateChangedArgs : EventArgs
    {
        public ISkill Skill;
        public bool Earned;
    }
}