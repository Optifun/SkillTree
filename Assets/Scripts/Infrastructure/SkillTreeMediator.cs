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
        [SerializeField] private SkillGraphScreen _skillGraphScreenPrefab;

        private SkillTreeView _skillTreeView;
        private SkillGraphScreen _skillGraphScreen;

        private SkillGraphExporter _skillGraphExporter;
        private SkillGraphProgress _skillGraphProgress;
        private SkillGraphModel _skillGraphModel;
        private GameState _gameState;

        private void Awake()
        {
            _skillGraphExporter = new SkillGraphExporter();
            SkillGraph skillGraph = JsonConvert.DeserializeObject<SkillGraph>(_skillTreeAsset.text);
            _skillGraphProgress = new SkillGraphProgress(skillGraph);
            _gameState = new GameState();
            _skillGraphModel = new SkillGraphModel(_gameState, _skillGraphProgress);

            _skillTreeView = Instantiate(_skillTreeViewPrefab, transform);
            _skillTreeView.Construct(_skillGraphModel);

            _skillGraphScreen = Instantiate(_skillGraphScreenPrefab, _rootCanvas.transform);
            SkillGraphPresenter presenter = new SkillGraphPresenter(_skillGraphModel);
            presenter.Initialize();
            _skillGraphScreen.Construct(presenter);
        }

        private void Start()
        {
            _skillTreeView.DisplayTree(_skillGraphProgress);
            _skillGraphScreen.ShowScreen();
        }
    }
}