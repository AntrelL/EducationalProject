using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;

    private Rigidbody _rigidbody;
    private CapsuleCollider _collider;

    private Vector3 _movementDirection;
    private bool _move = false;

    public Vector3 Size => _collider.bounds.size;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _collider = GetComponent<CapsuleCollider>();
    }

    private void FixedUpdate()
    {
        if (_move)
            Move();
    }

    public void StartMovement() => StartMovement(transform.forward);

    public void StartMovement(Vector3 movementDirection)
    {
        _movementDirection = movementDirection.normalized;
        _move = true;
    }

    public void StopMovement()
    {
        _move = false;
    }

    private void Move()
    {
        Vector3 velocity = _movementDirection * _movementSpeed * Time.fixedDeltaTime;
        _rigidbody.MovePosition(transform.position + velocity);
    }
}
