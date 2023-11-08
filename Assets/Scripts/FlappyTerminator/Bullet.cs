using System.Collections;
using UnityEngine;

namespace FlappyTerminator
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float _movementSpeed;
        [SerializeField] private float _selfDestructDelay;

        private WaitForSeconds _selfDestructDelaySaved;
        private Coroutine _deactivator;
        private Entity _owner;
        private Vector3 _movementDirection;

        private void Awake()
        {
            _selfDestructDelaySaved = new WaitForSeconds(_selfDestructDelay);
        }

        private void OnEnable()
        {
            _deactivator = StartCoroutine(Deactivate(_selfDestructDelaySaved));
        }

        private void FixedUpdate()
        {           
            transform.Translate(_movementDirection * _movementSpeed * Time.fixedDeltaTime, Space.World);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == _owner.gameObject)
                return;

            if (_owner.TryGetComponent(out Player ownerPlayer) && collision.TryGetComponent(out Enemy enemy))
            {
                ownerPlayer.IncreaseScore();
                enemy.Die();
            }
            else if (collision.TryGetComponent(out Player player))
            {
                player.Die();
            }
            else
            {
                return;
            }

            StopCoroutine(_deactivator);
            gameObject.SetActive(false);
        }

        public void Initialize(Entity owner, Vector2 movementDirection)
        {
            _owner = owner;
            _movementDirection = movementDirection;

            float angle = Vector2.SignedAngle(Vector2.right, movementDirection);
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private IEnumerator Deactivate(WaitForSeconds waitForSeconds)
        {
            yield return waitForSeconds;
            gameObject.SetActive(false);
        }
    }
}