using Runner;
using System.Collections;
using UnityEngine;

namespace FlappyTerminator
{
    public class EnemySpawner : ObjectPool<Enemy>
    {
        [SerializeField] private Enemy[] _enemyTemplates;
        [SerializeField] private BulletStorage _bulletStorage;
        [SerializeField] private float _spawnDelay;
        [SerializeField] private float _maxSpawnHeight;
        [SerializeField] private float _minSpawnHeight;

        private void Start()
        {
            Initialize(_enemyTemplates);
            StartCoroutine(ShootCyclically(_spawnDelay));
        }

        public void ResetEnemies() => ResetPool();

        private IEnumerator ShootCyclically(float delay)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(delay);

            while (true)
            {
                Spawn();
                yield return waitForSeconds;
            }
        }

        private void Spawn()
        {
            Enemy enemy = GetObject();

            float positionY = transform.position.y + Random.Range(_minSpawnHeight, _maxSpawnHeight);
            enemy.transform.position = new Vector3(transform.position.x, positionY, enemy.transform.position.z);

            enemy.Initialize(_bulletStorage);
            enemy.gameObject.SetActive(true);

            DisableOffScreenEnemies();
        }
    }
}