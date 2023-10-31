using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SkillTree.View
{
    public class SkillView : MonoBehaviour
    {
        [SerializeField] private Image _skillSkin;
        [SerializeField] private TMP_Text _skillNameLabel;
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _unlockedColor;

        public void SetName(string skillName)
        {
            _skillNameLabel.text = skillName;
        }

        public void SetState(bool unlocked)
        {
            _skillSkin.color = unlocked ? _unlockedColor : _normalColor;
        }
    }
}