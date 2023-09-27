using UnityEngine;
using System;

namespace UITask2
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private HealthVisualizer _healthVisualizer;

        private float _maxHealth = 100f;
        private float _health;

        public float Health
        {
            get => _health;
            private set
            {
                _health = Mathf.Clamp(value, 0, _maxHealth);
                _healthVisualizer?.UpdateHealthValue(_health);
            }
        }

        private void Start()
        {
            Health = _maxHealth;
        }

        public void TakeDamage(float value)
        {
            CheckCorrectChangeValue(value, "Damage");
            Health -= value;
        }

        public void Heal(float value)
        {
            CheckCorrectChangeValue(value, "Healing");
            Health += value;
        }

        private void CheckCorrectChangeValue(float value, string nameOfModificationType)
        {
            if (value < 0)
                throw new ArgumentException($"{nameOfModificationType} value can't be less than zero.");
        }
    }
}