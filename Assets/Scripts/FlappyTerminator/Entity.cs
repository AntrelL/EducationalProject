using UnityEngine;

namespace FlappyTerminator
{
    public class Entity : MonoBehaviour
    {
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private BulletStorage _bulletStorage;

        public void Initialize(BulletStorage bulletStorage)
        {
            _bulletStorage = bulletStorage;
        }

        protected void Shoot(Vector2 direction)
        {
            Bullet bullet = _bulletStorage.GetBullet();

            bullet.Initialize(this, direction);
            bullet.transform.position = _shootPoint.position;

            bullet.gameObject.SetActive(true);
        }
    }
}