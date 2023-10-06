using System.Collections;
using System.Linq;
using UnityEngine;

namespace Runner
{
    public class Spawner : ObjectPool
    {
        [SerializeField] private float _secondsBetweenSpawn;
        [SerializeField] private GameObject[] _prefabs;

        private Transform[] _spawnPoints;

        private void Start()
        {
            _spawnPoints = GetComponentsInChildren<Transform>().Skip(1).ToArray();
            Initialize(_prefabs);

            StartCoroutine(Spawn(_secondsBetweenSpawn));
        }

        private IEnumerator Spawn(float secondsBetweenSpawn)
        {
            var waitForSeconds = new WaitForSeconds(secondsBetweenSpawn);

            while (true)
            {
                GameObject gameObject = GetObject();
                int spawnPointNumber = Random.Range(0, _spawnPoints.Length);

                SetObject(gameObject, _spawnPoints[spawnPointNumber].position);

                yield return waitForSeconds;
            }
        }

        private void SetObject(GameObject gameObject, Vector3 spawnPosition)
        {
            gameObject.SetActive(true);
            gameObject.transform.position = spawnPosition;
        }
    }
}