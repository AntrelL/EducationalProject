using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Navigator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Enemy : Entity
    {
        [SerializeField] private LayerMask _detectionLayerMask;

        private Navigator _navigator;
        private SpriteRenderer _spriteRenderer;

        protected override void Start()
        {
            base.Start();

            _navigator = GetComponent<Navigator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if (TrySeePlayer(out Player player))
                MoveTo(player.transform.position);
            else
                MoveTo(_navigator.CurrentTargetPoint.Position);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.TryGetComponent(out Player player))
            {
                Vector2 playerKnockback = CalculateKnockback(RecliningForce, player.transform.position);
                player.ApplyDamage(Damage, playerKnockback);
            }
        }

        private void MoveTo(Vector3 targetPosition)
        {
            Vector2 movementDirection = (targetPosition - transform.position).normalized;
            Move(movementDirection.x);
        }

        private bool TrySeePlayer(out Player player)
        {
            float raycastDistance = 10f;
            Vector2 raycastDirection = _spriteRenderer.flipX ? Vector2.left : Vector2.right;

            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, raycastDirection,
                raycastDistance, _detectionLayerMask);

            player = null;
            return raycastHit ? raycastHit.collider.TryGetComponent(out player) : false;
        }
    }
}