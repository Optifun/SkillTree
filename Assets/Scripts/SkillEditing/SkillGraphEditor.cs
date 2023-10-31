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
        public const string FileExtension = ".asset";
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
            SkillView skillView = Instantiate(SkillPrefab, _nodesContainer);
            SkillDefinitionEditor editor = skillView.AddComponent<SkillDefinitionEditor>();
            editor.Construct(skillView);
            editor.Deleted += OnSkillNodeDeleted;
            Skills.Add(editor);
        }

        [ContextMenu(nameof(AddConnection))]
        public void AddConnection()
        {
            var connectionView = Instantiate(ConnectionPrefab, _connectionContainer);
            SkillConnectionEditor editor = connectionView.AddComponent<SkillConnectionEditor>();
            editor.Deleted += OnConnectionDeleted;
            editor.Construct(connectionView);
            Connections.Add(editor);
        }

        [ContextMenu(nameof(Export))]
        public void Export()
        {
            SkillGraph graph = this.ToDTO();
            string path = EditorUtility.SaveFilePanel("Save file", PathToData, graph.Name, FileExtension);

            _skillGraphExporter.SaveGraph(graph, path);
        }

        [ContextMenu(nameof(Load))]
        public void Load()
        {
            string path = EditorUtility.OpenFilePanel("Choose file", "skills", FileExtension);
            SkillGraph skillGraph = _skillGraphExporter.LoadGraph(path);
            InitializeEditor(skillGraph);
        }

        private void InitializeEditor(SkillGraph skillGraph)
        {
            TreeName = skillGraph.Name;
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