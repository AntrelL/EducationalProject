using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Navigator))]
    public class Enemy : Movement
    {
        private Navigator _navigator;

        private void Start()
        {
            _navigator = GetComponent<Navigator>();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            MoveTo(_navigator.CurrentTargetPoint.Position);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent<Player>(out _))
                Destroy(collision.gameObject);
        }

        private void MoveTo(Vector3 targetPosition)
        {
            Vector2 movementDirection = (targetPosition - transform.position).normalized;
            Move(movementDirection.x);
        }
    }
}