using System;
using System.Collections.Generic;
using SkillTree.Data.Events;
using SkillTree.StaticData.Skills;

namespace SkillTree.Data
{
    public class SkillNode : ISkill
    {
        public event EventHandler<SkillNodeStateChangedArgs> StateChanged;
        public Guid Id => Data.Id;
        public IEnumerable<ISkill> Nodes => _nodes;
        public bool Earned { get; private set; }
        public SkillDefinition Data { get; }
        private readonly List<SkillNode> _nodes = new();

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
            _nodes.Add(node);
        }

        public void RemoveConnection(SkillNode node)
        {
            _nodes.Remove(node);
        }

        private void InvokeStateChanged()
        {
            StateChanged?.Invoke(this, new SkillNodeStateChangedArgs() {Skill = this, Earned = Earned});
        }
    }
}