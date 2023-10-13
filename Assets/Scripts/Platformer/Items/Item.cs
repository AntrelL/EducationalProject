using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Animator))]
    public abstract class Item : MonoBehaviour
    {
        private readonly int TakeId = Animator.StringToHash("Take");

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public virtual void Take(Player player)
        {
            _animator.SetTrigger(TakeId);
            Destroy(gameObject, _animator.GetCurrentAnimatorClipInfo(0).Length);
        }
    }
}