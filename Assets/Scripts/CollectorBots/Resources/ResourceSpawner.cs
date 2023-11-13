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
            WaitForSeconds delay = new WaitForSeconds(_delay);

            while (true)
            {
                SpawnResource(_sideOfOuterSquareLimitationZone, _sideOfInnerSquareLimitationZone);

                yield return delay;
            }
        }

        private void SpawnResource(float sideOfOuterSquareLimitationZone, float sideOfInnerSquareLimitationZone)
        {
            Vector3 position = Vector3.zero;
            position.y = _spawnHeight;

            float numberOfPartsToDivideSides = 2f;

            float partOfOuterSide = sideOfOuterSquareLimitationZone / numberOfPartsToDivideSides;
            position.x = Random.Range(partOfOuterSide, -partOfOuterSide);

            float partOfInnerSide = sideOfInnerSquareLimitationZone / numberOfPartsToDivideSides;

            if (position.x < partOfInnerSide && position.x > -partOfInnerSide)
            {
                float firstOption = Random.Range(partOfInnerSide, partOfOuterSide);
                float secondOption = Random.Range(-partOfOuterSide, -partOfInnerSide);

                bool decision = System.Convert.ToBoolean(Random.Range(0, 2));

                position.z = decision ? firstOption : secondOption;
            }
            else
            {
                position.z = Random.Range(partOfOuterSide, -partOfOuterSide);
            }

            Resource resource = GetObject();

            resource.transform.position = position;
            resource.gameObject.SetActive(true);
        }
    }
}