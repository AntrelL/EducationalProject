using UnityEngine;

namespace Runner
{
    public class HealthReplenisher : SideMovementObject
    {
        [SerializeField] private int _healingValue;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Player player))
                player.ApplyHeal(_healingValue);

            Die();
        }
    }
}