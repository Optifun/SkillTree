using System;
using System.Collections.Generic;
using System.Linq;
using SkillTree.Data;
using SkillTree.Data.Events;
using SkillTree.Utils;
using UnityEngine;

namespace SkillTree.View
{
    public class SkillTreeView : MonoBehaviour
    {
        public event EventHandler<Guid> SkillClicked;
        [SerializeField] private SkillView _viewPrefab;
        [SerializeField] private SkillConnectionView _connectionPrefab;

        private readonly List<SkillView> _skillViews = new();
        private readonly List<SkillConnectionView> _skillConnectionViews = new();
        private Guid _lastSelectedSkill;

        public void DisplayTree(IEnumerable<ISkill> nodes)
        {
            List<(Guid, Guid)> connections = new();
            foreach (ISkill skill in nodes)
            {
                Vector3 position = skill.Data.Position.ToVector3().ConvertToUnityVector();
                SkillView skillView = Instantiate(_viewPrefab, position, Quaternion.identity, transform);
                skillView.SetId(skill.Data.Id);
                skillView.SetName(skill.Data.Name);
                skillView.SetEarned(skill.Earned);
                skillView.SetSelected(false);
                skillView.Clicked += OnSkillClicked;
                CreateConnections(skill, connections);
                _skillViews.Add(skillView);
                skill.StateChanged += OnSkillStateChanged;
            }
        }

        private void CreateConnections(ISkill skill, List<(Guid, Guid)> connections)
        {
            foreach (ISkill peer in skill.Nodes)
            {
                if (connections.Exists(pair =>
                    {
                        (Guid n1, Guid n2) = pair;
                        return (skill.Id == n1 && peer.Id == n2) ||
                                (skill.Id == n2 && peer.Id == n1);
                    }))
                {
                    continue;
                }
                SkillConnectionView connectionView = Instantiate(_connectionPrefab, transform);
                Vector3 from = skill.Data.Position.ToVector3().ConvertToUnityVector();
                Vector3 to = peer.Data.Position.ToVector3().ConvertToUnityVector();
                connectionView.DrawLine(from, to);
                connections.Add((skill.Id, peer.Id));
                _skillConnectionViews.Add(connectionView);
            }
        }

        private void OnSkillClicked(object sender, Guid id)
        {
            SkillClicked?.Invoke(this, id);
        }

        private void OnSkillStateChanged(object sender, SkillNodeStateChangedArgs e)
        {
            ISkill node = e.Skill;
            Guid skillId = node.Id;
            SkillView view = GetSkillView(skillId);
            view.SetEarned(e.Earned);
        }

        public void SelectSkill(Guid skillId)
        {
            if (_lastSelectedSkill != default)
            {
                GetSkillView(_lastSelectedSkill).SetSelected(false);
            }
            if (_lastSelectedSkill != skillId)
            {
                _lastSelectedSkill = skillId;
                GetSkillView(_lastSelectedSkill).SetSelected(true);
            }
        }

        private SkillView GetSkillView(Guid skillId)
        {
            return _skillViews.First(view1 => view1.Id == skillId);
        }
    }
}