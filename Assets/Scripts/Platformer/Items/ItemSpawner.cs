using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Platformer
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField][Range(0, 1)] private float _itemQuantitySpawnFactor;
        [SerializeField] private Transform _itemSpawnPointStorage;
        [SerializeField] private Transform _itemContainer;
        [SerializeField] private Item[] _itemTemplates;

        private List<Vector2> _itemSpawnPositions;

        private void Start()
        {
            if (_itemSpawnPointStorage == null)
                _itemSpawnPointStorage = GetComponent<Transform>();

            _itemSpawnPositions = _itemSpawnPointStorage.GetComponentsInChildren<Transform>()
                .Select(transform => new Vector2(transform.position.x, transform.position.y))
                .Skip(1).ToList();

            int amountOfFruit = (int)(_itemQuantitySpawnFactor * _itemSpawnPositions.Count);
            List<Vector2> availableFruitSpawnPositions = new List<Vector2>(_itemSpawnPositions);

            for (int i = 0; i < amountOfFruit; i++)
            {
                int positionIndex = Random.Range(0, amountOfFruit - i);
                int templateIndex = Random.Range(0, _itemTemplates.Length);

                Instantiate(_itemTemplates[templateIndex], availableFruitSpawnPositions[positionIndex],
                    Quaternion.identity, _itemContainer);

                availableFruitSpawnPositions.RemoveAt(positionIndex);
            }
        }
    }
}
