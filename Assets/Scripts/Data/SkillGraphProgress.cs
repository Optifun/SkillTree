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
            ForgetAllInternal(GraphRoot, new List<SkillNode>());
        }

        private void ForgetAllInternal(SkillNode source, List<SkillNode> visited)
        {
            visited.Add(source);
            if (source != GraphRoot && source.Earned)
            {
                source.SetEarned(false);
            }
            foreach (SkillNode node in source.Nodes)
            {
                if (false == visited.Contains(node))
                {
                    ForgetAllInternal(node, visited);
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
            return CanReachFrom(GraphRoot, node, new List<SkillNode>(), n => n.Earned);
        }

        public bool CanForget(Guid skillId)
        {
            return CanForget(Get(skillId));
        }

        public bool CanForget(SkillNode node)
        {
            if (node == GraphRoot)
            {
                return false;
            }
            return CanReachFrom(node, GraphRoot, new List<SkillNode>(), n => n.Earned && n != node);
        }

        private static bool CanReachFrom(SkillNode source, SkillNode destination, List<SkillNode> nodes, Func<SkillNode, bool> predicate)
        {
            if (nodes.Contains(source))
            {
                return false;
            }
            nodes.Add(source);
            if (false == predicate(source))
            {
                return false;
            }
            if (source.Nodes.Contains(destination))
            {
                return true;
            }
            foreach (SkillNode node in source.Nodes)
            {
                if (CanReachFrom(node, destination, nodes, predicate))
                {
                    return true;
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