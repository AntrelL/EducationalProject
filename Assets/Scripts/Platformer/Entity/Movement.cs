using System.Collections;
using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] protected EntityActionAnimator EntityActionAnimator;

        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _jumpForce;

        protected BoxCollider2D BoxCollider2D;

        private Rigidbody2D _rigidbody2D;

        private bool _animated;
        private bool _grounded;
        private float _knockback = 0;

        protected bool Animated => _animated;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            BoxCollider2D = GetComponent<BoxCollider2D>();
            _animated = EntityActionAnimator != null;
        }

        protected virtual void Update()
        {
            if (_animated)
            {
                EntityActionAnimator.VerticalVelocity = _rigidbody2D.velocity.y;
                EntityActionAnimator.Grounded = _grounded;
            }
        }

        protected virtual void FixedUpdate()
        {
            float groundCheckAreaHight = 0.5f;

            (Vector2 groundCheckAreaPointA, Vector2 groundCheckAreaPointB) = 
                CalculateGroundCheckAreaPoints(transform.position, BoxCollider2D.size, groundCheckAreaHight);

            _grounded = Physics2D.OverlapAreaAll(groundCheckAreaPointA, groundCheckAreaPointB).Length > 1;
        }

        protected void Move(float direction, float speedNormalizationFactor = 1)
        {
            float minDirectionValue = -1;
            float maxDirectionValue = 1;

            if (direction > maxDirectionValue || direction < minDirectionValue)
            {
                throw new System.ArgumentException(
                    $"Direction can have values from {minDirectionValue} to {maxDirectionValue}.");
            }

            float horizontalVelocity = direction * _movementSpeed * speedNormalizationFactor;

            if (_knockback != 0)
                horizontalVelocity = _knockback;

            _rigidbody2D.velocity = new Vector2(horizontalVelocity, _rigidbody2D.velocity.y);

            if (_animated)
                EntityActionAnimator.HorizontalVelocity = horizontalVelocity;
        }

        protected void Jump()
        {
            if (_grounded)
                _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
        }

        protected void Throw(Vector2 force)
        {
            _rigidbody2D.AddForce(force, ForceMode2D.Impulse);

            _knockback = force.x;
            StartCoroutine(ResetKnockbackValue(0.5f));
        }

        private IEnumerator ResetKnockbackValue(float delay)
        {
            float elapsedTime = 0;
            float startKnockbackValue = _knockback;

            while (elapsedTime < delay)
            {
                _knockback = Mathf.Lerp(startKnockbackValue, 0, elapsedTime / delay);
                elapsedTime += Time.deltaTime;

                yield return null;
            }

            _knockback = 0;
        }

        private (Vector2 pointA, Vector2 pointB) CalculateGroundCheckAreaPoints(
            Vector2 mainObjectPosition, Vector2 colliderSize, float areaHeight)
        {
            Vector2 colliderHalfSize = colliderSize / 2f;

            Vector2 pointA = mainObjectPosition - colliderHalfSize;
            Vector2 pointB = new Vector2(pointA.x + colliderSize.x, pointA.y - areaHeight);

            return (pointA, pointB);
        }
    }
}