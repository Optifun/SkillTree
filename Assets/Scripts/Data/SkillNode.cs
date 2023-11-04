using System;
using System.Collections.Generic;
using SkillTree.Data.Events;
using SkillTree.StaticData.Skills;

namespace SkillTree.Data
{
    public class SkillNode
    {
        public event EventHandler<SkillNodeStateChangedArgs> StateChanged;
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
                InvokeStateChanged();
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

        private void InvokeStateChanged()
        {
            StateChanged?.Invoke(this, new SkillNodeStateChangedArgs(){SkillNode = this, Earned = Earned});
        }
    }
}