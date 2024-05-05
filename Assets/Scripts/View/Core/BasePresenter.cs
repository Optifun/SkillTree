namespace SkillTree.UI.Core
{
    public abstract class BasePresenter : IScreenPresenter
    {

        public virtual void Initialize()
        {
        }

        public void Dispose()
        {
            OnDispose();
        }

        protected virtual void OnDispose()
        {
        }
    }
}