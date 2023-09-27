using UnityEngine;
using UnityEngine.UI;

namespace UITask2
{
    [RequireComponent(typeof(Slider))]
    public class HealthBar : HealthVisualizer
    {
        [SerializeField] private float _rateOfChangeValue;

        private Slider _slider;
        private float _targetValue;

        private void Start()
        {
            _slider = GetComponent<Slider>();
        }

        private void Update()
        {
            _slider.value = Mathf.MoveTowards(_slider.value, _targetValue, _rateOfChangeValue * Time.deltaTime);
        }

        public override void UpdateHealthValue(float value)
        {
            _targetValue = value;
        }
    }
}