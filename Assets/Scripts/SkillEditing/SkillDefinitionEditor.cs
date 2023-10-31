using System;
using SkillTree.View;
using UnityEngine;

namespace SkillTree.SkillEditing.Skills
{
    [ExecuteInEditMode]
    public class SkillDefinitionEditor : MonoBehaviour
    {
        public event EventHandler<SkillDefinitionEditor> Deleted;
        public string Name;
        public Guid Id => Guid.Parse(_id);
        [SerializeField] private string _id;
        private SkillView _skillView;

        public void Construct(SkillView skillView)
        {
            _skillView = skillView;
        }

        [ContextMenu(nameof(RegenerateId))]
        private void RegenerateId()
        {
            _id = Guid.NewGuid().ToString();
        }

        [ContextMenu(nameof(Delete))]
        public void Delete()
        {
            Deleted?.Invoke(this, this);
        }

        private void OnDestroy()
        {
            Deleted?.Invoke(this, this);
        }

        private void Reset()
        {
            RegenerateId();
        }

        private void OnValidate()
        {
            gameObject.name = $"Skill [{Name}]";
            _skillView?.SetName(Name);
        }
    }
}