using System;
using System.Collections.Generic;
using System.Linq;
using SkillTree.Data;
using SkillTree.Data.Events;
using SkillTree.UI.Screens;
using SkillTree.Utils;
using UnityEngine;

namespace SkillTree.View
{
    public class SkillTreeView : MonoBehaviour
    {
        [SerializeField] private SkillView _viewPrefab;
        [SerializeField] private SkillConnectionView _connectionPrefab;

        private readonly List<SkillView> _skillViews = new();
        private readonly List<SkillConnectionView> _skillConnectionViews = new();
        private SkillGraphModel _model;

        public void Construct(SkillGraphModel model)
        {
            _model = model;
        }

        public void DisplayTree(SkillGraphProgress graph)
        {
            List<(Guid, Guid)> connections = new();
            foreach (SkillNode skill in graph.Nodes)
            {
                Vector3 position = skill.Data.Position.ToVector3().ConvertToUnityVector();
                SkillView skillView = Instantiate(_viewPrefab, position, Quaternion.identity, transform);
                skillView.SetId(skill.Data.Id);
                skillView.SetName(skill.Data.Name);
                skillView.SetState(skill.Earned);
                skillView.Clicked += OnSkillClicked;
                CreateConnections(skill, connections);
                _skillViews.Add(skillView);
                skill.StateChanged += OnSkillStateChanged;
            }
        }

        private void CreateConnections(SkillNode skill, List<(Guid, Guid)> connections)
        {
            foreach (SkillNode peer in skill.Nodes)
            {
                if (connections.Exists(pair =>
                    {
                        (Guid n1, Guid n2) = pair;
                        return (skill.Id == n1 && peer.Id == n2)
                                || (skill.Id == n2 && peer.Id == n1);
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
            _model.SetSelection(id);
        }

        private void OnSkillStateChanged(object sender, SkillNodeStateChangedArgs e)
        {
            SkillNode node = e.SkillNode;
            SkillView view = _skillViews.First(view => view.Id == node.Id);
            view.SetState(e.Earned);
        }
    }
}