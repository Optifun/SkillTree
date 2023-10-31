using UnityEditor;
using UnityEngine;

namespace SkillTree.UI.Core
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class BaseScreen : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup _canvasGroup;
        
        public void ShowScreen()
        {
            SetActive(true);
            OnScreenShown();
        }

        public void HideScreen()
        {
            SetActive(false);
            OnScreenHidden();
        }

        public void CloseScreen()
        {
            SetActive(false);
            OnScreenHidden();
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