using System;
using R3;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace SkillTree.UI.Core
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseScreen : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        protected DisposableBag Subscriptions { get; private set; }

        public void ShowScreen()
        {
            SetActive(true);
            OnScreenShown();
        }

        public void HideScreen()
        {
            OnScreenHidden();
            SetActive(false);
        }

        public void CloseScreen()
        {
            Subscriptions.Dispose();
            OnScreenHidden();
            SetActive(false);
        }

        private void SetActive(bool state)
        {
            if (state)
            {
                _canvasGroup.alpha = 1;
                _canvasGroup.interactable = true;
                _canvasGroup.blocksRaycasts = true;
            }
            else
            {
                _canvasGroup.alpha = 0;
                _canvasGroup.interactable = false;
                _canvasGroup.blocksRaycasts = false;
            }
        }

        protected virtual void OnScreenShown() {}
        protected virtual void OnScreenHidden() {}
        
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
        
        protected void Bind(Button.ButtonClickedEvent source, Action onNext)
        {
            UnityAction unityAction = () => onNext();
            var observable = Observable.FromEvent(_ =>
            {
                source.AddListener(unityAction);
            }, _ => source.RemoveListener(unityAction));
            Bind(observable, onNext);
        }

        protected virtual void Reset()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void OnValidate()
        {
            if (_canvasGroup == null)
            {
                Debug.LogError("Canvas group was null. Trying to pick a reference again.", gameObject);
                _canvasGroup = GetComponent<CanvasGroup>();
#if UNITY_EDITOR
                EditorUtility.SetDirty(gameObject);
#endif
            }
        }
    }
}