using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Platformer
{
    public class FruitSpawner : MonoBehaviour
    {
        [SerializeField][Range(0, 1)] private float _fruitQuantitySpawnFactor;
        [SerializeField] private Transform _fruitSpawnPointStorage;
        [SerializeField] private Transform _fruitContainer;
        [SerializeField] private Fruit _fruitTemplate;

        private List<Vector2> _fruitSpawnPositions;

        private void Start()
        {
            if (_fruitSpawnPointStorage == null)
                _fruitSpawnPointStorage = GetComponent<Transform>();

            _fruitSpawnPositions = _fruitSpawnPointStorage.GetComponentsInChildren<Transform>()
                .Select(transform => new Vector2(transform.position.x, transform.position.y))
                .Skip(1).ToList();

            int amountOfFruit = (int)(_fruitQuantitySpawnFactor * _fruitSpawnPositions.Count);
            List<Vector2> availableFruitSpawnPositions = new List<Vector2>(_fruitSpawnPositions);

            for (int i = 0; i < amountOfFruit; i++)
            {
                int positionIndex = Random.Range(0, amountOfFruit - i);

                Instantiate(_fruitTemplate, availableFruitSpawnPositions[positionIndex],
                    Quaternion.identity, _fruitContainer);

                availableFruitSpawnPositions.RemoveAt(positionIndex);
            }
        }
    }
}
