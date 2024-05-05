using System;
using System.Collections.Generic;
using R3;
using SkillTree.StaticData.Skills;

namespace SkillTree.Data
{
    public class SkillNode : ISkill
    {
        public Guid Id => Data.Id;
        public IEnumerable<ISkill> Nodes => _nodes;
        public SkillDefinition Data { get; }
        public ReadOnlyReactiveProperty<bool> IsEarned => _isEarned;

        private readonly ReactiveProperty<bool> _isEarned;
        private readonly List<SkillNode> _nodes = new();

        public SkillNode(SkillDefinition data, bool earned = false)
        {
            Data = data;
            _isEarned = new ReactiveProperty<bool>(earned);
        }

        public void SetEarned(bool value)
        {
            if (_isEarned.Value != value)
            {
                _isEarned.Value = value;
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
    }
}