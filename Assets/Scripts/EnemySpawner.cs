using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _directionPointer;

    private float _horizontalDirection;

    private void Start()
    {
        _horizontalDirection = _directionPointer.rotation.eulerAngles.y;
    }

    public void Spawn(Enemy enemyPattern)
    {
        Enemy enemy = Instantiate(enemyPattern, transform.position, Quaternion.Euler(Vector3.up * _horizontalDirection));

        Vector3 enemyPosition = transform.position;
        enemyPosition.y = enemy.Size.y / 2f;
        
        enemy.transform.position = enemyPosition;

        enemy.StartMovement();
    }
}
