using UnityEngine;
using Runner;

namespace FlappyTerminator
{
    public class BulletStorage : ObjectPool<Bullet>
    {
        [SerializeField] private Bullet _bulletTemplate;

        private void Start()
        {
            Initialize(new Bullet[] { _bulletTemplate });
        }

        public Bullet GetBullet() => GetObject();

        public void ResetBullets() => ResetPool();
    }
}