using System.Collections;
using UnityEngine;

namespace Platformer
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private float _rateOfChangeValue;
        [SerializeField] private Transform _fillingLine;
        [SerializeField] private Entity _entity;

        private Coroutine _healthChanger;

        private void OnEnable()
        {
            _entity.HealthChanged += OnHealthChanged;
        }

        private void OnDisable()
        {
            _entity.HealthChanged -= OnHealthChanged;
        }

        private void OnHealthChanged(int health)
        {
            float limitedValue = Mathf.Clamp01(health / (float)_entity.MaxHealth);

            UpdateHealthValue(limitedValue);
        }

        private void UpdateHealthValue(float value)
        {
            if (_healthChanger != null)
                StopCoroutine(_healthChanger);

            _healthChanger = StartCoroutine(ChangeHealthValue(value, _rateOfChangeValue));
        }

        private IEnumerator ChangeHealthValue(float targetValue, float rateOfChange)
        {
            Vector3 fillingLineScale = _fillingLine.localScale;

            while (fillingLineScale.x != targetValue)
            {
                fillingLineScale.x = Mathf.MoveTowards(fillingLineScale.x, targetValue, rateOfChange * Time.deltaTime);
                _fillingLine.localScale = fillingLineScale;

                yield return null;
            }
        }
    }
}