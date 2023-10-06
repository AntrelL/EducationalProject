using UnityEngine;

namespace Runner
{
    public class Asteroid : SideMovementObject
    {
        [SerializeField] private int _damage;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
                player.ApplyDamage(_damage);

            Die();
        }
    }
}