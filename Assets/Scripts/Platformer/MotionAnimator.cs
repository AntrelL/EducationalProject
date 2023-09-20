using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class MotionAnimator : MonoBehaviour
    {
        private readonly int HorizontalSpeedId = Animator.StringToHash("HorizontalSpeed");
        private readonly int VerticalVelocityId = Animator.StringToHash("VerticalVelocity");
        private readonly int GroundedId = Animator.StringToHash("Grounded");

        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        public float HorizontalVelocity
        {
            set
            {
                _animator.SetFloat(HorizontalSpeedId, Mathf.Abs(value));
                _spriteRenderer.flipX = value == 0 ? _spriteRenderer.flipX : value < 0;
            }
        }

        public float VerticalVelocity
        {
            set
            {
                _animator.SetFloat(VerticalVelocityId, value);
            }
        }

        public bool Grounded
        {
            set
            {
                _animator.SetBool(GroundedId, value);
            }
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}
