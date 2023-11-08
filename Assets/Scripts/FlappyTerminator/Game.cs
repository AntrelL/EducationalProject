using UnityEngine;

namespace FlappyTerminator
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private Player _player;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private BulletStorage _bulletStorage;
        [SerializeField] private StartScreen _startScreen;
        [SerializeField] private GameOverScreen _gameOverScreen;

        private void OnEnable()
        {
            _player.GameOver += OnGameOver;
            _startScreen.PlayButtonClick += OnPlayButtonClick;
            _gameOverScreen.RestartButtonClick += OnRestartButtonClick;
        }

        private void OnDisable()
        {
            _player.GameOver -= OnGameOver;
            _startScreen.PlayButtonClick -= OnPlayButtonClick;
            _gameOverScreen.RestartButtonClick -= OnRestartButtonClick;
        }

        private void Start()
        {
            Time.timeScale = 0;
            _startScreen.Open();
        }

        private void OnPlayButtonClick()
        {
            _startScreen.Close();
            StartGame();
        }

        private void OnRestartButtonClick()
        {
            _gameOverScreen.Close();
            _enemySpawner.ResetEnemies();
            _bulletStorage.ResetBullets();
            StartGame();
        }

        private void StartGame()
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            _player.ResetValues();
        }

        private void OnGameOver()
        {
            Time.timeScale = 0;
            Cursor.visible = true;
            _gameOverScreen.Open();
        }
    }
}