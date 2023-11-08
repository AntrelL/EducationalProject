using UnityEngine;
using TMPro;

namespace FlappyTerminator
{
    [RequireComponent(typeof(TMP_Text))]
    public class ScoreViewer : MonoBehaviour
    {
        [SerializeField] private Player _player;

        private TMP_Text _TMPText;

        private void Awake()
        {
            _TMPText = GetComponent<TMP_Text>();
        }

        private void OnEnable()
        {
            _player.ScoreChanged += OnScoreChanged;
        }

        private void OnDisable()
        {
            _player.ScoreChanged -= OnScoreChanged;
        }

        private void OnScoreChanged(int value)
        {
            _TMPText.text = value.ToString();
        }
    }
}