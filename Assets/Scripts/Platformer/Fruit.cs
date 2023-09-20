using UnityEngine;

namespace Platformer
{
    [RequireComponent(typeof(Animator))]
    public class Fruit : MonoBehaviour
    {
        private readonly int TakeId = Animator.StringToHash("Take");

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void Take()
        {
            _animator.SetTrigger(TakeId);
            Destroy(gameObject, _animator.GetCurrentAnimatorClipInfo(0).Length);
        }
    }
}