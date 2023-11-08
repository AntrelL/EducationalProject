using UnityEngine;
using UnityEngine.Events;

namespace FlappyTerminator
{
    [RequireComponent(typeof(PlayerMover))]
    public class Player : Entity
    {
        private int _score;
        private PlayerMover _mover;

        public event UnityAction GameOver;
        public event UnityAction<int> ScoreChanged;

        private int Score
        {
            get => _score;
            set
            {
                _score = value;
                ScoreChanged?.Invoke(_score);
            }
        }

        private void Start()
        {
            _mover = GetComponent<PlayerMover>();
            Score = 0;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && Time.timeScale != 0)
            {
                Shoot(transform.right);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<Bullet>(out _) == false)
                Die();
        }

        public void IncreaseScore()
        {
            Score++;
        }

        public void ResetValues()
        {
            Score = 0;
            _mover.ResetValues();
        }

        public void Die()
        {
            GameOver?.Invoke();
        }
    }
}