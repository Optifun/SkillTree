using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using SkillTree.Data;
using SkillTree.Utils;
using UnityEngine;

namespace SkillTree.View
{
    public class SkillTreeView : MonoBehaviour
    {
        public readonly ReactiveCommand<Guid> SkillClicked = new();

        [SerializeField] private SkillView _viewPrefab;
        [SerializeField] private SkillConnectionView _connectionPrefab;

        private readonly List<SkillView> _skillViews = new();
        private readonly List<SkillConnectionView> _skillConnectionViews = new();
        private Guid? _lastSelectedSkill;
        private CompositeDisposable _subscriptions = new();

        public void DisplayTree(IEnumerable<ISkill> nodes)
        {
            List<(Guid, Guid)> connections = new();
            foreach (ISkill skill in nodes)
            {
                Vector3 position = skill.Data.Position.ToVector3().ConvertToUnityVector();
                SkillView skillView = Instantiate(_viewPrefab, position, Quaternion.identity, transform);
                skillView.SetId(skill.Data.Id);
                skillView.SetName(skill.Data.Name);
                skillView.SetEarned(skill.IsEarned.CurrentValue);
                skillView.SetSelected(false);
                skillView.Clicked += OnSkillClicked;
                CreateConnections(skill, connections);
                _skillViews.Add(skillView);
                _subscriptions.Add(skill.IsEarned.Subscribe(skill, OnSkillStateChanged));
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
            SkillClicked.Execute(id);
        }

        private void OnSkillStateChanged(bool isEarned, ISkill skill)
        {
            SkillView view = GetSkillView(skill.Id);
            view.SetEarned(isEarned);
        }

        public void SelectSkill(Guid? skillId)
        {
            if (_lastSelectedSkill != null)
            {
                GetSkillView(_lastSelectedSkill.Value).SetSelected(false);
            }

            if (skillId is Guid newValue && _lastSelectedSkill != newValue)
            {
                _lastSelectedSkill = newValue;
                GetSkillView(newValue).SetSelected(true);
            }
        }

        private SkillView GetSkillView(Guid skillId)
        {
            return _skillViews.First(view1 => view1.Id == skillId);
        }
    }
}