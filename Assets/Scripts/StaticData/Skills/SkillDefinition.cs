using System;
using System.Numerics;

namespace SkillTree.StaticData.Skills
{
    public class SkillDefinition
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public Vector2 Position { get; private set; }
        public int EarnCost { get; private set; }

        public SkillDefinition(Guid id, string name, Vector2 position, int earnCost)
        {
            Id = id;
            Name = name;
            Position = position;
            EarnCost = earnCost;
        }
    }
}