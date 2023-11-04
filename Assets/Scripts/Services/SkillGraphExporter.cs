using Newtonsoft.Json;
using SkillTree.StaticData.Skills;
using UnityEditor;
using UnityEngine;

namespace SkillTree.Services
{
    public class SkillGraphExporter
    {
        public void SaveGraph(SkillGraph graph, string projectPath)
        {
            TextAsset asset = new TextAsset(JsonConvert.SerializeObject(graph, Formatting.Indented));
            AssetDatabase.DeleteAsset(projectPath);
            AssetDatabase.CreateAsset(asset, projectPath);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public SkillGraph LoadGraph(string projectPath)
        {
            string json = AssetDatabase.LoadAssetAtPath<TextAsset>(projectPath).text;
            SkillGraph skillGraph = JsonConvert.DeserializeObject<SkillGraph>(json);
            return skillGraph;
        }
    }
}