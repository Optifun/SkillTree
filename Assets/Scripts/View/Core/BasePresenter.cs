using System;
using R3;

namespace SkillTree.UI.Core
{
    public abstract class BasePresenter : IScreenPresenter
    {
        protected DisposableBag Subscriptions { get; private set; } = new();

        public virtual void Initialize()
        {
        }

        public void Dispose()
        {
            OnDispose();
            Subscriptions.Dispose();
        }

        protected virtual void OnDispose()
        {
        }

        protected void Bind<T1>(Observable<T1> source, Action<T1> onNext)
        {
            Subscriptions.Add(source.Subscribe(onNext));
        }

        protected void Bind<T1>(Observable<T1> source, Action<T1> onNext, Action<Result> onCompleted)
        {
            Subscriptions.Add(source.Subscribe(onNext, onCompleted));
        }

        protected void Bind<T1>(Observable<T1> source, Action<T1> onNext, Action<Exception> onError, Action<Result> onCompleted)
        {
            Subscriptions.Add(source.Subscribe(onNext, onError, onCompleted));
        }

        protected void Bind<T1>(Observable<T1> source, Action onNext)
        {
            Bind(source, _ => onNext());
        }
        
        protected void Bind<T1>(Observable<T1> source, Action onNext, Action<Result> onCompleted)
        {
            Bind(source, _ => onNext(), onCompleted);
        }

        protected void Bind<T1>(Observable<T1> source, Action onNext, Action<Exception> onError, Action<Result> onCompleted)
        {
            Bind(source, _ => onNext(), onError, onCompleted);
        }
    }
}