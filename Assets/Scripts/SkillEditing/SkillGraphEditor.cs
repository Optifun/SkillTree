using System;
using System.Collections.Generic;
using System.IO;
using SkillTree.Services;
using SkillTree.StaticData.Skills;
using SkillTree.Utils;
using SkillTree.View;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace SkillTree.SkillEditing
{
    [ExecuteInEditMode]
    public class SkillGraphEditor : MonoBehaviour
    {
        public const string FileExtension = "asset";
        public const string PathToData = "Assets/Configs/SkillTree/";

        public string TreeName;
        public SkillDefinitionEditor BaseSkill;
        public List<SkillDefinitionEditor> Skills;
        public List<SkillConnectionEditor> Connections;

        [Header("Miscellaneous")]
        public SkillView SkillPrefab;
        public SkillConnectionView ConnectionPrefab;
        [SerializeField] private Transform _nodesContainer;
        [SerializeField] private Transform _connectionContainer;
        private readonly SkillGraphExporter _skillGraphExporter = new();


        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        [ContextMenu(nameof(AddSkill))]
        public void AddSkill()
        {
            ConstructSkillEditor();
        }

        [ContextMenu(nameof(AddConnection))]
        public void AddConnection()
        {
            ConstructConnectionEditor();
        }

        [ContextMenu(nameof(Export))]
        public void Export()
        {
            SkillGraph graph = this.ToDTO();
            string path = EditorUtility.SaveFilePanel("Save file", PathToData, graph.Name, FileExtension);

            string relativePath = GetProjectRelativePath(path);
            _skillGraphExporter.SaveGraph(graph, relativePath);
        }

        [ContextMenu(nameof(Import))]
        public void Import()
        {
            string path = EditorUtility.OpenFilePanel("Choose file", "skills", FileExtension);
            SkillGraph skillGraph = _skillGraphExporter.LoadGraph(GetProjectRelativePath(path));
            InitializeEditor(skillGraph);
        }

        [ContextMenu(nameof(ClearGraph))]
        public void ClearGraph()
        {
            foreach (SkillDefinitionEditor editor in Skills)
            {
                editor.Deleted -= OnSkillNodeDeleted;
                DestroyImmediate(editor.gameObject);
            }
            foreach (SkillConnectionEditor editor in Connections)
            {
                editor.Deleted -= OnConnectionDeleted;
                DestroyImmediate(editor.gameObject);
            }
            BaseSkill = null;
            Skills.Clear();
            Connections.Clear();
        }

        private void InitializeEditor(SkillGraph skillGraph)
        {
            ClearGraph();
            foreach (SkillDefinition skill in skillGraph.Skills)
            {
                SkillDefinitionEditor skillEditor = ConstructSkillEditor();
                skillEditor.SetId(skill.Id);
                skillEditor.Name = skill.Name;
                skillEditor.Cost = skill.EarnCost;
                skillEditor.transform.localPosition = skill.Position.ToVector3().ConvertToUnityVector();
                skillEditor.OnValidate();
            }
            foreach (SkillConnection connection in skillGraph.Connections)
            {
                var connectionEditor = ConstructConnectionEditor();
                connectionEditor.Source = GetSkillEditor(connection.Source);
                connectionEditor.Target = GetSkillEditor(connection.Target);
                connectionEditor.Redraw();
            }
            TreeName = skillGraph.Name;
            BaseSkill = GetSkillEditor(skillGraph.BaseSkill);
        }

        private static string GetProjectRelativePath(string path)
        {
            return path.Substring(path.IndexOf("Assets", StringComparison.Ordinal));
        }

        private void Subscribe()
        {
            foreach (SkillDefinitionEditor skill in Skills)
            {
                skill.Deleted += OnSkillNodeDeleted;
            }
            foreach (SkillConnectionEditor connection in Connections)
            {
                connection.Deleted += OnConnectionDeleted;
            }
        }

        private void Unsubscribe()
        {
            foreach (SkillDefinitionEditor skill in Skills)
            {
                skill.Deleted -= OnSkillNodeDeleted;
            }
            foreach (SkillConnectionEditor connection in Connections)
            {
                connection.Deleted -= OnConnectionDeleted;
            }
        }

        private SkillDefinitionEditor GetSkillEditor(Guid skillId)
        {
            return Skills.Find(skill => skill.Id == skillId);
        }

        private SkillDefinitionEditor ConstructSkillEditor()
        {
            SkillView skillView = Instantiate(SkillPrefab, _nodesContainer);
            SkillDefinitionEditor editor = skillView.AddComponent<SkillDefinitionEditor>();
            editor.Construct(skillView);
            editor.Deleted += OnSkillNodeDeleted;
            Skills.Add(editor);
            return editor;
        }

        private SkillConnectionEditor ConstructConnectionEditor()
        {
            SkillConnectionView connectionView = Instantiate(ConnectionPrefab, _connectionContainer);
            SkillConnectionEditor editor = connectionView.AddComponent<SkillConnectionEditor>();
            editor.Deleted += OnConnectionDeleted;
            editor.Construct(connectionView);
            Connections.Add(editor);
            return editor;
        }

        private void OnConnectionDeleted(object sender, SkillConnectionEditor editor)
        {
            editor.Deleted -= OnConnectionDeleted;
            Connections.Remove(editor);
            DestroyImmediate(editor.gameObject);
        }

        private void OnSkillNodeDeleted(object sender, SkillDefinitionEditor editor)
        {
            editor.Deleted -= OnSkillNodeDeleted;
            if (BaseSkill == editor)
            {
                BaseSkill = null;
            }
            Skills.Remove(editor);
            DestroyImmediate(editor.gameObject);
            EditorUtility.SetDirty(this);
        }
    }
}