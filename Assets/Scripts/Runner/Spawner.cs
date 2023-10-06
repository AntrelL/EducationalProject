using System.Collections;
using System.Linq;
using UnityEngine;

namespace Runner
{
    public class Spawner : ObjectPool<SideMovementObject>
    {
        [SerializeField] private float _delay;
        [SerializeField] private SideMovementObject[] _prefabs;

        private Transform[] _spawnPoints;

        private void Start()
        {
            _spawnPoints = GetComponentsInChildren<Transform>().Skip(1).ToArray();
            Initialize(_prefabs);

            StartCoroutine(Spawn(_delay));
        }

        private IEnumerator Spawn(float delay)
        {
            var waitForSeconds = new WaitForSeconds(delay);

            while (true)
            {
                SideMovementObject sideMovementObject = GetObject();
                int spawnPointNumber = Random.Range(0, _spawnPoints.Length);

                SetObject(sideMovementObject.gameObject, _spawnPoints[spawnPointNumber].position);

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