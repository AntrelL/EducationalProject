using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    [SerializeField] [Tooltip("In seconds")] private float _spawnDelay;
    [SerializeField] private Enemy _enemyTemplate;

    private EnemySpawner[] _enemySpawners;
    private float _elapsedSpawnDelayTime = 0;

    private void Start()
    {
        _enemySpawners = GetComponentsInChildren<EnemySpawner>();
    }

    private void Update()
    {
        if (_elapsedSpawnDelayTime >= _spawnDelay)
        {
            SpawnEnemy();
            _elapsedSpawnDelayTime = 0;
        }
        else
        {
            _elapsedSpawnDelayTime += Time.deltaTime;
        }
    }

    private void SpawnEnemy()
    {
        if (_enemySpawners.Length == 0)
            return;

        int spawnerNumber = Random.Range(0, _enemySpawners.Length);

        _enemySpawners[spawnerNumber].Spawn(_enemyTemplate);
    }
}
