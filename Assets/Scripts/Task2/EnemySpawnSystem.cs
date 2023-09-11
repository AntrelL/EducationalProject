using System.Collections;
using UnityEngine;

public class EnemySpawnSystem : MonoBehaviour
{
    [SerializeField] [Tooltip("In seconds")] private float _spawnDelay;

    private EnemySpawner[] _enemySpawners;

    private void Start()
    {
        _enemySpawners = GetComponentsInChildren<EnemySpawner>();

        StartCoroutine(CyclicallySpawnEnemies(_spawnDelay));
    }

    private IEnumerator CyclicallySpawnEnemies(float spawnDelay)
    {
        var waitForSeconds = new WaitForSeconds(spawnDelay);

        while (true)
        {
            SpawnEnemy();

            yield return waitForSeconds;
        }
    }

    private void SpawnEnemy()
    {
        if (_enemySpawners.Length == 0)
            return;

        int spawnerNumber = Random.Range(0, _enemySpawners.Length);

        _enemySpawners[spawnerNumber].Spawn();
    }
}
