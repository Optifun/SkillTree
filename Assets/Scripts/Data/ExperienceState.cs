using R3;

namespace SkillTree.Data
{
    public class ExperienceState
    {
        public ReadOnlyReactiveProperty<int> ExperiencePoints => _experiencePoints;

        private readonly ReactiveProperty<int> _experiencePoints = new();

        public void Add(int value)
        {
            Set(_experiencePoints.Value + value);
        }

        public void Decrease(int value)
        {
            Add(-value);
        }

        public void Set(int value)
        {
            _experiencePoints.Value = value;
        }
    }
}