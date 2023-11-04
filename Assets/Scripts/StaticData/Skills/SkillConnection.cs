using System;

namespace SkillTree.StaticData.Skills
{
    public class SkillConnection
    {
        public readonly Guid Source;
        public readonly Guid Target;

        public SkillConnection(Guid source, Guid target)
        {
            Source = source;
            Target = target;
        }
    }
}