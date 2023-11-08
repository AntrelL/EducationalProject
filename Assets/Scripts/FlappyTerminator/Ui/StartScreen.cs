using UnityEngine.Events;

namespace FlappyTerminator
{
    public class StartScreen : Screen
    {
        public event UnityAction PlayButtonClick;

        protected override void OnButtonClick()
        {
            PlayButtonClick?.Invoke();
        }
    }
}