using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Enemy : MovementObject
{
    private Collider _collider;
    private Transform _target;

    public Vector3 Size => _collider.bounds.size;

    protected override void Awake()
    {
        base.Awake();
        _collider = GetComponent<Collider>();
    }

    private void FixedUpdate()
    {
        MoveTo(_target.position);
    }

    public void StartMovement(Transform target)
    {
        _target = target;
    }
}
