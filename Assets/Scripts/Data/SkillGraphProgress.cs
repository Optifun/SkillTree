using System;
using System.Collections.Generic;
using System.Linq;
using SkillTree.Data.Events;
using SkillTree.StaticData.Skills;

namespace SkillTree.Data
{
    public class SkillGraphProgress : IDisposable
    {
        public event EventHandler<SkillNodeStateChangedArgs> SkillStateChanged;
        public IReadOnlyList<SkillNode> Nodes => _nodes;
        public readonly SkillNode GraphRoot;
        private readonly SkillNode[] _nodes;

        public SkillGraphProgress(SkillGraph skillGraph)
        {
            _nodes = skillGraph.Skills.Select(s =>
            {
                SkillNode skillNode = new SkillNode(s);
                skillNode.StateChanged += OnSkillStateChanged;
                return skillNode;
            }).ToArray();
            foreach (SkillConnection connection in skillGraph.Connections)
            {
                SkillNode source = Get(connection.Source);
                SkillNode target = Get(connection.Target);
                source.AddConnection(target);
                target.AddConnection(source);
            }
            GraphRoot = Get(skillGraph.BaseSkill);
            GraphRoot.SetEarned(true);
        }

        public bool TryEarnSkill(Guid skillId)
        {
            SkillNode node = Get(skillId);
            if (false == CanEarn(node))
            {
                return false;
            }
            node.SetEarned(true);
            return true;
        }

        public bool TryForgetSkill(Guid skillId)
        {
            SkillNode node = Get(skillId);
            if (false == CanForget(node))
            {
                return false;
            }
            node.SetEarned(false);
            return true;
        }

        public void ForgetAll()
        {
            List<ISkill> visited = new(); // can be pooled
            Stack<ISkill> stack = new();  // can be pooled
            stack.Push(GraphRoot);

            while (stack.TryPop(out ISkill source))
            {
                visited.Add(source);
                if (source != GraphRoot && source.Earned && source is SkillNode skillNode)
                {
                    skillNode.SetEarned(false);
                }

                foreach (ISkill node in source.Nodes)
                {
                    if (false == visited.Contains(node))
                    {
                        stack.Push(node);
                    }
                }
            }
        }

        public SkillNode Get(Guid skillId)
        {
            return _nodes.First(n => n.Id == skillId);
        }

        public bool CanEarn(Guid skillId)
        {
            SkillNode node = Get(skillId);
            return CanEarn(node);
        }

        public bool CanEarn(SkillNode node)
        {
            if (node == GraphRoot || node.Earned)
            {
                return false;
            }
            return CanReachFrom(node, GraphRoot, n => n.Earned);
        }

        public bool CanForget(Guid skillId)
        {
            return CanForget(Get(skillId));
        }

        public bool CanForget(ISkill node)
        {
            if (node == GraphRoot || false == node.Earned)
            {
                return false;
            }
            IEnumerable<ISkill> earnedPeers = node.Nodes.Where(n => n.Earned);
            foreach (var peer in earnedPeers)
            {
                if (false == CanReachFrom(peer, GraphRoot, n => n.Earned && n != node))
                {
                    return false;
                }
            }
            return true;
        }

        private static bool CanReachFrom(ISkill source, ISkill destination, Func<ISkill, bool> predicate)
        {
            List<ISkill> visited = new(); // can be pooled
            Stack<ISkill> stack = new();  // can be pooled
            stack.Push(source);

            while (stack.TryPop(out ISkill node))
            {
                visited.Add(node);
                if (node == destination)
                {
                    return true;
                }
                foreach (ISkill peer in node.Nodes)
                {
                    if (false == visited.Contains(peer) && predicate(peer))
                    {
                        stack.Push(peer);
                    }
                }
            }
            return false;
        }

        private void OnSkillStateChanged(object sender, SkillNodeStateChangedArgs e)
        {
            SkillStateChanged?.Invoke(this, e);
        }

        public void Dispose()
        {
            foreach (SkillNode skillNode in _nodes)
            {
                skillNode.StateChanged -= OnSkillStateChanged;
            }
        }
    }
}