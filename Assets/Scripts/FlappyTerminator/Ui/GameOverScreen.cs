using UnityEngine.Events;

namespace FlappyTerminator
{
    public class GameOverScreen : Screen
    {
        public event UnityAction RestartButtonClick;

        protected override void OnButtonClick()
        {
            RestartButtonClick?.Invoke();
        }
    }
}