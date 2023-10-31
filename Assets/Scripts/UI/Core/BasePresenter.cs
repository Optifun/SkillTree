namespace SkillTree.UI.Core
{
    public abstract class BasePresenter : IScreenPresenter
    {

        public void Initialize()
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