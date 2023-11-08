using UnityEngine;

namespace FlappyTerminator
{
    public class EnemyMover : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private void FixedUpdate()
        {
            transform.Translate(transform.right * _speed * Time.fixedDeltaTime, Space.World);
        }
    }
}