using UnityEngine;
using UnityEngine.UI;

namespace FlappyTerminator
{
    public abstract class Screen : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup CanvasGroup;
        [SerializeField] protected Button Button;

        private void OnEnable()
        {
            Button.onClick.AddListener(OnButtonClick);
        }

        private void OnDisable()
        {
            Button.onClick.RemoveListener(OnButtonClick);
        }

        protected abstract void OnButtonClick();

        public void Open()
        {
            CanvasGroup.alpha = 1;
            Button.interactable = true;
        }

        public void Close()
        {
            CanvasGroup.alpha = 0;
            Button.interactable = false;
        }
    }
}