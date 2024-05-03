using System;

namespace SkillTree.StaticData.Skills
{
    public class SkillConnection
    {
        public Guid Source { get; private set; }
        public Guid Target { get; private set; }

        public SkillConnection(Guid source, Guid target)
        {
            Source = source;
            Target = target;
        }
    }
}