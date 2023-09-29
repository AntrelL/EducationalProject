using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UITask2
{
    [RequireComponent(typeof(Slider))]
    public class HealthBar : HealthVisualizer
    {
        [SerializeField] private float _rateOfChangeValue;

        private Slider _slider;
        private Coroutine _healthChanger;

        private void Start()
        {
            _slider = GetComponent<Slider>();
        }

        public override void UpdateValue(float value)
        {
            if (_healthChanger != null)
                StopCoroutine(_healthChanger);

            _healthChanger = StartCoroutine(ChangeValue(value, _rateOfChangeValue));
        }

        private IEnumerator ChangeValue(float targetValue, float rateOfChange)
        {
            while (_slider.value != targetValue)
            {
                _slider.value = Mathf.MoveTowards(_slider.value, targetValue, rateOfChange * Time.deltaTime);

                yield return null;
            }
        }
    }
}