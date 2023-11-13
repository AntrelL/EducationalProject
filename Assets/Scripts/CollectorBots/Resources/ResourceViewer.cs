using UnityEngine;
using TMPro;

namespace CollectorBots
{
    [RequireComponent(typeof(TMP_Text))]
    public class ResourceViewer : MonoBehaviour
    {
        [SerializeField] private BotBase _botBase;

        private TMP_Text _tmpText;

        private void OnEnable()
        {
            _botBase.NumberOfResourcesChanged += OnNumberOfResourcesChanged;
        }

        private void OnDisable()
        {
            _botBase.NumberOfResourcesChanged -= OnNumberOfResourcesChanged;
        }

        private void Awake()
        {
            _tmpText = GetComponent<TMP_Text>();
        }

        private void OnNumberOfResourcesChanged(int value)
        {
            _tmpText.text = "Ресурсы: " + value;
        }
    }
}