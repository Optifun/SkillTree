using Newtonsoft.Json;
using SkillTree.Data;
using SkillTree.Services;
using SkillTree.StaticData.Skills;
using SkillTree.UI.Screens;
using SkillTree.View;
using UnityEngine;

namespace SkillTree.Infrastructure
{
    public class SkillTreeMediator : MonoBehaviour
    {
        [SerializeField] private TextAsset _skillTreeAsset;
        [SerializeField] private SkillTreeView _skillTreeViewPrefab;
        [SerializeField] private Canvas _rootCanvas;

        private SkillGraphExporter _skillGraphExporter;
        private SkillTreeView _skillTreeView;
        private SkillGraphProgress _skillGraphProgress;
        private SkillGraphModel _skillGraphModel;

        private void Awake()
        {
            _skillGraphExporter = new SkillGraphExporter();
            SkillGraph skillGraph = JsonConvert.DeserializeObject<SkillGraph>(_skillTreeAsset.text);
            _skillGraphProgress = new SkillGraphProgress(skillGraph);
            _skillGraphModel = new SkillGraphModel(new GameState());

            _skillTreeView = Instantiate(_skillTreeViewPrefab, transform);
            _skillTreeView.Construct(_skillGraphModel);
        }

        private void Start()
        {
            _skillTreeView.DisplayTree(_skillGraphProgress);
        }
    }
}