using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SkillTree.View
{
    public class SkillView : MonoBehaviour, IPointerClickHandler
    {
        public event EventHandler<Guid> Clicked;

        public Guid Id { get; private set; }
        [SerializeField] private Image _skillSkin;
        [SerializeField] private TMP_Text _skillNameLabel;
        [SerializeField] private Color _normalColor;
        [SerializeField] private Color _selectedColor;
        [SerializeField] private Color _earnedColor;
        [SerializeField] private Color _earnedSelectedColor;
        private bool _earned;
        private bool _selected;

        public void SetId(Guid skillId)
        {
            Id = skillId;
        }

        public void SetName(string skillName)
        {
            _skillNameLabel.text = skillName;
        }

        public void SetEarned(bool value)
        {
            _earned = value;
            RedrawSkin();
        }

        public void SetSelected(bool selected)
        {
            _selected = selected;
            RedrawSkin();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke(this, Id);
        }

        private void RedrawSkin()
        {
            Color color = _normalColor;
            if (_selected && _earned)
            {
                color = _earnedSelectedColor;
            }
            else if (_selected)
            {
                color = _selectedColor;
            }
            else if (_earned)
            {
                color = _earnedColor;
            }
            _skillSkin.color = color;
        }
    }
}