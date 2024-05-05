using Newtonsoft.Json;
using SkillTree.Data;
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

        private SkillGraphProgress _skillGraphProgress;
        private GameState _gameState;

        private void Awake()
        {
            InitModel();

            SkillGraphPresenter presenter = new SkillGraphPresenter(_gameState, _skillGraphProgress);

            CreateViews(presenter);
        }

        private void Start()
        {
            _skillGraphScreen.ShowScreen();
        }

        private void InitModel()
        {
            SkillGraph skillGraph = JsonConvert.DeserializeObject<SkillGraph>(_skillTreeAsset.text);
            _skillGraphProgress = new SkillGraphProgress(skillGraph);
            _gameState = new GameState();
        }

        private void CreateViews(SkillGraphPresenter presenter)
        {
            _skillTreeView = Instantiate(_skillTreeViewPrefab, transform);
            _skillGraphScreen = Instantiate(_skillGraphScreenPrefab, _rootCanvas.transform);
            _skillGraphScreen.Construct(presenter);
            presenter.Initialize();
            _skillGraphScreen.Initialize(_skillTreeView);
        }
    }
}