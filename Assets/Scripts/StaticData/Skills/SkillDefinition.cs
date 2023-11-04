using System;
using System.Numerics;

namespace SkillTree.StaticData.Skills
{
    public class SkillDefinition
    {
        public readonly Guid Id;
        public readonly string Name;
        public readonly Vector2 Position;
        public readonly int EarnCost;

        public SkillDefinition(Guid id, string name, Vector2 position, int earnCost)
        {
            Id = id;
            Name = name;
            Position = position;
            EarnCost = earnCost;
        }
    }
}