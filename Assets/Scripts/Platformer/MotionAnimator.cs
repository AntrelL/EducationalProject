using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class MotionAnimator : MonoBehaviour
    {
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        public float HorizontalVelocity
        {
            set
            {
                _animator.SetFloat("HorizontalSpeed", Mathf.Abs(value));
                _spriteRenderer.flipX = value == 0 ? _spriteRenderer.flipX : value < 0;
            }
        }

        public float VerticalVelocity
        {
            set
            {
                _animator.SetFloat("VerticalVelocity", value);
            }
        }

        public bool Grounded
        {
            set
            {
                _animator.SetBool("Grounded", value);
            }
        }

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }
}
