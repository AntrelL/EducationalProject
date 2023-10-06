using UnityEngine.Events;
using UnityEngine;
using System;

namespace Runner
{
    public class Player : MonoBehaviour
    {
        private readonly string Damage = "Damage";
        private readonly string Healing = "Healing";

        [SerializeField] private int _maxHealth;

        private int _health;

        public event UnityAction<int> HealthChanged;
        public event UnityAction Died;

        private int Health
        {
            get => _health;
            set
            {
                _health = Mathf.Clamp(value, 0, _maxHealth);
                HealthChanged?.Invoke(_health);
            }
        }

        private void Start()
        {
            Health = _maxHealth;
        }

        public void ApplyDamage(int damage)
        {
            CheckCorrectHealthChangeValue(damage, Damage);
            Health -= damage;

            if (Health == 0)
                Die();
        }

        public void ApplyHeal(int healingValue)
        {
            CheckCorrectHealthChangeValue(healingValue, Healing);
            Health += healingValue;
        }

        private void Die()
        {
            Died?.Invoke();
        }

        private void CheckCorrectHealthChangeValue(float value, string nameOfModificationType)
        {
            if (value < 0)
                throw new ArgumentException($"{nameOfModificationType} value can't be less than zero.");
        }
    }
}