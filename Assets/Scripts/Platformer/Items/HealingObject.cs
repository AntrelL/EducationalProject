using UnityEngine;

namespace Platformer
{
    public class HealingObject : Item
    {
        [SerializeField] private int _healingValue;

        public override void Take(Player player)
        {
            player.ApplyHeal(_healingValue);

            base.Take(player);
        }
    }
}