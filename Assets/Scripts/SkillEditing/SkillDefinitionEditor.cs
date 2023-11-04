using System;
using SkillTree.View;
using UnityEngine;

namespace SkillTree.SkillEditing
{
    [ExecuteInEditMode]
    public class SkillDefinitionEditor : MonoBehaviour
    {
        public event EventHandler<SkillDefinitionEditor> Deleted;

        public Guid Id => Guid.Parse(_id);
        public string Name;
        [SerializeField] private string _id;
        [field: SerializeField] public int Cost { get; set; }
        [SerializeField] private SkillView _skillView;

        public void Construct(SkillView skillView)
        {
            _skillView = skillView;
        }

        public void SetId(Guid id)
        {
            _id = id.ToString();
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

        public void OnValidate()
        {
            gameObject.name = $"Skill [{Name}]";
            _skillView?.SetName(Name);
            Cost = Math.Clamp(Cost, 0, int.MaxValue);
        }
    }
}