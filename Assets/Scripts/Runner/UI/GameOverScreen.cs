using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using UnityEditor;

namespace Runner
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Player _player;

        private CanvasGroup _gameOverGroup;

        private void OnEnable()
        {
            _player.Died += OnDied;

            _restartButton.onClick.AddListener(OnRestartButtonClick);
            _exitButton.onClick.AddListener(OnExitButtonClick);
        }

        private void OnDisable()
        {
            _player.Died -= OnDied;

            _restartButton.onClick.RemoveListener(OnRestartButtonClick);
            _exitButton.onClick.RemoveListener(OnExitButtonClick);
        }

        private void Start()
        {
            _gameOverGroup = GetComponent<CanvasGroup>();
            _gameOverGroup.alpha = 0;

            Cursor.SetCursor(PlayerSettings.defaultCursor, Vector2.zero, CursorMode.ForceSoftware);
            Cursor.visible = false;
        }

        private void OnDied()
        {
            _gameOverGroup.alpha = 1;
            Time.timeScale = 0;
            Cursor.visible = true;
        }

        private void OnRestartButtonClick()
        {
            SceneManager.LoadScene(0);
            Time.timeScale = 1;
        }

        private void OnExitButtonClick()
        {
            Application.Quit();
        }
    }
}