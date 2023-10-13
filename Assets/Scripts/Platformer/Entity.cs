using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Platformer
{
    [RequireComponent(typeof(Animator))]
    public abstract class Entity : Movement
    {
        private readonly string DamageText = "Damage";
        private readonly string HealingText = "Healing";

        [SerializeField] private int _maxHealth;
        [SerializeField] private int _damage;
        [SerializeField] private float _recliningForce;

        public int _health;  // изменить на privite

        protected int Health
        {
            get => _health;
            set
            {
                _health = Mathf.Clamp(value, 0, _maxHealth);
            }
        }

        protected int Damage => _damage;

        protected float RecliningForce => _recliningForce;

        protected virtual void Start()
        {
            Health = _maxHealth;
        }

        protected override void Update() => base.Update();

        protected override void FixedUpdate() => base.FixedUpdate();

        public void ApplyDamage(int damage) => ApplyDamage(damage, Vector2.zero);

        public void ApplyDamage(int damage, Vector2 knockbackForce)
        {
            CheckCorrectHealthChangeValue(damage, DamageText);
            Health -= damage;

            if (Health == 0)
                Die();

            Throw(knockbackForce);

            if (Animated)
                EntityActionAnimator.TakeDamage();
        }

        public void ApplyHeal(int healingValue)
        {
            CheckCorrectHealthChangeValue(healingValue, HealingText);
            Health += healingValue;
        }

        protected Vector2 CalculateKnockback(float recliningForce, Vector3 targetPosition)
        {
            float verticalThrowForce = 0.3f;

            Vector2 verticalForce = Vector2.up * verticalThrowForce;
            Vector2 horizontalDirection = (targetPosition - transform.position) * Vector2.right;

            Vector2 resultDirection = (horizontalDirection.normalized + verticalForce).normalized;

            return resultDirection * recliningForce;
        }

        private void Die()
        {
            Destroy(gameObject);
        }

        private void CheckCorrectHealthChangeValue(float value, string nameOfModificationType)
        {
            if (value < 0)
                throw new ArgumentException($"{nameOfModificationType} value can't be less than zero.");
        }
    }
}