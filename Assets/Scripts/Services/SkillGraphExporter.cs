using System.IO;
using Newtonsoft.Json;
using SkillTree.StaticData.Skills;
using UnityEditor;
using UnityEngine;

namespace SkillTree.Services
{
    public class SkillGraphExporter
    {
        public void SaveGraph(SkillGraph graph, string path)
        {
            TextAsset asset = new TextAsset(JsonConvert.SerializeObject(graph, Formatting.Indented));
            if (File.Exists(path))
            {
                AssetDatabase.DeleteAsset(path);
            }
            AssetDatabase.CreateAsset(asset, path);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public SkillGraph LoadGraph(string path)
        {
            string json = AssetDatabase.LoadAssetAtPath<TextAsset>(path).text;
            SkillGraph skillGraph = JsonConvert.DeserializeObject<SkillGraph>(json);
            return skillGraph;
        }
    }
}