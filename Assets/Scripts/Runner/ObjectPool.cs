using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Runner
{
    public class ObjectPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private Transform _container;
        [SerializeField] private int _capacity;

        private List<T> _pool = new List<T>();
        private T[] _sparePrefabs;
        private Camera _camera;

        protected void Initialize(T[] prefabs)
        {
            _camera = Camera.main;

            int objectTypeCounter = 0;

            for (int i = 0; i < _capacity; i++)
            {
                if (objectTypeCounter == prefabs.Length)
                    objectTypeCounter = 0;

                InitializeOneObject(prefabs[objectTypeCounter++]);
            }

            if (prefabs.Length > 1)
                Shuffle(_pool);

            _sparePrefabs = prefabs;
        }

        protected T GetObject()
        {
            T result = _pool.FirstOrDefault(savedObject => savedObject.gameObject.activeSelf == false);

            if (result == null)
            {
                result = InitializeOneRandomObject(_sparePrefabs);
                _capacity++;
            }

            return result;
        }

        protected void DisableOffScreenEnemies()
        {
            Vector3 disablePoint = _camera.ViewportToWorldPoint(new Vector2(0f, 0.5f));

            foreach (T item in _pool)
            {
                if (item.gameObject.activeSelf == true && item.transform.position.x < disablePoint.x)
                {
                    item.gameObject.SetActive(false);
                }
            }
        }

        protected void ResetPool()
        {
            foreach (T item in _pool)
            {
                item.gameObject.SetActive(false);
            }
        }

        private T InitializeOneRandomObject(T[] prefabs)
        {
            int randomIndex = Random.Range(0, prefabs.Length);

            return InitializeOneObject(prefabs[randomIndex]);
        }

        private T InitializeOneObject(T prefab)
        {
            T spawned = Instantiate(prefab, _container);
            spawned.gameObject.SetActive(false);

            _pool.Add(spawned);
            return spawned;
        }

        private void Shuffle(List<T> list)
        {
            for (int i = list.Count - 1; i >= 1; i--)
            {
                int j = Random.Range(0, i + 1);

                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}