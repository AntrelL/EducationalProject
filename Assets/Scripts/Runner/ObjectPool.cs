using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runner
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private int _capacity;

        private List<GameObject> _pool = new List<GameObject>();
        private GameObject[] _sparePrefabs;

        public void Initialize(GameObject[] prefabs)
        {
            int objectTypeCounter = 0;

            for (int i = 0; i < _capacity; i++)
            {
                if (objectTypeCounter == prefabs.Length)
                    objectTypeCounter = 0;

                InitializeOneObject(prefabs[objectTypeCounter++]);
            }

            Shuffle(_pool);
            _sparePrefabs = prefabs;
        }

        public GameObject GetObject()
        {
            GameObject result = _pool.FirstOrDefault(savedObject => savedObject.activeSelf == false);

            if (result == null)
            {
                result = InitializeOneRandomObject(_sparePrefabs);
                _capacity++;
            }

            return result;
        }

        private GameObject InitializeOneRandomObject(GameObject[] prefabs)
        {
            int randomIndex = Random.Range(0, prefabs.Length);

            return InitializeOneObject(prefabs[randomIndex]);
        }

        private GameObject InitializeOneObject(GameObject prefab)
        {
            GameObject spawned = Instantiate(prefab, _container);
            spawned.SetActive(false);

            _pool.Add(spawned);
            return spawned;
        }

        private void Shuffle<T>(List<T> list)
        {
            for (int i = list.Count - 1; i >= 1; i--)
            {
                int j = Random.Range(0, i + 1);

                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}