using System;
using System.Collections.Generic;
using System.Linq;
using SkillTree.StaticData.Skills;
using UnityEngine;

namespace SkillTree.Data
{
    public class SkillGraphProgress
    {
        public readonly SkillNode GraphRoot;
        public IReadOnlyList<SkillNode> Nodes => _nodes;
        private readonly SkillNode[] _nodes;

        public SkillGraphProgress(SkillGraph skillGraph)
        {
            _nodes = skillGraph.Skills.Select(s => new SkillNode(s)).ToArray();
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

        public void EarnSkill(Guid skillId)
        {
            SkillNode node = Get(skillId);
            if (CanEarn(node))
            {
                node.SetEarned(true);
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

        private bool CanEarn(SkillNode node)
        {
            return CanReachFrom(GraphRoot, node, new List<SkillNode>(), n => n.Earned);
        }

        public bool CanForget(Guid skillId)
        {
            SkillNode node = Get(skillId);
            return CanReachFrom(node, GraphRoot, new List<SkillNode>(), n => n.Earned && n != node);
        }

        private static bool CanReachFrom(SkillNode source, SkillNode destination, List<SkillNode> nodes, Func<SkillNode, bool> predicate)
        {
            if (nodes.Contains(source))
            {
                return false;
            }
            nodes.Add(source);
            if (predicate(source))
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
    }
}