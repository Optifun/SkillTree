using UnityEngine;
using UnityEngine.SceneManagement;

namespace SkillTree.Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.LoadScene("SkillTree");
        }
    }
}