using UnityEngine;

namespace Runner
{
    public class SideMovementObject : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed;

        private void Update()
        {
            transform.Translate(Vector2.left * _moveSpeed * Time.deltaTime);
        }

        protected void Die()
        {
            gameObject.SetActive(false);
        }
    }
}