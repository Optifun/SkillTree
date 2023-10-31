using System;
using Unity.VisualScripting;

namespace SkillTree.UI.Core
{
    public interface IScreenPresenter : IInitializable, IDisposable
    {
    }

    public abstract class BaseScreen<T> : BaseScreen where T : IScreenPresenter
    {
        protected T Presenter { get; private set; }

        public void Construct(T presenter)
        {
            Presenter = presenter;
        }
    }
}