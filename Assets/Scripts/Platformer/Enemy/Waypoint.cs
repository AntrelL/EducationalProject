using UnityEngine;

namespace Platformer
{
    public class Waypoint : MonoBehaviour
    {
        public Vector2 Position => transform.position;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Navigator navigator))
                navigator.OnTouchingWithPoint(this);
        }
    }
}