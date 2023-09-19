using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    public class Movement : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _jumpForce;
        [SerializeField] private MotionAnimator _motionAnimator;

        private Rigidbody2D _rigidbody2D;
        private BoxCollider2D _boxCollider2D;

        private bool _animated;
        private bool _grounded;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _animated = _motionAnimator != null;
        }

        protected virtual void Update()
        {
            if (_animated)
            {
                _motionAnimator.VerticalVelocity = _rigidbody2D.velocity.y;
                _motionAnimator.Grounded = _grounded;
            }
        }

        protected virtual void FixedUpdate()
        {
            float groundCheckAreaHight = 0.5f;

            (Vector2 groundCheckAreaPointA, Vector2 groundCheckAreaPointB) = 
                CalculateGroundCheckAreaPoints(transform.position, _boxCollider2D.size, groundCheckAreaHight);

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
            _rigidbody2D.velocity = new Vector2(horizontalVelocity, _rigidbody2D.velocity.y);

            if (_animated)
                _motionAnimator.HorizontalVelocity = horizontalVelocity;
        }

        protected void Jump()
        {
            if (_grounded)
                _rigidbody2D.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
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