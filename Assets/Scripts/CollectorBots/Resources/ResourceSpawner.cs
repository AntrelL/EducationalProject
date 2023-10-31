using UnityEngine;
using Runner;
using System.Collections;

namespace CollectorBots
{
    public class ResourceSpawner : ObjectPool<Resource>
    {
        [SerializeField] private float _delay;
        [SerializeField] private Resource _resourceTemplate;
        [SerializeField] private float _sideOfOuterSquareLimitationZone;
        [SerializeField] private float _sideOfInnerSquareLimitationZone;
        [SerializeField] private float _spawnHeight;

        private void Start()
        {
            Initialize(new Resource[] { _resourceTemplate });
            StartCoroutine(SpawnCycles());
        }

        private IEnumerator SpawnCycles()
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(_delay);

            while (true)
            {
                SpawnResource(_sideOfOuterSquareLimitationZone, _sideOfInnerSquareLimitationZone);

                yield return waitForSeconds;
            }
        }

        private void SpawnResource(float sideOfOuterSquareLimitationZone, float sideOfInnerSquareLimitationZone)
        {
            Vector3 position = Vector3.zero;
            position.y = _spawnHeight;

            float halfOfOuterSide = sideOfOuterSquareLimitationZone / 2f;
            position.x = Random.Range(halfOfOuterSide, -halfOfOuterSide);

            float halfOfInnerSide = sideOfInnerSquareLimitationZone / 2f;

            if (position.x < halfOfInnerSide && position.x > -halfOfInnerSide)
            {
                float firstOption = Random.Range(halfOfInnerSide, halfOfOuterSide);
                float secondOption = Random.Range(-halfOfOuterSide, -halfOfInnerSide);

                bool decision = System.Convert.ToBoolean(Random.Range(0, 2));

                position.z = decision ? firstOption : secondOption;
            }
            else
            {
                position.z = Random.Range(halfOfOuterSide, -halfOfOuterSide);
            }

            Resource resource = GetObject();

            resource.transform.position = position;
            resource.gameObject.SetActive(true);
        }
    }
}