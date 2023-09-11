using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _enemyTarget;
    [SerializeField] private Enemy _enemyTemplate;

    public void Spawn()
    {
        Enemy enemy = Instantiate(_enemyTemplate, transform.position, Quaternion.identity);

        Vector3 enemyPosition = transform.position;
        enemyPosition.y = enemy.Size.y / 2f;
        
        enemy.transform.position = enemyPosition;

        enemy.StartMovement(_enemyTarget);
    }
}
