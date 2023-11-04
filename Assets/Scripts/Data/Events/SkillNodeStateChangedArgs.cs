using System;

namespace SkillTree.Data.Events
{
    public class SkillNodeStateChangedArgs : EventArgs
    {
        public SkillNode SkillNode;
        public bool Earned;
    }
}