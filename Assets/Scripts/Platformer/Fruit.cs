using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Animator))]
    public class Fruit : MonoBehaviour
    {
        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
            {
                player.TakeFruit(this);

                _animator.SetTrigger("Take");
                Destroy(gameObject, _animator.GetCurrentAnimatorClipInfo(0).Length);
            }
        }
    }
}