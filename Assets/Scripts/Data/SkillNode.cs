using System;
using System.Collections.Generic;
using SkillTree.StaticData.Skills;

namespace SkillTree.Data
{
    public class SkillNode
    {
        public event EventHandler<bool> EarnedChanged;
        public Guid Id => Data.Id;
        public List<SkillNode> Nodes { get; } = new();
        public bool Earned { get; private set; }
        public SkillDefinition Data { get; }

        public SkillNode(SkillDefinition data, bool earned = false)
        {
            Earned = earned;
            Data = data;
        }

        public void SetEarned(bool value)
        {
            if (Earned != value)
            {
                Earned = value;
                EarnedChanged?.Invoke(this, value);
            }
        }

        public void AddConnection(SkillNode node)
        {
            Nodes.Add(node);
        }

        public void RemoveConnection(SkillNode node)
        {
            Nodes.Remove(node);
        }
    }
}