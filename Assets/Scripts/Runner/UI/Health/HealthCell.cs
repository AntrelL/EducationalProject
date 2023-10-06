using UnityEngine;

namespace Runner
{
    [RequireComponent(typeof(Animator))]
    public class HealthCell : MonoBehaviour
    {
        private readonly int RemoveId = Animator.StringToHash("Remove");

        private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void Destroy()
        {
            _animator.SetTrigger(RemoveId);
            Destroy(gameObject, _animator.GetCurrentAnimatorClipInfo(0).Length);
        }
    }
}