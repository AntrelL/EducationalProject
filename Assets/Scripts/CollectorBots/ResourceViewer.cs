using UnityEngine;
using TMPro;

namespace CollectorBots
{
    [RequireComponent(typeof(TMP_Text))]
    public class ResourceViewer : MonoBehaviour
    {
        [SerializeField] private CollectorBotBase _collectorBotBase;

        private TMP_Text _TMPText;

        private void OnEnable()
        {
            _collectorBotBase.NumberOfResourcesChanged += OnNumberOfResourcesChanged;
        }

        private void OnDisable()
        {
            _collectorBotBase.NumberOfResourcesChanged -= OnNumberOfResourcesChanged;
        }

        private void Awake()
        {
            _TMPText = GetComponent<TMP_Text>();
        }

        private void OnNumberOfResourcesChanged(int value)
        {
            _TMPText.text = "�������: " + value;
        }
    }
}