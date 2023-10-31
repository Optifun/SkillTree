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
        [SerializeField] private Color _unlockedColor;

        public void SetId(Guid skillId)
        {
            Id = skillId;
        }

        public void SetName(string skillName)
        {
            _skillNameLabel.text = skillName;
        }

        public void SetState(bool unlocked)
        {
            _skillSkin.color = unlocked ? _unlockedColor : _normalColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke(this, Id);
        }
    }
}