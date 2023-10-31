using System;
using SkillTree.View;
using UnityEngine;

namespace SkillTree.SkillEditing.Skills
{
    [ExecuteInEditMode]
    public class SkillConnectionEditor : MonoBehaviour
    {
        public event EventHandler<SkillConnectionEditor> Deleted; 
        public SkillDefinitionEditor Source;
        public SkillDefinitionEditor Target;
        [SerializeField] private SkillConnectionView _view;

        [ContextMenu(nameof(Redraw))]
        public void Redraw()
        {
            _view.DrawLine(Source.transform.position, Target.transform.position);
            gameObject.name = $"Connection {Source.Name}-{Target.Name}";
        }

        public void Construct(SkillConnectionView connectionView)
        {
            _view = connectionView;
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
    }
}