using System;

namespace SkillTree.Data
{
    public class ExperienceState
    {
        public event Action<int> ExperienceChanged;
        public int ExperiencePoints { get; private set; }

        public void Set(int value)
        {
            ExperiencePoints = value;
            ExperienceChanged?.Invoke(ExperiencePoints);
        }

        public void Add(int value)
        {
            Set(ExperiencePoints + value);
        }
    }
}